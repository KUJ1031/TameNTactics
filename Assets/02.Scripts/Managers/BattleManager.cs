using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    public List<Monster> EntryMonsters => PlayerManager.Instance.player.battleEntry;
    public List<Monster> BenchMonsters => PlayerManager.Instance.player.benchEntry;
    public List<Monster> OwnedMonsters => PlayerManager.Instance.player.ownedMonsters;

    public List<Monster> enemyTeam;

    public Monster selectedPlayerMonster;
    public SkillData selectedSkill;

    public bool battleEnded = false;

    public void StartBattle()
    {
       // InitializeTeams();
        InitializeUltimateSkill(EntryMonsters);
        InitializeUltimateSkill(enemyTeam);
    }

    public void SelectPlayerMonster(Monster selectedMonster)
    {
        if (battleEnded || selectedMonster.CurHp <= 0) return;
        selectedPlayerMonster = selectedMonster;
        // ShowSkillSelectionUI(selectedMonster.skills);
    }

    public void SelectSkill(SkillData skill)
    {
        selectedSkill = skill;
        
        List<Monster> possibleTargets = new();

        if (skill.isTargetSelf && !skill.isAreaAttack)
        {
            possibleTargets = new List<Monster> { selectedPlayerMonster };
        }
        else if (skill.isTargetSingleAlly)
        {
            possibleTargets = EntryMonsters.Where(m => m.CurHp > 0).ToList();
        }
        else if (skill.isAreaAttack)
        {
            possibleTargets = skill.isTargetSelf
                ? EntryMonsters.Where(m => m.CurHp > 0).ToList()
                : enemyTeam.Where(m => m.CurHp > 0).ToList();
        }
        else
        {
            possibleTargets = enemyTeam.Where(m => m.CurHp > 0).ToList();
        }

        // ShowTargetSelectionUI(possibleTargets);
    }

    public void SelectTargetMonster(Monster target)
    {
        if (target.CurHp <= 0) return;

        List<Monster> selectedTargets = selectedSkill.isAreaAttack
            ? (selectedSkill.isTargetSelf ? EntryMonsters : enemyTeam).Where(m => m.CurHp > 0).ToList()
            : new List<Monster> { target };

        var enemyAction = EnemyAIController.DecideAction(enemyTeam, EntryMonsters);

        bool playerGoesFirst = selectedPlayerMonster.Speed >= enemyAction.actor.Speed;

        if (playerGoesFirst)
        {
            ExecuteSkill(selectedPlayerMonster, selectedSkill, selectedTargets);
            if (IsTeamDead(enemyTeam)) { EndBattle(true); return; }

            ExecuteSkill(enemyAction.actor, enemyAction.selectedSkill, enemyAction.targets);
            if (IsTeamDead(EntryMonsters)) { EndBattle(false); return; }
        }
        else
        {
            ExecuteSkill(enemyAction.actor, enemyAction.selectedSkill, enemyAction.targets);
            if (IsTeamDead(EntryMonsters)) { EndBattle(false); return; }

            ExecuteSkill(selectedPlayerMonster, selectedSkill, selectedTargets);
            if (IsTeamDead(enemyTeam)) { EndBattle(true); return; }
        }
    }

    public void ExecuteSkill(Monster caster, SkillData skill, List<Monster> targets)
    {
        if (caster.CurHp <= 0 || targets == null || targets.Count == 0) return;

        foreach (var target in targets.Where(t => t.CurHp > 0))
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skill);
            Debug.Log($"{caster.monsterName}가 {target.monsterName}에게 {result.damage} 데미지 (치명타: {result.isCritical}, 상성: {result.effectiveness})");

            target.TakeDamage(result.damage);

            IncreaseUltimateCost(target);
        }

        IncreaseUltimateCost(caster);
    }

    public void CaptureSelectedEnemy(Monster target)
    {
        if (target.CurHp <= 0)
        {
            Debug.Log($"{target.monsterName}는 이미 쓰러져 포획할 수 없습니다.");
            return;
        }

        var player = PlayerManager.Instance.player;

        if (player.entryMonsters.Count < 5)
        {
            player.entryMonsters.Add(target);
        }
        else
        {
            player.ownedMonsters.Add(target);
        }

        Debug.Log($"{target.monsterName}를 포획했습니다!");
    }

    public void BattleReward()
    {
        int totalExp = enemyTeam.Sum(e => e.ExpReward);
        int getBenchExp = Mathf.RoundToInt(totalExp * 0.7f);
        int totalGold = enemyTeam.Sum(e => e.GoldReward);

        PlayerManager.Instance.player.gold += totalGold;

        foreach (var monster in EntryMonsters.Where(m => m.CurHp > 0))
            monster.AddExp(totalExp);

        foreach (var monster in BenchMonsters.Where(m => m.CurHp > 0))
            monster.AddExp(getBenchExp);
    }

    public void EndTurn()
    {
        IncreaseUltimateCostAll(EntryMonsters);
        IncreaseUltimateCostAll(enemyTeam);
    }

    private bool IsTeamDead(List<Monster> team)
    {
        return team.All(m => m.CurHp <= 0);
    }

    private void EndBattle(bool playerWin)
    {
        battleEnded = true;
        Debug.Log(playerWin ? "승리!" : "패배!");
        // 전투 종료 UI 호출
    }

    // private void ShowSkillSelectionUI(List<SkillData> skills)
    // {
    //     // 스킬 선택 UI 필요 합니당!
    //     // 스킬 이름, 설명 등
    // }
    //
    // // 타겟을 나타내줘요!
    // private void ShowTargetSelectionUI(List<MonsterData> targets)
    // {
    //     // 타겟 선택 영역 표시 필요해욤!
    // }


    public void InitializeUltimateSkill(List<Monster> team)
    {
        foreach (var monster in team)
        {
            foreach (var skill in monster.skills)
            {
                if (skill.skillType == SkillType.UltimateSkill)
                {
                    skill.curUltimateCost = 0;
                }
            }
        }
    }

    public void IncreaseUltimateCost(Monster monster)
    {
        foreach (var skill in monster.skills)
        {
            if (skill.skillType == SkillType.UltimateSkill)
            {
                skill.curUltimateCost = Mathf.Min(skill.maxUltimateCost, skill.curUltimateCost + 1);
            }
        }
    }

    public void IncreaseUltimateCostAll(List<Monster> team)
    {
        foreach (var monster in team)
        {
            foreach (var skill in monster.skills)
            {
                if (skill.skillType == SkillType.UltimateSkill)
                {
                    skill.curUltimateCost = Mathf.Min(skill.maxUltimateCost, skill.curUltimateCost + 1);
                }
            }
        }
    }

    public void InitializeTeams()
    {
        enemyTeam = BattleTriggerManager.Instance.GetEnemyTeam();

        if (EntryMonsters == null || enemyTeam == null)
        {
            Debug.LogError("플레이어 팀 또는 적 팀이 설정되지 않았습니다!");
            return;
        }

        Debug.Log($"플레이어 팀 멤버: {string.Join(", ", EntryMonsters.Select(m => m.monster.monsterName))}");
        Debug.Log($"벤치 몬스터: {string.Join(", ", BenchMonsters.Select(m => m.monster.monsterName))}");
        Debug.Log($"적 팀 멤버: {string.Join(", ", enemyTeam.Select(m => m.monster.monsterName))}");
        UnityEngine.SceneManagement.SceneManager.LoadScene("BattleUITest");
    }

    public void CancelSelectedMonster()
    {
        selectedPlayerMonster = null;
    }

    public void CancelSelectedSkill()
    {
        selectedSkill = null;
    }

    public bool TryRunAway()
    {
        float chance = 0.5f;
        bool success = Random.value < chance;
        Debug.Log(success ? "도망 성공!" : "도망 실패!");
        return success;
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    public List<Monster> BattleEntry => PlayerManager.Instance.player.battleEntry;
    public List<Monster> BenchMonsters => PlayerManager.Instance.player.benchEntry;
    public List<Monster> OwnedMonsters => PlayerManager.Instance.player.ownedMonsters;

    public List<Monster> enemyTeam;

    public Monster selectedPlayerMonster;
    public SkillData selectedSkill;

    public bool battleEnded = false;
    
    public void StartBattle()
    {
       // InitializeTeams();
       // 전투 시작시 초기화 되는 것들 적용 해야 되는것들 추가
        InitializeUltimateSkill(BattleEntry);
        InitializeUltimateSkill(enemyTeam);
        
    }

    public void SelectPlayerMonster(Monster selectedMonster)
    {
        if (selectedMonster.CurHp <= 0) return;
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
            possibleTargets = BattleEntry.Where(m => m.CurHp > 0).ToList();
        }
        else if (skill.isAreaAttack)
        {
            possibleTargets = skill.isTargetSelf
                ? BattleEntry.Where(m => m.CurHp > 0).ToList()
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
            ? (selectedSkill.isTargetSelf ? BattleEntry : enemyTeam).Where(m => m.CurHp > 0).ToList()
            : new List<Monster> { target };

        var enemyAction = EnemyAIController.DecideAction(enemyTeam, BattleEntry);

        bool playerGoesFirst = selectedPlayerMonster.Speed >= enemyAction.actor.Speed;

        if (playerGoesFirst)
        {
            ExecuteSkill(selectedPlayerMonster, selectedSkill, selectedTargets);
            if (IsTeamDead(enemyTeam)) { EndBattle(true); return; }

            ExecuteSkill(enemyAction.actor, enemyAction.selectedSkill, enemyAction.targets);
            if (IsTeamDead(BattleEntry)) { EndBattle(false); return; }
        }
        else
        {
            ExecuteSkill(enemyAction.actor, enemyAction.selectedSkill, enemyAction.targets);
            if (IsTeamDead(BattleEntry)) { EndBattle(false); return; }

            ExecuteSkill(selectedPlayerMonster, selectedSkill, selectedTargets);
            if (IsTeamDead(enemyTeam)) { EndBattle(true); return; }
        }
    }

    public void ExecuteSkill(Monster caster, SkillData skill, List<Monster> targets)
    {
        if (caster.CurHp <= 0 || targets == null || targets.Count == 0) return;
        
        ISkillEffect effect = SkillFactory.GetSkillEffect(skill);
        if (effect == null) return;
        
        effect.Execute(caster, targets);
        
        IncreaseUltimateCost(caster);
        foreach (var t in targets)
        {
            IncreaseUltimateCost(t);
        }
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

        foreach (var monster in BattleEntry.Where(m => m.CurHp > 0))
            monster.AddExp(totalExp);

        foreach (var monster in BenchMonsters.Where(m => m.CurHp > 0))
            monster.AddExp(getBenchExp);
    }

    public void IncreaseUltCostAllMonsters()
    {
        IncreaseUltimateCostAll(BattleEntry);
        IncreaseUltimateCostAll(enemyTeam);
    }

    public bool IsTeamDead(List<Monster> team)
    {
        if (team.Count == 0 || team.All(m => m.CurHp <= 0))
        {
            return true;
        }
        
        return false;
    }

    public void EndBattle(bool playerWin)
    {
        battleEnded = true;
        Debug.Log(playerWin ? "승리!" : "패배!");
        // 전투 종료 UI 호출
    }
    
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
    
    public bool TryRunAway()
    {
        float chance = 0.5f;
        bool success = Random.value < chance;
        Debug.Log(success ? "도망 성공!" : "도망 실패!");
        return success;
    }
}

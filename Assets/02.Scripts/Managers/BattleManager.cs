using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    public List<Monster> BattleEntry => PlayerManager.Instance.player.battleEntry;
    public List<Monster> BenchMonsters => PlayerManager.Instance.player.benchEntry;
    public List<Monster> OwnedMonsters => PlayerManager.Instance.player.ownedMonsters;

    public List<Monster> enemyTeam;

    public List<Monster> BattleEntryTeam { get; private set; } = new();
    public List<Monster> BattleEnemyTeam { get; private set; } = new();

    public List<Monster> possibleTargets = new();
    public List<Monster> selectedTargets = new();

    public Monster selectedPlayerMonster;
    public SkillData selectedSkill;

    public bool battleEnded = false;

    // 배틀 시작시 배틀에 나오는 몬스터 찾아서 리스트에 넣어줌
    public void FindSpawnMonsters()
    {
        BattleEntryTeam.Clear();
        BattleEnemyTeam.Clear();

        GameObject allyObj = GameObject.Find("AllySpawner");
        GameObject enemyObj = GameObject.Find("EnemySpawner");

        if (allyObj == null || enemyObj == null) return;

        Transform allySpawner = allyObj.transform;
        Transform enemySpawner = enemyObj.transform;

        foreach (Transform spawnPoint in allySpawner)
        {
            MonsterCharacter monsterChar = spawnPoint.GetComponentInChildren<MonsterCharacter>();
            if (monsterChar != null && monsterChar.monster != null)
            {
                BattleEntryTeam.Add(monsterChar.monster);
            }
        }

        foreach (Transform spawnPoint in enemySpawner)
        {
            MonsterCharacter monsterChar = spawnPoint.GetComponentInChildren<MonsterCharacter>();
            if (monsterChar != null && monsterChar.monster != null)
            {
                BattleEnemyTeam.Add(monsterChar.monster);
            }
        }
    }

    public void StartBattle()
    {
        InitializeUltCost(BattleEntryTeam);
        InitializeUltCost(BattleEnemyTeam);

        foreach (var monster in BattleEntryTeam)
        {
            monster.InitializeBattleStats();
            monster.InitializePassiveSkills();
            monster.TriggerOnBattleStart(BattleEntryTeam);
            Debug.Log($"Entry Monster의 현재 최대 체력 : {monster.CurMaxHp}");
            Debug.Log($"Entry Monster의 현재 최대 궁극기 게이지 : {monster.MaxUltimateCost}");
        }

        foreach (var monster in BattleEnemyTeam)
        {
            monster.RecalculateStats();
            monster.InitializeBattleStats();
            monster.InitializePassiveSkills();
            monster.TriggerOnBattleStart(BattleEnemyTeam);
            Debug.Log($"Enemy Monster의 현재 최대 체력 : {monster.CurMaxHp}");
            Debug.Log($"Enemy Monster의 현재 최대 궁극기 게이지 : {monster.MaxUltimateCost}");
        }
    }

    public void EndTurn()
    {
        foreach (var monster in BattleEntryTeam)
            monster.TriggerOnTurnEnd();

        foreach (var monster in BattleEnemyTeam)
            monster.TriggerOnTurnEnd();
    }

    public void DealDamage(Monster target, int damage, Monster attacker)
    {
        target.TakeDamage(damage);
        target.TriggerOnDamaged(damage, attacker);
    }

    public void SelectPlayerMonster(Monster selectedMonster)
    {
        if (selectedMonster.CurHp <= 0) return;
        selectedPlayerMonster = selectedMonster;
    }

    public void SelectSkill(SkillData skill)
    {
        selectedSkill = skill;
        selectedTargets.Clear();

        var alivePlayerTeam = BattleEntryTeam.Where(m => m.CurHp > 0).ToList();
        var aliveEnemyTeam = BattleEnemyTeam.Where(m => m.CurHp > 0).ToList();

        switch (skill.targetScope)
        {
            case TargetScope.Self:
                possibleTargets = new List<Monster> { selectedPlayerMonster };
                selectedTargets = new List<Monster> { selectedPlayerMonster };
                ExecuteSkill(selectedPlayerMonster, selectedSkill, selectedTargets);
                EnemyAttackAfterPlayerTurn();
                return;

            case TargetScope.All:
                possibleTargets = alivePlayerTeam.Concat(aliveEnemyTeam).ToList();
                selectedTargets = new List<Monster>(possibleTargets);
                ExecuteSkill(selectedPlayerMonster, selectedSkill, selectedTargets);
                EnemyAttackAfterPlayerTurn();
                return;

            case TargetScope.EnemyTeam:
                possibleTargets = aliveEnemyTeam;
                break;

            case TargetScope.PlayerTeam:
                possibleTargets = alivePlayerTeam;
                break;

            default:
                possibleTargets = new List<Monster>();
                break;
        }

        // targetCount가 0이면 전체 대상으로 설정
        if (skill.targetCount == 0)
        {
            selectedTargets = new List<Monster>(possibleTargets);
            ExecuteSkill(selectedPlayerMonster, selectedSkill, selectedTargets);
            EnemyAttackAfterPlayerTurn();
        }
    }



    public void SelectTargetMonster(Monster target)
    {
        if (target.CurHp <= 0 || !possibleTargets.Contains(target)) return;

        if (selectedTargets.Contains(target))
        {
            selectedTargets.Remove(target); // 타겟 카운트 2 이상일때 다시 클릭하면 선택 취소
            return;
        }

        if (selectedTargets.Count >= selectedSkill.targetCount) return;

        selectedTargets.Add(target);

        if (selectedTargets.Count == selectedSkill.targetCount)
        {
            ExecuteSkill(selectedPlayerMonster, selectedSkill, selectedTargets);
            EnemyAttackAfterPlayerTurn();
        }
    }


    public void ExecuteSkill(Monster caster, SkillData skill, List<Monster> targets)
    {
        if (caster.CurHp <= 0 || targets == null || targets.Count == 0) return;

        ISkillEffect effect = null;
        
        if (!caster.canAct) return;

        if (skill.skillType == SkillType.UltimateSkill)
        {
            effect = UltimateSkillFactory.GetUltimateSkill(skill);
            caster.UseUltimateCost();
        }
        else
        {
            effect = NormalSkillFactory.GetNormalSkill(skill);
        }

        if (effect == null) return;

        effect.Execute(caster, targets);

        IncreaseUltCost(caster);
        foreach (var t in targets)
        {
            IncreaseUltCost(t);
        }
    }

    public void CaptureSelectedEnemy(Monster target)
    {
        if (target.CurHp <= 0)
        {
            Debug.Log($"{target.monsterName}는 이미 쓰러져 포획할 수 없습니다.");
            return;
        }

        if (BenchMonsters.Count < 2)
        {
            BenchMonsters.Add(target);
        }
        else
        {
            OwnedMonsters.Add(target);
        }

        Debug.Log($"{target.monsterName}를 포획했습니다!");
    }

    public void BattleReward()
    {
        int totalExp = BattleEnemyTeam.Sum(e => e.ExpReward);
        int getBenchExp = Mathf.RoundToInt(totalExp * 0.7f);
        int totalGold = BattleEnemyTeam.Sum(e => e.GoldReward);

        PlayerManager.Instance.player.gold += totalGold;

        foreach (var monster in BattleEntryTeam.Where(m => m.CurHp > 0))
            monster.AddExp(totalExp);

        foreach (var monster in BenchMonsters.Where(m => m.CurHp > 0))
            monster.AddExp(getBenchExp);
    }

    public void IncreaseUltCostAllMonsters()
    {
        IncreaseUltimateCostTeam(BattleEntryTeam);
        IncreaseUltimateCostTeam(BattleEnemyTeam);
    }

    public bool IsTeamDead(List<Monster> team)
    {
        return team.All(m => m.CurHp <= 0);
    }

    public void EndBattle(bool playerWin)
    {
        battleEnded = true;
        Debug.Log(playerWin ? "승리!" : "패배!");
        // 전투 종료 UI 호출
    }

    public void InitializeUltCost(List<Monster> team)
    {
        foreach (var monster in team)
        {
            monster.InitializeUltimateCost();
        }
    }

    public void IncreaseUltCost(Monster monster)
    {
        monster.IncreaseUltimateCost();
    }

    public void IncreaseUltimateCostTeam(List<Monster> team)
    {
        foreach (var monster in team)
        {
            monster.IncreaseUltimateCost();
        }
    }

    public bool TryRunAway()
    {
        foreach (var monster in BattleEntryTeam)
        {
            if (monster.TryRunAwayWithPassive(out bool isGuaranteed) && isGuaranteed)
            {
                Debug.Log("도망 100% 성공!");
                return true;
            }
        }

        float chance = 0.5f;
        bool success = Random.value < chance;
        Debug.Log(success ? "도망 성공!" : "도망 실패!");
        return success;
    }
    
    private void EnemyAttackAfterPlayerTurn()
    {
        var enemyAction = EnemyAIController.DecideAction(BattleEnemyTeam, BattleEntryTeam);

        if (IsTeamDead(BattleEnemyTeam)) { EndBattle(true); return; }

        ExecuteSkill(enemyAction.actor, enemyAction.selectedSkill, enemyAction.targets);

        if (IsTeamDead(BattleEntryTeam)) { EndBattle(false); return; }

        EndTurn();
        IncreaseUltCostAllMonsters();
    }
}

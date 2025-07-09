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

    public string previousSceneName;

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

    // 배틀 시작시 실행
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

    // 턴 끝날 때 실행
    public void EndTurn()
    {
        foreach (var monster in BattleEntryTeam.Concat(BattleEnemyTeam))
        {
            monster.TriggerOnTurnEnd();
            monster.UpdateStatusEffects();
        }
    }

    // 데미지 넣기 + 데미지 후 패시브 발동
    public void DealDamage(Monster target, int damage, Monster attacker)
    {
        target.TakeDamage(damage);
        target.TriggerOnDamaged(damage, attacker);
    }

    // 공격 실행할 몬스터 고르기
    public void SelectPlayerMonster(Monster selectedMonster)
    {
        if (selectedMonster.CurHp <= 0) return;
        selectedPlayerMonster = selectedMonster;
    }

    // 스킬 고르기
    public void SelectSkill(SkillData skill)
    {
        selectedSkill = skill;
        selectedTargets.Clear();

        Debug.Log($"스킬의 타입 : {skill.skillType}");

        var alivePlayerTeam = BattleEntryTeam.Where(m => m.CurHp > 0).ToList();
        var aliveEnemyTeam = BattleEnemyTeam.Where(m => m.CurHp > 0).ToList();

        // 고른 스킬의 타겟 유형에 따라 바로 실행
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

    // 타겟이 될 몬스터 고르기
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

    // 사용 할 스킬 종류에 따라 스킬 발동
    public void ExecuteSkill(Monster caster, SkillData skill, List<Monster> targets)
    {
        if (caster.CurHp <= 0 || targets == null || targets.Count == 0) return;

        ISkillEffect effect = null;

        if (!caster.canAct) return;

        if (skill.skillType == SkillType.UltimateSkill && caster.Level >= 15)
        {
            effect = UltimateSkillFactory.GetUltimateSkill(skill);
            caster.InitializeUltimateCost();
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

    // 선택한 몬스터 잡기
    public void CaptureSelectedEnemy(Monster target)
    {
        if (target.CurHp <= 0)
        {
            Debug.Log($"{target.monsterName}는 이미 쓰러져 포획할 수 없습니다.");
            return;
        }

        if (BenchMonsters.Count < 2)
        {
            BenchMonsters.Add(target); // 벤치가 비어있으면 우선으로
        }
        else
        {
            OwnedMonsters.Add(target); // 엔트릴 5마리 꽉 찼으면 전체몬스터안으로
        }

        GameObject enemyObj = GameObject.Find("EnemySpawner");

        if (enemyObj == null) return;

        Transform enemySpawner = enemyObj.transform;

        foreach (Transform spawnPoint in enemySpawner)
        {
            MonsterCharacter monsterChar = spawnPoint.GetComponentInChildren<MonsterCharacter>();
            if (monsterChar.monster == target && monsterChar.monster.CurHp > 0)
            {
                Destroy(monsterChar.gameObject);
                break;
            }
        }

        RuntimePlayerSaveManager.Instance.SaveBattleGameState(PlayerManager.Instance.player);

        Debug.Log($"{target.monsterName}를 포획했습니다!");
    }

    // 배틀 승리시 보상
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

    // 모든 몬스터 궁극기 코스트 1개씩 증가
    public void IncreaseUltCostAllMonsters()
    {
        IncreaseUltimateCostTeam(BattleEntryTeam);
        IncreaseUltimateCostTeam(BattleEnemyTeam);
    }

    // 팀이 전체 죽었는지 체크
    public bool IsTeamDead(List<Monster> team)
    {
        return team.All(m => m.CurHp <= 0);
    }

    // 배틀이 끝나고 true 플레이팀 승리, false 적팀 승리
    public void EndBattle(bool playerWin)
    {
        battleEnded = true;
        Debug.Log(playerWin ? "승리!" : "패배!");
        // 전투 종료 UI 호출
    }

    // 배틀 시작시 궁극기 0으로 초기화
    public void InitializeUltCost(List<Monster> team)
    {
        foreach (var monster in team)
        {
            monster.InitializeUltimateCost();
        }
    }

    // 몬스터 궁극기 코스트 1개 증가
    public void IncreaseUltCost(Monster monster)
    {
        monster.IncreaseUltimateCost();
    }

    // 정해진 팀 전체 궁극기 코스트 1개씩 증가
    public void IncreaseUltimateCostTeam(List<Monster> team)
    {
        foreach (var monster in team)
        {
            monster.IncreaseUltimateCost();
        }
    }

    // 도망가기
    public bool TryRunAway()
    {
        foreach (var monster in BattleEntryTeam)
        {
            // 도망가기 마스터 패시브 있을시 100% 도망 성공
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

    // 플레이어 행동 선택 후 적 죽었는지 판단 후 공격
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

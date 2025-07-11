using System.Collections;
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

    private EnemyAIController.EnemyAction enemyChosenAction;

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
        enemyChosenAction = null;

        var alivePlayer = BattleEntryTeam.Where(m => m.CurHp > 0).ToList();
        var aliveEnemy = BattleEnemyTeam.Where(m => m.CurHp > 0).ToList();

        switch (skill.targetScope)
        {
            case TargetScope.Self:
                possibleTargets = new() { selectedPlayerMonster };
                selectedTargets = new(possibleTargets);
                StartCoroutine(CompareSpeedAndFight());
                return;

            case TargetScope.All:
                possibleTargets = alivePlayer.Concat(aliveEnemy).ToList();
                selectedTargets = new(possibleTargets);
                StartCoroutine(CompareSpeedAndFight());
                return;

            case TargetScope.EnemyTeam:
                possibleTargets = aliveEnemy;
                break;

            case TargetScope.PlayerTeam:
                possibleTargets = alivePlayer;
                break;

            default:
                possibleTargets = new();
                break;
        }

        if (skill.targetCount == 0)
        {
            selectedTargets = new(possibleTargets);
            StartCoroutine(CompareSpeedAndFight());
        }
    }

    // 타겟이 될 몬스터 고르기
    public void SelectTargetMonster(Monster target)
    {
        if (target.CurHp <= 0 || !possibleTargets.Contains(target)) return;

        if (selectedTargets.Contains(target))
        {
            selectedTargets.Remove(target);
            return;
        }

        if (selectedTargets.Count < selectedSkill.targetCount)
        {
            selectedTargets.Add(target);
        }

        if (selectedTargets.Count == selectedSkill.targetCount &&
            selectedTargets.All(t => t.CurHp > 0))
        {
            StartCoroutine(CompareSpeedAndFight());
        }
    }

    // 속도 비교해서 누가 먼저 공격하는지 정함
    private IEnumerator CompareSpeedAndFight()
    {
        if (enemyChosenAction == null)
            enemyChosenAction = EnemyAIController.DecideAction(BattleEnemyTeam, BattleEntryTeam);

        bool playerStunned = BattleEntryTeam.All(m => !m.canAct);
        if (playerStunned)
        {
            EnemyAttackAfterPlayerTurn();
            yield break;
        }

        bool playerFirst = selectedPlayerMonster.CurSpeed >= enemyChosenAction.actor.CurSpeed;

        if (playerFirst)
        {
            yield return StartCoroutine(ExecuteSkill(selectedPlayerMonster, selectedSkill, selectedTargets));
            if (IsTeamDead(BattleEnemyTeam))
            {
                EndBattle(true);
                BattleSystem.Instance.ChangeState(new EndBattleState(BattleSystem.Instance));
                yield break;
            }

            yield return StartCoroutine(ExecuteSkill(
                enemyChosenAction.actor, enemyChosenAction.selectedSkill, enemyChosenAction.targets));
            if (IsTeamDead(BattleEntryTeam))
            {
                EndBattle(false);
                BattleSystem.Instance.ChangeState(new EndBattleState(BattleSystem.Instance));
                yield break;
            }
        }
        else
        {
            yield return StartCoroutine(ExecuteSkill(
                enemyChosenAction.actor, enemyChosenAction.selectedSkill, enemyChosenAction.targets));
            if (IsTeamDead(BattleEntryTeam))
            {
                EndBattle(false);
                BattleSystem.Instance.ChangeState(new EndBattleState(BattleSystem.Instance));
                yield break;
            }

            yield return StartCoroutine(ExecuteSkill(selectedPlayerMonster, selectedSkill, selectedTargets));
            if (IsTeamDead(BattleEnemyTeam))
            {
                EndBattle(true);
                BattleSystem.Instance.ChangeState(new EndBattleState(BattleSystem.Instance));
                yield break;
            }
        }

        EndTurn();
        yield return StartCoroutine(IncreaseUltCostAllMonsters());
        ClearSelections();
        BattleSystem.Instance.ChangeState(new PlayerMenuState(BattleSystem.Instance));
    }

    // 사용 할 스킬 종류에 따라 스킬 발동
    private IEnumerator ExecuteSkill(Monster caster, SkillData skill, List<Monster> targets)
    {
        Debug.Log("스킬사용!");

        if (!caster.canAct || caster.CurHp <= 0 || targets == null || targets.Count == 0) yield break;

        ISkillEffect effect = null;

        if (skill.skillType == SkillType.UltimateSkill && caster.Level >= 15)
        {
            effect = UltimateSkillFactory.GetUltimateSkill(skill);
            caster.InitializeUltimateCost();
        }
        else
        {
            effect = NormalSkillFactory.GetNormalSkill(skill);
        }

        if (effect == null) yield break;

        yield return StartCoroutine(effect.Execute(caster, targets));

        IncreaseUltCost(caster);
        foreach (var t in targets)
        {
            IncreaseUltCost(t);
        }

        yield return new WaitForSeconds(1f);
    }

    // 선택한 몬스터 잡기
    public void CaptureSelectedEnemy(Monster target)
    {
        foreach (var monster in BattleEntryTeam)
        {
            if (monster == target) return;
        }
        if (target.CurHp <= 0)
        {
            Debug.Log($"{target.monsterName}는 이미 쓰러져 포획할 수 없습니다.");
            return;
        }
        else
        {
            OwnedMonsters.Add(target);
        }

        GameObject enemyObj = GameObject.Find("EnemySpawner");

        if (enemyObj == null) return;

        Transform enemySpawner = enemyObj.transform;

        foreach (Transform spawnPoint in enemySpawner)
        {
            MonsterCharacter monsterChar = spawnPoint.GetComponentInChildren<MonsterCharacter>();

            if (monsterChar == null) continue;

            if (monsterChar.monster == target && monsterChar.monster.CurHp > 0)
            {
                UIManager.Instance.battleUIManager.RemoveGauge(monsterChar.monster);
                BattleEnemyTeam.Remove(target);
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
    private IEnumerator IncreaseUltCostAllMonsters()
    {
        IncreaseUltimateCostTeam(BattleEntryTeam);
        IncreaseUltimateCostTeam(BattleEnemyTeam);

        yield return new WaitForSeconds(0.5f);
    }

    // 팀이 전체 죽었는지 체크
    public bool IsTeamDead(List<Monster> team)
    {
        return team.All(m => m.CurHp <= 0) || team.Count <= 0;
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

        float chance = 0.8f;
        bool success = Random.value < chance;
        Debug.Log(success ? "도망 성공!" : "도망 실패!");
        return success;
    }

    // 플레이어 행동 선택 후 적 죽었는지 판단 후 공격
    public void EnemyAttackAfterPlayerTurn()
    {
        StartCoroutine(EnemyAttackAfterPlayerTurnCoroutine());
    }

    private IEnumerator EnemyAttackAfterPlayerTurnCoroutine()
    {
        Debug.Log("EnemyAttackAfterPlayerTurn");
        var enemyAction = EnemyAIController.DecideAction(BattleEnemyTeam, BattleEntryTeam);

        yield return StartCoroutine(ExecuteSkill(enemyAction.actor, enemyAction.selectedSkill, enemyAction.targets));

        if (IsTeamDead(BattleEntryTeam))
        {
            EndBattle(false);
            BattleSystem.Instance.ChangeState(new EndBattleState(BattleSystem.Instance));
            yield break;
        }

        EndTurn();
        yield return StartCoroutine(IncreaseUltCostAllMonsters());
        ClearSelections();
    }

    // 한턴이 지났을때 선택했던 스킬, 타겟 등등 리셋
    private void ClearSelections()
    {
        selectedPlayerMonster = null;
        selectedSkill = null;
        selectedTargets.Clear();
        enemyChosenAction = null;
    }

    public List<MonsterCharacter> CheckPossibleTargets()
    {
        List<MonsterCharacter> characters = new();

        // 씬에 있는 모든 MonsterCharacter 찾아오기
        var allCharacters = GameObject.FindObjectsOfType<MonsterCharacter>();

        foreach (var character in allCharacters)
        {
            if (BattleManager.Instance.possibleTargets.Contains(character.monster))
            {
                characters.Add(character);
            }
        }

        return characters;
    }
}

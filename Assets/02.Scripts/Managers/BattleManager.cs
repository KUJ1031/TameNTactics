using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    protected override bool IsDontDestroy => true;
    public List<Monster> BattleEntry => PlayerManager.Instance.player.battleEntry;
    public List<Monster> BenchMonsters => PlayerManager.Instance.player.benchEntry;
    public List<Monster> OwnedMonsters => PlayerManager.Instance.player.ownedMonsters;

    public List<Monster> enemyTeam;

    public List<Monster> BattleEntryTeam { get; private set; } = new();
    public List<Monster> BattleEnemyTeam { get; private set; } = new();
    public List<Monster> DeadEntryMonsters { get; private set; } = new();
    public List<Monster> DeadEnemyMonsters { get; private set; } = new();

    public List<Monster> possibleActPlayerMonsters = new();
    public List<Monster> possibleTargets = new();
    public List<Monster> selectedTargets = new();

    public Monster selectedPlayerMonster;
    public SkillData selectedSkill;
    public Transform AttackPosition { get; set; }

    private EnemyAIController.EnemyAction enemyChosenAction;

    public string previousSceneName;

    public bool isCoroutineOver = false;

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
        InitializeDeadMonsters();

        foreach (var monster in BattleEntryTeam)
        {
            monster.InitializeBattleStart();
            monster.TriggerOnBattleStart(BattleEntryTeam);
            Debug.Log($"Entry Monster의 현재 최대 체력 : {monster.CurMaxHp}");
            Debug.Log($"Entry Monster의 현재 최대 궁극기 게이지 : {monster.MaxUltimateCost}");
            Debug.Log($"이름 : {monster.monsterName}\n공격력 : {monster.CurAttack}\n방어력 : {monster.CurDefense}\n치명타확률 : {monster.CurCriticalChance}\n스피드 : {monster.CurSpeed}");
        }

        foreach (var monster in BattleEnemyTeam)
        {
            monster.RecalculateStats();
            monster.InitializeBattleStart();
            monster.TriggerOnBattleStart(BattleEnemyTeam);
            Debug.Log($"Enemy Monster의 현재 최대 체력 : {monster.CurMaxHp}");
            Debug.Log($"Enemy Monster의 현재 최대 궁극기 게이지 : {monster.MaxUltimateCost}");
        }

        ClearSelections();
    }

    // 턴 끝날 때 실행
    public void EndTurn()
    {
        foreach (var monster in BattleEntryTeam.Concat(BattleEnemyTeam))
        {
            monster.TriggerOnTurnEnd();
            monster.UpdateStatusEffects();
            monster.UpdateBuffEffects();
            monster.CheckMonsterAction();
        }

        isCoroutineOver = true;
        BattleSystem.Instance.ChangeState(new PlayerMenuState(BattleSystem.Instance));
    }

    // 데미지 넣기 + 데미지 후 패시브 발동
    public void DealDamage(Monster target, int damage, Monster attacker, SkillData skillData, bool isCrit, float effectiveness)
    {
        int finalDamage = target.TriggerOnDamaged(damage, attacker);
        var team = BattleEntryTeam.Contains(target) ? BattleEntryTeam : BattleEnemyTeam;

        if (skillData.targetCount == 1 || skillData.targetCount == 2)
        {
            foreach (var monster in team)
            {
                foreach (var passive in monster.PassiveSkills)
                {
                    if (passive is InterceptDamage)
                    {
                        InterceptDamage(monster, skillData, finalDamage, team);
                        StartCoroutine(attacker.TriggerOnAttack(attacker, finalDamage, monster, skillData, effectiveness));
                        BattleDialogueManager.Instance.UseSkillDialogue(attacker, monster, finalDamage, skillData);
                        return;
                    }
                }
            
                foreach (var buff in monster.ActiveBuffEffects)
                {
                    if (buff.Type == BuffEffectType.Taunt)
                    {
                        monster.TakeDamage(finalDamage);
                        StartCoroutine(attacker.TriggerOnAttack(attacker, finalDamage, monster, skillData, effectiveness));
                        BattleDialogueManager.Instance.UseSkillDialogue(attacker, monster, finalDamage, skillData);
                        return;
                    }
                }
            }
        }
        
        target.TakeDamage(finalDamage);
        NotifyReceivedCrit(target, isCrit);
        StartCoroutine(attacker.TriggerOnAttack(attacker, finalDamage, target, skillData, effectiveness));
        BattleDialogueManager.Instance.UseSkillDialogue(attacker, target, finalDamage, skillData);
    }

    public void PossibleActMonster()
    {
        possibleActPlayerMonsters.Clear();

        foreach (var monster in BattleEntryTeam)
        {
            if (monster.canAct && monster.CurHp > 0)
            {
                possibleActPlayerMonsters.Add(monster);
            }
        }
    }

    // 공격 실행할 몬스터 고르기
    public void SelectPlayerMonster(Monster selectedMonster)
    {
        if (selectedMonster.CurHp <= 0 || !selectedMonster.canAct) return;
        selectedPlayerMonster = selectedMonster;
    }

    // 스킬 고르기
    public void SelectSkill(SkillData skill)
    {
        if (skill.skillType == SkillType.UltimateSkill && selectedPlayerMonster.Level < 15)
        {
            Debug.Log($"레벨이 낮아 궁극기를 사용할 수 없습니다.");
            return;
        }

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

                if (aliveEnemy.Count == 1)
                {
                    if (PlayerManager.Instance.player.playerBattleTutorialCheck)
                    {
                        selectedTargets.Add(aliveEnemy[0]);
                        StartCoroutine(CompareSpeedAndFight());

                        UIManager.Instance.battleUIManager.BattleSelectView.HideSkillPanel();
                        UIManager.Instance.battleUIManager.BattleSelectView.HideSelectPanel();
                        UIManager.Instance.battleUIManager.SkillView.HideActiveSkillTooltip();
                        return;
                    }
                }

                BattleSystem.Instance.ChangeState(new SelectTargetState(BattleSystem.Instance));
                break;

            case TargetScope.PlayerTeam:
                possibleTargets = alivePlayer;
                BattleSystem.Instance.ChangeState(new SelectTargetState(BattleSystem.Instance));
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
            UIManager.Instance.battleUIManager.HidePossibleTargets();
            StartCoroutine(CompareSpeedAndFight());
        }
    }



    // 속도 비교해서 누가 먼저 공격하는지 정함
    private IEnumerator CompareSpeedAndFight()
    {
        UIManager.Instance.battleUIManager.DeselectAllMonsters();
        UIManager.Instance.battleUIManager.OnActionComplete();
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
                yield break;
            }

            yield return StartCoroutine(ExecuteSkill(
                enemyChosenAction.actor, enemyChosenAction.selectedSkill, enemyChosenAction.targets));
            if (IsTeamDead(BattleEntryTeam))
            {
                EndBattle(false);
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
                yield break;
            }

            yield return StartCoroutine(ExecuteSkill(selectedPlayerMonster, selectedSkill, selectedTargets));
            if (IsTeamDead(BattleEnemyTeam))
            {
                EndBattle(true);
                yield break;
            }
        }

        yield return StartCoroutine(IncreaseUltCostAllMonsters());
        EndTurn();
        ClearSelections();
    }

    // 사용 할 스킬 종류에 따라 스킬 발동
    private IEnumerator ExecuteSkill(Monster caster, SkillData skill, List<Monster> targets)
    {
        Debug.Log("스킬사용!");

        if (!caster.canAct || caster.CurHp <= 0 || targets == null || targets.Count == 0) yield break;

        MonsterCharacter casterChar = FindMonsterCharacter(caster);
        List<MonsterCharacter> targetChars = new();

        foreach (var t in targets)
        {
            MonsterCharacter targetChar = FindMonsterCharacter(t);
            if (targetChar != null) targetChars.Add(targetChar);
        }

        if (casterChar != null)
        {
            Vector2 originalPos = casterChar.transform.position;
            Vector2 attackPos = AttackPosition.transform.position;

            casterChar.PlayAttack();

            yield return StartCoroutine(MoveToPosition(casterChar, attackPos, 0.3f, false));
            casterChar.PlayIdle();

            ISkillEffect effect = null;

            if (skill.skillType == SkillType.UltimateSkill)
            {
                effect = UltimateSkillFactory.GetUltimateSkill(skill);
                caster.InitializeUltimateCost();
                NotifyUltUsed(caster, BattleEntryTeam.Contains(caster) ? BattleEntryTeam : BattleEnemyTeam);
            }
            else
            {
                effect = NormalSkillFactory.GetNormalSkill(skill);
            }

            if (effect != null)
            {
                yield return StartCoroutine(effect.Execute(caster, targets));
                SkillEffectController.PlayEffect(skill, casterChar, targetChars);

                foreach (var target in targets)
                {
                    yield return StartCoroutine(UIManager.Instance.battleUIManager.WaitHpBarAnimationDone(target));
                }
            }

            yield return StartCoroutine(MoveToPosition(casterChar, originalPos, 0.3f, true));
        }

        CheckDeadMonster();
        IncreaseUltCost(caster);
        foreach (var t in targets)
        {
            IncreaseUltCost(t);
            Stage1BossBattleCheck(caster, t);
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
            PlayerManager.Instance.player.AddOwnedMonster(target);
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
                BattleEnemyTeam.Remove(target);
                Destroy(monsterChar.gameObject);
                break;
            }
        }

        RuntimePlayerSaveManager.Instance.SaveBattleGameState(PlayerManager.Instance.player);

        Debug.Log($"{target.monsterName}를 포획했습니다!");
    }

    // 배틀 승리시 보상
    public (int totalExp, int totalGold) BattleReward()
    {
        int totalExp = DeadEnemyMonsters.Sum(e => e.ExpReward);
        int getBenchExp = Mathf.RoundToInt(totalExp * 0.7f);
        int totalGold = DeadEnemyMonsters.Sum(e => e.GoldReward);

        Debug.Log($"전투 보상: 경험치 {totalExp}, 벤치 몬스터 경험치 {getBenchExp}, 골드 {totalGold}");

        PlayerManager.Instance.player.AddGold(totalGold);

        foreach (var monster in BattleEntryTeam.Where(m => m.CurHp > 0))
            monster.AddExp(totalExp);

        foreach (var monster in BenchMonsters.Where(m => m.CurHp > 0))
            monster.AddExp(getBenchExp);

        return (totalExp, totalGold);
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
        BattleSystem.Instance.ChangeState(new EndBattleState(BattleSystem.Instance));
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
        isCoroutineOver = false;
        Debug.Log("EnemyAttackAfterPlayerTurn");
        var enemyAction = EnemyAIController.DecideAction(BattleEnemyTeam, BattleEntryTeam);

        yield return StartCoroutine(ExecuteSkill(enemyAction.actor, enemyAction.selectedSkill, enemyAction.targets));

        if (IsTeamDead(BattleEntryTeam))
        {
            EndBattle(false);
            yield break;
        }

        yield return StartCoroutine(IncreaseUltCostAllMonsters());
        EndTurn();
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
            if (possibleTargets.Contains(character.monster))
            {
                characters.Add(character);
            }
        }

        return characters;
    }

    public void CheckDeadMonster()
    {
        DeadEntryMonsters = BattleEntryTeam.Where(m => m.CurHp <= 0).ToList();
        DeadEnemyMonsters = BattleEnemyTeam.Where(m => m.CurHp <= 0).ToList();

        BattleEnemyTeam.RemoveAll(m => m.CurHp <= 0);
        BattleEntryTeam.RemoveAll(m => m.CurHp <= 0);

        bool isDeanDead = DeadEnemyMonsters.Any(m => m.monsterName == "Dean");
        bool isEisenDead = DeadEnemyMonsters.Any(m => m.monsterName == "Eisen");
        bool isDolanDead = DeadEnemyMonsters.Any(m => m.monsterName == "Dolan");
        if (isDeanDead)
        {
            Debug.Log("엘리트 Dean 처치");
            PlayerManager.Instance.player.playerEliteClearCheck[0] = true;
        }
        if (isEisenDead)
        {
            Debug.Log("엘리트 Eisen 처치");
            PlayerManager.Instance.player.playerEliteClearCheck[1] = true;
        }
        if (isDolanDead)
        {
            Debug.Log("엘리트 Dolan 처치");
            PlayerManager.Instance.player.playerEliteClearCheck[2] = true;
        }
    }

    // 공격중인 몬스터의 character를 가져오기 위한 메서드
    private MonsterCharacter FindMonsterCharacter(Monster monster)
    {
        var allChars = GameObject.FindObjectsOfType<MonsterCharacter>();
        foreach (var mc in allChars)
        {
            if (mc.monster == monster) return mc;
        }
        return null;
    }

    // 몬스터 공격 시 화면 중앙/원래 위치로 이동
    private IEnumerator MoveToPosition(MonsterCharacter character, Vector2 targetPos, float duration, bool setIdle = false)
    {
        Vector2 startPos = character.transform.position;
        float elapsed = 0f;

        var gaugeHolder = character.GetComponent<MonsterGaugeHolder>();
        RectTransform gaugeRect = null;
        Canvas parentCanvas = null;

        if (gaugeHolder != null && gaugeHolder.gauge != null)
        {
            gaugeRect = gaugeHolder.gauge.GetComponent<RectTransform>();
            parentCanvas = gaugeHolder.gauge.GetComponentInParent<Canvas>();
        }

        while (elapsed < duration)
        {
            character.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            character.PlayMove();

            yield return null;
        }

        character.transform.position = targetPos;

        if (setIdle) character.PlayIdle();

        yield return new WaitForSeconds(duration);
    }

    private void Stage1BossBattleCheck(Monster caster, Monster target)
    {
        if (target.monsterData.monsterNumber == 100)
        {
            caster.SetActionRestriction(1);
        }
    }

    private void InitializeDeadMonsters()
    {
        DeadEnemyMonsters.Clear();
        DeadEntryMonsters.Clear();
    }

    private void NotifyUltUsed(Monster ultimateUser, List<Monster> team)
    {
        foreach (var monster in team)
        {
            foreach (var passive in monster.PassiveSkills)
            {
                if (passive is PowerBoostPerUlt powerBoostPerUlt)
                {
                    powerBoostPerUlt.OnUseUlt(monster, ultimateUser, team);
                }
                else if (passive is CleanseSelfOnUlt cleanseSelfOnUlt)
                {
                    cleanseSelfOnUlt.OnUseUlt(ultimateUser);
                }
            }
        }
    }

    private void NotifyReceivedCrit(Monster self, bool isCritical)
    {
        foreach (var passive in self.PassiveSkills)
        {
            if (passive is CritUpOnCritHit critUpOnCritHit)
            {
                critUpOnCritHit.OnCritHit(self, isCritical);
            }
        }
    }

    private void InterceptDamage(Monster target, SkillData skillData, int damage, List<Monster> team)
    {
        foreach (var monster in team)
        {
            foreach (var passive in monster.PassiveSkills)
            {
                if (passive is InterceptDamage interceptDamage)
                {
                    interceptDamage.OnDamagedAlly(monster, target, skillData, damage);
                }
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    public List<MonsterData> playerTeam;
    public List<MonsterData> benchMonsters;
    public List<MonsterData> enemyTeam;
    public List<MonsterData> allPlayerMonsters;

    public MonsterData selectedPlayerMonster;
    public SkillData selectedSkill;
    
    public bool battleEnded = false;
    
    // 배틀 시작시 호출
    public void StartBattle()
    {
        InitializeTeams();
        InitializeUltimateSkill(playerTeam);
        InitializeUltimateSkill(enemyTeam);

        foreach (var monster in playerTeam)
        {
            MonsterStatsManager.RecalculateStats(monster);
        }
    }

    // 플레이어 몬스터 고르기
    public void SelectPlayerMonster(MonsterData selectedMonster)
    {
        if (battleEnded) return;
        if (selectedMonster.curHp <= 0) return;

        selectedPlayerMonster = selectedMonster;
        
        ShowSkillSelectionUI(selectedMonster.skills);
    }

    // 스킬 고르기
    public void SelectSkill(SkillData skill)
    {
        selectedSkill = skill;

        List<MonsterData> possibleTargets = new();

        if (skill.isTargetSelf && !skill.isAreaAttack)
        {
            possibleTargets = new List<MonsterData> { selectedPlayerMonster };
        }

        else if (skill.isTargetSingleAlly)
        {
            possibleTargets = playerTeam.Where(m => m.curHp > 0).ToList();
        }
        
        else if (skill.isAreaAttack)
        {
            if (skill.isTargetSelf)
            {
                possibleTargets = playerTeam.Where(m => m.curHp > 0).ToList();
            }

            else
            {
                possibleTargets = enemyTeam.Where(m => m.curHp > 0).ToList();
            }
        }

        else if (!skill.isTargetSelf && !skill.isAreaAttack && !skill.isTargetSingleAlly)
        {
            possibleTargets = enemyTeam.Where(m => m.curHp > 0).ToList();
        }
        
        ShowTargetSelectionUI(possibleTargets);
    }

    // 타겟 몬스터 선택
    public void SelectTargetMonster(MonsterData target)
    {
        if (target.curHp <= 0) return;
        
        List<MonsterData> selectedTargets = new();

        if (selectedSkill.isAreaAttack)
        {
            selectedTargets = selectedSkill.isTargetSelf
                ? playerTeam.Where(m => m.curHp > 0).ToList()
                : enemyTeam.Where(m => m.curHp > 0).ToList();
        }

        else
        {
            selectedTargets.Add(target);
        }

        var enemyAction = EnemyAIController.DecideAction(enemyTeam, playerTeam);

        bool playerGoesFirst = selectedPlayerMonster.speed >= enemyAction.actor.speed;

        if (playerGoesFirst)
        {
            ExecuteSkill(selectedPlayerMonster, selectedSkill, selectedTargets);
            if (IsTeamDead(playerTeam))
            {
                EndBattle(false);
                return;
            }

            ExecuteSkill(enemyAction.actor, enemyAction.selectedSkill,
                enemyAction.targets);
            if (IsTeamDead(enemyTeam))
            {
                EndBattle(true);
                return;
            }
        }

        else
        {
            ExecuteSkill(enemyAction.actor, enemyAction.selectedSkill,
                enemyAction.targets);
            if (IsTeamDead(enemyTeam))
            {
                EndBattle(true);
                return;
            }
            
            ExecuteSkill(selectedPlayerMonster, selectedSkill, selectedTargets);
            if (IsTeamDead(playerTeam))
            {
                EndBattle(false);
                return;
            }
        }
    }

    // 스킬 사용
    public void ExecuteSkill(MonsterData caster, SkillData skill, List<MonsterData> targets)
    {
        if (caster.curHp <= 0 || targets == null || targets.Count == 0) return;

        foreach (var target in targets.Where(t => t.curHp > 0))
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skill);
            Debug.Log($"{caster.name}가 {target.name}에게 {result.damage} 데미지 " +
                      $"(치명타: {result.isCritical},상성: {result.effectiveness})");
            target.curHp -= result.damage;
            if (target.curHp < 0) target.curHp = 0;
            
            IncreaseUltimateCost(target);
        }
        
        IncreaseUltimateCost(caster);
    }

    // 선택한 몬스터를 포획
    public void CaptureSelectedEnemy(MonsterData target)
    {
        if (target.curHp <= 0)
        {
            Debug.Log($"{target.monsterName}는 이미 쓰러져 포획할 수 없습니다.");
            return;
        }

        allPlayerMonsters.Add(target);
        Debug.Log($"{target.monsterName}를(을) 포획했습니다!" +
                  $"(현재 총 보유 수: {allPlayerMonsters.Count(m => m == target)})");
    }

    
    // 배틀 보상(경험치, 골드) 로직
    public void BattleReward(List<MonsterData> entryMonsters, List<MonsterData> benchMonsters)
    {
        int totalExp = enemyTeam.Sum(e => e.expReward);
        int getBenchExp = Mathf.RoundToInt(totalExp * 0.7f);
        int totalGold = enemyTeam.Sum(e => e.goldReward);
        
        // player.gold += totalGold;
        
        var aliveEntryMonsters = entryMonsters.Where(m => m.curHp > 0).ToList();
        var aliveBenchMonsters = benchMonsters.Where(m => m.curHp > 0).ToList();

        foreach (var monster in aliveEntryMonsters)
        {
            MonsterStatsManager.AddExp(monster, totalExp);
        }

        foreach (var monster in aliveBenchMonsters)
        {
            if (aliveBenchMonsters != null)
            {
                MonsterStatsManager.AddExp(monster, getBenchExp);
            }
        }
    }

    public void StartTurn()
    {
        IncreaseUltimateCostAll(playerTeam);
        IncreaseUltimateCostAll(enemyTeam);
    }
    
    // 팀 죽었나요?
    private bool IsTeamDead(List<MonsterData> team)
    {
        return team.All(m => m.curHp <= 0);
    }

    // 배틀이 끝났으면 판넬을 띄어주면 될거같아요!
    private void EndBattle(bool playerWin)
    {
        battleEnded = true;
        Debug.Log(playerWin ? "승리 판넬" : "패배 판넬");
    }
    
    // 스킬 목록 보여주세요!
    private void ShowSkillSelectionUI(List<SkillData> skills)
    {
        // 스킬 선택 UI 필요 합니당!
        // 스킬 이름, 설명 등
    }
    
    // 타겟을 나타내줘요!
    private void ShowTargetSelectionUI(List<MonsterData> targets)
    {
        // 타겟 선택 영역 표시 필요해욤!
    }

    // 호출 시 궁극기 코스트 0으로 초기화
    public void InitializeUltimateSkill(List<MonsterData> team)
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
    
    public void IncreaseUltimateCost(MonsterData monster)
    {
        foreach (var skill in monster.skills)
        {
            if (skill.skillType == SkillType.UltimateSkill)
            {
                skill.curUltimateCost = Mathf.Min(skill.maxUltimateCost, skill.curUltimateCost + 1);
            }
        }
    }
    
    // 모든 몬스터 궁극기 코스트 하나씩 증가
    public void IncreaseUltimateCostAll(List<MonsterData> team)
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
        playerTeam = BattleTriggerManager.Instance.GetPlayerTeam();
        enemyTeam = BattleTriggerManager.Instance.GetEnemyTeam();

        if (playerTeam == null || enemyTeam == null)
        {
            Debug.LogError("플레이어 팀 또는 적 팀이 설정되지 않았습니다!");
            return;
        }

        Debug.Log($"플레이어 팀 멤버: {string.Join(", ", playerTeam.Select(m => m.monsterName))}");
        Debug.Log($"적 팀 멤버: {string.Join(", ", enemyTeam.Select(m => m.monsterName))}");
    }
    
    public void CancelPlayerAction()
    {
        selectedPlayerMonster = null;
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

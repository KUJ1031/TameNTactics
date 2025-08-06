using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 본인이 살아있는 동안 팀 전체 방어력 10% 상승, 20레벨 20% 상승
public class AliveTeamGuard : IPassiveSkill
{
    private int lastBuffAmount = 0;
    
    public void OnBattleStart(Monster self, List<Monster> monsters)
    {
        lastBuffAmount = 0;
        
        List<Monster> team = BattleManager.Instance.BattleEntryTeam.Contains(self)
            ? BattleManager.Instance.BattleEntryTeam
            : BattleManager.Instance.BattleEnemyTeam;
        
        ApplyDefenseBuff(team, self);
    }

    public void OnTurnEnd(Monster self)
    {
        List<Monster> team = BattleManager.Instance.BattleEntryTeam.Contains(self)
            ? BattleManager.Instance.BattleEntryTeam
            : BattleManager.Instance.BattleEnemyTeam;
        
        if (self.CurHp <= 0)
        {
            RemoveDefenseBuff(team);
        }
    }

    private void ApplyDefenseBuff(List<Monster> team, Monster self)
    {
        float value = self.Level >= 20 ? 0.2f : 0.1f;

        foreach (var monster in team)
        {
            int amount = Mathf.RoundToInt(monster.CurDefense * value);
            lastBuffAmount = amount;
            
            Debug.Log($"이름 : {monster.monsterName}\n방어력 : {monster.CurDefense} 이건데");
            monster.BattleDefenseUp(amount);
            Debug.Log($"이름 : {monster.monsterName}\n{monster.CurDefense} 요렇게 됐슴둥");
        }
    }

    private void RemoveDefenseBuff(List<Monster> team)
    {
        foreach (var monster in team)
        {
            monster.BattleDefenseDown(lastBuffAmount);
        }

        lastBuffAmount = 0;
    }


    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}
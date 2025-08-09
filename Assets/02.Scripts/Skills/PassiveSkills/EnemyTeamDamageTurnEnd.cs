using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeamDamageTurnEnd : IPassiveSkill
{
    public void OnTurnEnd(Monster self)
    {
        var team = BattleManager.Instance.BattleEntryTeam.Contains(self)
            ? BattleManager.Instance.BattleEntryTeam
            : BattleManager.Instance.BattleEnemyTeam;

        foreach (var monster in team)
        {
            int amount = Mathf.RoundToInt(monster.CurMaxHp * 0.1f);
            monster.TakeDamage(amount);
        }
    }
    
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self, List<Monster> team) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

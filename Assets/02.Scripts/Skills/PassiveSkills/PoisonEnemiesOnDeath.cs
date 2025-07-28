using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEnemiesOnDeath : IPassiveSkill
{
    public void OnDeath(List<Monster> enemyTeam)
    {
        if (enemyTeam.Count == 0) return;
        
        foreach (var monster in enemyTeam)
        {
            monster.ApplyStatus(new Poison(3));
        }
    }
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill){}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritUpOnTurnEnd : IPassiveSkill
{
    public void OnTurnEnd(Monster self)
    {
        if (self.CurCriticalChance < 80)
        {
            self.BattleCritChanceUpWithLimit(5, 80);
        }
    }
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; } 
    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill) {}
}

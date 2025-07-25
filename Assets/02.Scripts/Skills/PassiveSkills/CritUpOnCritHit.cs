using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritUpOnCritHit : IPassiveSkill
{
    public void OnCritHit(Monster target, bool isCritical)
    {
        if (isCritical)
        {
            int amount = Mathf.RoundToInt(target.CurCriticalChance * 0.1f);
            target.BattleCritChanceUp(amount);
        }
    }
    
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill) {}
}

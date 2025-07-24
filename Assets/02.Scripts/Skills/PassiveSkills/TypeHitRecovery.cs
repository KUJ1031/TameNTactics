using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeHitRecovery : IPassiveSkill
{
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill)
    {
        var result = DamageCalculator.CalculateDamage(attacker, target, skill);
        if (result.effectiveness == 1.5f)
        {
            int amount = Mathf.RoundToInt(damage * 0.2f);
            attacker.Heal(amount);
        }
    }
    
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}
}

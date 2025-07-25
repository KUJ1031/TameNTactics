using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOnKill : IPassiveSkill
{
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill)
    {
        if (target.CurHp <= 0)
        {
            int amount = Mathf.RoundToInt(attacker.MaxHp * 0.2f);
            attacker.Heal(amount);
        }
    }
    
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}
}

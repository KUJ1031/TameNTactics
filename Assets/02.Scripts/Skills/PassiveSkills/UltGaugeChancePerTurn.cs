using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltGaugeChancePerTurn : IPassiveSkill
{
    public void OnTurnEnd(Monster self)
    {
        if (Random.value < 0.2f)
        {
            self.IncreaseUltimateCost();
        }
    }
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }

    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}

    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill) {}
}

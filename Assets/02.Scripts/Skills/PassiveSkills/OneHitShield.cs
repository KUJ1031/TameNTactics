using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneHitShield : IPassiveSkill
{
    private bool isShielding = false;

    public void OnBattleStart(Monster self, List<Monster> monsters)
    {
        isShielding = true;
    }
    
    public int OnDamaged(Monster self, int damage, Monster actor)
    {
        if (isShielding)
        {
            return 0;
            isShielding = false;
        }
        else return damage;
    }
    
    public void OnTurnEnd(Monster self) {}
    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill) {}
}

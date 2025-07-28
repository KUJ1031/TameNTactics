using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCritBoost : IPassiveSkill
{
    private int curStack = 0;
    private int maxStack = 3;
    private float increaseAmount = 0.05f;
    
    public void OnBattleStart(Monster self, List<Monster> monsters)
    {
        curStack = 0;
    }

    public int OnDamaged(Monster self, int damage, Monster actor)
    {
        if (curStack < maxStack)
        {
            self.BattleCritChanceUp(15);
            curStack++;
        }
        
        return damage;
    }
    
    public void OnTurnEnd(Monster self) {}
    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill) {}
}

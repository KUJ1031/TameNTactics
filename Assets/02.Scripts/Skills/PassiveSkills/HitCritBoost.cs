using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 피격시 치명타확률 10% 상승(최대 3스택), 20레벨 20% 상승
public class HitCritBoost : IPassiveSkill
{
    private int curStack = 0;
    private int maxStack = 3;
    
    public void OnBattleStart(Monster self, List<Monster> monsters)
    {
        curStack = 0;
    }

    public int OnDamaged(Monster self, int damage, Monster actor)
    {
        if (curStack < maxStack)
        {
            int amount = self.Level >= 20 ? 20 : 10;
            
            self.BattleCritChanceUp(amount);
            curStack++;
        }
        
        return damage;
    }
    
    public void OnTurnEnd(Monster self) {}
    public void OnAllyDeath(Monster self) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

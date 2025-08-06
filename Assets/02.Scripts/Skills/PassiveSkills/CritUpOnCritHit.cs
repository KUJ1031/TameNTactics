using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 치명타로 맞을시 치명타 확률 30% 상승, 20레벨 40% 상승
public class CritUpOnCritHit : IPassiveSkill
{
    private int curStack = 0;
    private int maxStack = 3;

    public void OnBattleStart(Monster self, List<Monster> monsters)
    {
        curStack = 0;
    }
    
    public void OnCritHit(Monster self, bool isCritical)
    {
        if (isCritical)
        {
            int amount = self.Level >= 20 ? 40 : 30;
            self.BattleCritChanceUp(amount);
            curStack++;
        }
    }
    
    public void OnTurnEnd(Monster self) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

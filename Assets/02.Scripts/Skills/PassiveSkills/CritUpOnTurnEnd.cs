using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 매 턴 끝날때 치명타확률 5% 상승 (최대 60%), 20레벨 최대 100%
public class CritUpOnTurnEnd : IPassiveSkill
{
    public void OnTurnEnd(Monster self)
    {
        int amount = self.Level >= 20 ? 100 : 60;
        
        if (self.CurCriticalChance < amount)
        {
            self.BattleCritChanceUpWithLimit(5, amount);
        }
    }
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; } 
    public void OnAllyDeath(Monster self) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 피격시 스피드 10% 상승, 20레벨 치명타 확률 10% 상승(최대 3스택)
public class HitSpdBoost : IPassiveSkill
{
    private int curStack = 0;
    private int maxStack = 3;
    
    public void OnBattleStart(Monster self, List<Monster> monsters)
    {
        curStack = 0;
    }

    public int OnDamaged(Monster self, int damage, Monster actor)
    {
        int amount = Mathf.RoundToInt(self.CurSpeed * 0.1f);
        self.SpeedUpEffect(amount);
        
        if (self.Level >= 20 && curStack < maxStack)
        {
            self.BattleCritChanceUp(10);
            curStack++;
        }
        
        return damage;
    }
    
    public void OnTurnEnd(Monster self) {}
    public void OnAllyDeath(Monster self, List<Monster> team) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

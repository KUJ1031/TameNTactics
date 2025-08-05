using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 피격시 공격력 5% 상승(최대 3스택), 15레벨 10% 상승
public class AtkUpOnDamaged : IPassiveSkill
{
    private int curStack = 0;
    private int maxStack = 3;

    public void OnBattleStart(Monster self, List<Monster> monsters)
    {
        curStack = 0;
    }

    public int OnDamaged(Monster self, int damage, Monster actor)
    {
        float increaseAmount = self.Level >= 15 ? 0.1f : 0.05f;
        
        if (curStack < maxStack)
        {
            int amount = Mathf.RoundToInt(self.CurAttack * increaseAmount);
            self.PowerUp(amount);
            curStack++;
        }
        
        return damage;
    }
    public void OnTurnEnd(Monster self) {}
    public void OnAllyDeath(Monster self) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

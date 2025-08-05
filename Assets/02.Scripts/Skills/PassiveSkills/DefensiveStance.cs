using System.Collections.Generic;
using UnityEngine;

// 공격받는 데미지 10% 감소, 15레벨 데미지 20% 감소
public class DefensiveStance : IPassiveSkill
{
    public int OnDamaged(Monster self, int damage, Monster actor)
    {
        int amount = Mathf.RoundToInt(self.Level >= 20 ? damage * 0.15f : damage * 0.1f);
        return damage - amount;
    }
    
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public void OnAllyDeath(Monster self) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

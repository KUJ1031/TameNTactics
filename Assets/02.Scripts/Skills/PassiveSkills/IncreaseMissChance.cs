using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 5% 확률로 공격 회피, 20레벨 20% 반사 데미지
public class IncreaseMissChance : IPassiveSkill
{
    public int OnDamaged(Monster self, int damage, Monster actor)
    {
        if (Random.value < 0.05f)
        {
            if (self.Level >= 20)
            {
                int amount = Mathf.RoundToInt(damage * 0.2f);
                actor.TakeDamage(amount);
            }
            
            return 0;
        }
        
        return damage;
    }
    
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public void OnAllyDeath(Monster self, List<Monster> team) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

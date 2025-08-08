using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 공격을 한번 막아주는 실드(상태이상은 못막음), 20레벨 20% 확률로 공격 받아도 안깨짐
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
            if (self.Level >= 20 && Random.value < 0.2f)
            {
                isShielding = true;
                return 0;
            }
            
            isShielding = false;
            return 0;
        }
        
        return damage;
    }
    
    public void OnTurnEnd(Monster self) {}
    public void OnAllyDeath(Monster self, List<Monster> team) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

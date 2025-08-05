using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 10% 확률로 추가 공격(데미지의 20%), 20레벨 20% 확률 데미지의 30%
public class BonusAttack : IPassiveSkill
{
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness)
    {
        int amount = Mathf.RoundToInt(attacker.Level >= 20 ? 0.3f : 0.2f);
        int bonusDamage = damage * amount;
        float value = attacker.Level >= 20 ? 0.2f : 0.1f;
        
        if (Random.value < value)
        {
            target.TakeDamage(bonusDamage);
        }
    }
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self) {}
}

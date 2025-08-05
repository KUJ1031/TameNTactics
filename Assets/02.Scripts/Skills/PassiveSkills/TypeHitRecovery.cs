using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 유리한 상성 공격시 데미지의 20% 체력 회복 20레벨 30% 회복
public class TypeHitRecovery : IPassiveSkill
{
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness)
    {
        if (effectiveness == 1.5f)
        {
            int amount = Mathf.RoundToInt(attacker.Level >= 20 ? damage * 0.3f : damage * 0.2f);
            attacker.Heal(amount);
        }
    }
    
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public void OnAllyDeath(Monster self) {}
}

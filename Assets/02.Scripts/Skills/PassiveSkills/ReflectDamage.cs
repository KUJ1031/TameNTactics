using System.Collections.Generic;
using UnityEngine;

// 받는 데미지의 10% 되돌려줌, 15레벨 15% 증가
public class ReflectDamage : IPassiveSkill
{
    public int OnDamaged(Monster self, int damage, Monster actor)
    {
        int reflectDamage = Mathf.RoundToInt(self.Level >= 15 ? damage * 0.15f : damage * 0.1f);
        actor.TakeDamage(reflectDamage);
        return damage;
    }

    public void OnBattleStart(Monster self, List<Monster> allies) { }
    public void OnTurnEnd(Monster self) { }
    public void OnAllyDeath(Monster self) { }
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}
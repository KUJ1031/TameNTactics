using System.Collections.Generic;
using UnityEngine;

public class ReflectDamage : IPassiveSkill
{
    // 데미지를 입었을때 받은 데미지의 10% 반사함
    public int OnDamaged(Monster self, int damage, Monster actor)
    {
        int reflectDamage = Mathf.RoundToInt(damage * 0.1f);
        actor.TakeDamage(reflectDamage);
        return damage;
    }

    public void OnBattleStart(Monster self, List<Monster> allies) { }
    public void OnTurnEnd(Monster self) { }
    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) { }
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill) {}
}
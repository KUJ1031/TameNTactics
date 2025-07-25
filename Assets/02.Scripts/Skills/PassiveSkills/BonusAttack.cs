using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusAttack : IPassiveSkill
{
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill)
    {
        int bonusDamage = Mathf.RoundToInt(damage * 0.2f);
        if (Random.value < 0.1f)
        {
            target.TakeDamage(bonusDamage);
        }
    }
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}
}

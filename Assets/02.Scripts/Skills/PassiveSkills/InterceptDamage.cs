using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterceptDamage : IPassiveSkill
{
    public void OnDamagedAlly(Monster self, Monster target, SkillData skill, int damage)
    {
        if (skill.targetScope != TargetScope.All ||
            skill.targetScope != TargetScope.Self ||
            skill.targetCount > 0)
        {
            int check = Mathf.RoundToInt(target.MaxHp * 0.7f);
            if (damage >= check)
            {
                self.TakeDamage(damage);
            }
        }
    }
    
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill) {}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 같은팀중 최대체력의 50% 이상의 데미지를 받을 시 공격을 대신 받음, 20레벨 30% 이상 데미지 받을 시
public class InterceptDamage : IPassiveSkill
{
    public void OnDamagedAlly(Monster self, Monster target, SkillData skill, int damage)
    {
        if (skill.targetScope != TargetScope.All ||
            skill.targetScope != TargetScope.Self ||
            skill.targetCount > 0)
        {
            int amount = Mathf.RoundToInt(self.Level >= 20 ? damage * 0.5f : damage * 0.3f);
            int check = target.CurMaxHp * amount;
            
            if (damage >= check)
            {
                self.TakeDamage(damage);
            }
        }
    }
    
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

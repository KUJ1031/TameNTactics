using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 궁극기 사용시 모든 상태이상 제거, 20레벨 공격력 10% 증가
public class CleanseSelfOnUlt : IPassiveSkill
{
    public void OnUseUlt(Monster self)
    {
        self.RemoveStatusEffects();

        if (self.Level >= 20)
        {
            int amount = Mathf.RoundToInt(self.CurAttack * 0.1f);
            self.PowerUp(amount);
        }
    }
    
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self, List<Monster> team) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

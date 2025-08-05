using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 모든 상태이상 무효, 15레벨 배틀 시작시 방어력 10% 상승
public class StatusEffectImmunity : IPassiveSkill
{
    public bool IsImmuneToStatus { get; private set; } = true;


    public void OnBattleStart(Monster self, List<Monster> monsters)
    {
        if (self.Level >= 15)
        {
            int amount = Mathf.RoundToInt(self.CurDefense * 0.1f);
            self.BattleDefenseUp(amount);
        }
    }

    public void OnTurnEnd(Monster self) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

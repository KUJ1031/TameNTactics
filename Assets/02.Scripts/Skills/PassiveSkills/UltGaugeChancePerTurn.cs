using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 매 턴 끝날때 20% 확률로 궁극기 게이지 1개 증가, 20레벨 40% 증가
public class UltGaugeChancePerTurn : IPassiveSkill
{
    public void OnTurnEnd(Monster self)
    {
        int amount = Mathf.RoundToInt(self.Level >= 20 ? 0.7f : 0.4f);
        
        if (Random.value < amount)
        {
            self.IncreaseUltimateCost();
        }
    }
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }

    public void OnAllyDeath(Monster self, List<Monster> team) {}

    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

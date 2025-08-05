using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 매 턴 끝날때 20% 확률로 궁극기 게이지 1개 증가, 15레벨 40% 증가
public class UltGaugeChancePerTurn : IPassiveSkill
{
    public void OnTurnEnd(Monster self)
    {
        int amount = Mathf.RoundToInt(self.Level >= 15 ? 0.4f : 0.2f);
        
        if (Random.value < amount)
        {
            self.IncreaseUltimateCost();
        }
    }
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }

    public void OnAllyDeath(Monster self) {}

    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

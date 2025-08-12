using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 체력 50% 이하일때 방어력 20% 상승, 20레벨 30% 상승
public class LowHpDefBoost : IPassiveSkill
{
    private bool isApplied = false;
    private int increaseDef;
    
    public void OnTurnEnd(Monster self)
    {
        bool isBelowHalf = self.CurHp <= self.MaxHp / 2;

        if (isBelowHalf && !isApplied)
        {
            float amount = self.Level >= 20 ? 0.3f : 0.2f;
            increaseDef = Mathf.RoundToInt(self.CurDefense * amount);
            self.BattleDefenseUp(increaseDef);
            isApplied = true;
        }
        else if (!isBelowHalf && isApplied)
        {
            self.BattleDefenseDown(increaseDef);
            isApplied = false;
        }
    }

    public void OnBattleStart(Monster self, List<Monster> allies)
    {
        isApplied = false;
    }
    
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self, List<Monster> team) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowHpDefBoost : IPassiveSkill
{
    private bool isApplied = false;
    private int increaseDef;
    
    public void OnTurnEnd(Monster self)
    {
        bool isBelowHalf = self.CurHp <= self.MaxHp / 2;

        if (isBelowHalf && !isApplied)
        {
            int amount = Mathf.RoundToInt(self.Level >= 15 ? 0.3f : 0.2f);
            increaseDef = self.CurDefense * amount;
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
    public void OnAllyDeath(Monster self) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

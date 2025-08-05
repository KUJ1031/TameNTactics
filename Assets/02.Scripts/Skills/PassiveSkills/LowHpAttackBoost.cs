using System.Collections.Generic;
using UnityEngine;

// 체력 50% 이하일때 공격력 20% 상승, 15레벨 30% 상승
public class LowHpAttackBoost : IPassiveSkill
{
    private bool isApplied = false;
    private int powerDelta;
    
    public void OnTurnEnd(Monster self)
    {
        bool isBelowHalf = self.CurHp <= self.MaxHp / 2;

        if (isBelowHalf && !isApplied)
        {
            int amount = Mathf.RoundToInt(self.Level >= 15 ? 0.3f : 0.2f);
            powerDelta = self.CurAttack * amount;
            self.PowerUp(powerDelta);
            isApplied = true;
        }
        else if (!isBelowHalf && isApplied)
        {
            self.PowerDown(powerDelta);
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

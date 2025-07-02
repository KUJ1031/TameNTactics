using System.Collections.Generic;
using UnityEngine;

public class LowHpAttackBoost : IPassiveSkill
{
    private bool isApplied = false;
    private int powerDelta;

    public void OnTurnEnd(Monster self)
    {
        bool isBelowHalf = self.CurHp <= self.MaxHp / 2;

        if (isBelowHalf && !isApplied)
        {
            powerDelta = Mathf.RoundToInt(self.Attack * 0.2f);
            self.PowerUp(powerDelta);
            isApplied = true;
        }
        else if (!isBelowHalf && isApplied)
        {
            self.PowerDown(powerDelta);
            isApplied = false;
        }
    }

    public void OnBattleStart(Monster self, List<Monster> allies) { }
    public void OnDamaged(Monster self, int damage, int reflectDamage) { }
    public bool TryEscape(Monster self, bool isGuaranteedEscape) => false;
}

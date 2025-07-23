using System.Collections.Generic;
using UnityEngine;

public class LowHpAttackBoost : IPassiveSkill
{
    private bool isApplied = false;
    private int powerDelta;

    // 턴이 끝날때, 본인 현재 체력이 50% 이하일때 공격력 20% 상승
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
    public void OnDamaged(Monster self, int damage, Monster actor) { }
    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}
}

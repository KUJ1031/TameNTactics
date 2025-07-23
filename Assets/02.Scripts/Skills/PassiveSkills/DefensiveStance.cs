using System.Collections.Generic;
using UnityEngine;

public class DefensiveStance : IPassiveSkill
{
    // 공격받는 데미지 10% 감소
    public void OnDamaged(Monster self, int damage, Monster actor)
    {
        int decreaseAmount = Mathf.RoundToInt(damage * 0.1f);
        self.TakeDamage(decreaseAmount);
    }
    
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public bool TryEscape(Monster self, ref bool isGuaranteedEscape) => false;
}

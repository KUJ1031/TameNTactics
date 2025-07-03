using System.Collections.Generic;
using UnityEngine;

public class SelfHealOnTurnEnd : IPassiveSkill
{
    public void OnTurnEnd(Monster self)
    {
        int amount = Mathf.RoundToInt(self.MaxHp * 0.05f);
        self.Heal(amount);
    }

    public void OnBattleStart(Monster self, List<Monster> allies) { }
    public void OnDamaged(Monster self, int damage, Monster actor) { }
    public bool TryEscape(Monster self, ref bool isGuaranteedEscape) => false;
}

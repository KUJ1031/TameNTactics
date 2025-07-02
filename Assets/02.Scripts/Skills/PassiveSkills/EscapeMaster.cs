using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMaster : IPassiveSkill
{
    public bool TryEscape(Monster self, ref bool isGuaranteedEscape)
    {
        isGuaranteedEscape = true;
        return true;
    }

    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnDamaged(Monster self, int damage, Monster actor) {}
    public void OnTurnEnd(Monster self) {}
}

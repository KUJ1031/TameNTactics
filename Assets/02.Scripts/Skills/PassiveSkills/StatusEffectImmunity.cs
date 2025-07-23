using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectImmunity : IPassiveSkill
{
    public bool IsImmuneToStatus => true;
    public void OnBattleStart(Monster self, List<Monster> monsters) {}

    public void OnTurnEnd(Monster self) {}

    public void OnDamaged(Monster self, int damage, Monster actor) {}

    public bool TryEscape(Monster self, ref bool isGuaranteedEscape) => false;
}

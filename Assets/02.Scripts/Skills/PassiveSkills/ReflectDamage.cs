using System.Collections.Generic;
using UnityEngine;

public class ReflectDamage : IPassiveSkill
{
    public void OnDamaged(Monster self, int damage, Monster actor)
    {
        if (actor != null)
        {
            int reflectDamage = Mathf.RoundToInt(damage * 0.2f);
            actor.TakeDamage(reflectDamage);
        }
    }

    public void OnBattleStart(Monster self, List<Monster> allies) { }
    public void OnTurnEnd(Monster self) { }
    public bool TryEscape(Monster self, ref bool isGuaranteedEscape) => false;
}

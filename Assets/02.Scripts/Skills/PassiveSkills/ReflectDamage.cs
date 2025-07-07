using System.Collections.Generic;
using UnityEngine;

public class ReflectDamage : IPassiveSkill
{
    // 데미지를 입었을때 받은 데미지의 10% 반사함
    public void OnDamaged(Monster self, int damage, Monster actor)
    {
        if (actor != null)
        {
            int reflectDamage = Mathf.RoundToInt(damage * 0.1f);
            actor.TakeDamage(reflectDamage);
        }
    }

    public void OnBattleStart(Monster self, List<Monster> allies) { }
    public void OnTurnEnd(Monster self) { }
    public bool TryEscape(Monster self, ref bool isGuaranteedEscape) => false;
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IncreaseMissChance : IPassiveSkill
{
    // 5% 확률로 공격 회피
    public void OnDamaged(Monster self, int damage, Monster actor)
    {
        if (Random.value < 0.05f)
        {
            self.TakeDamage(0);
        }
        else self.TakeDamage(damage);
    }
    
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public bool TryEscape(Monster self, ref bool isGuaranteedEscape) => false;
}

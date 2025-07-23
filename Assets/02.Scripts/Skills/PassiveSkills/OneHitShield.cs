using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneHitShield : IPassiveSkill
{
    private bool isShielding = false;

    public void OnBattleStart(Monster self, List<Monster> monsters)
    {
        isShielding = true;
    }
    
    public void OnDamaged(Monster self, int damage, Monster actor)
    {
        if (isShielding)
        {
            self.TakeDamage(0);
            isShielding = false;
        }
        else self.TakeDamage(damage);
    }
    
    public void OnTurnEnd(Monster self) {}
    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}
}

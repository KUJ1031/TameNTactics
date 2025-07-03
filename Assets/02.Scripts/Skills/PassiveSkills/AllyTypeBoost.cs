using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllyTypeBoost : IPassiveSkill
{
    private bool applied = false;

    public void OnBattleStart(Monster self, List<Monster> allies)
    {
        if (applied) return;

        int sameTypeCount = allies.Count(m => m.type == self.type);
        
        if (sameTypeCount > 1)
        {
            int amount = Mathf.RoundToInt(self.Attack * 0.1f);
            
            foreach (var monster in allies)
            {
                monster.PowerUp(amount);
            }
            
            applied = true;
        }
    }

    public void OnTurnEnd(Monster self) { }
    public void OnDamaged(Monster self, int damage, Monster actor) { }
    public bool TryEscape(Monster self, ref bool isGuaranteedEscape) => false;
}

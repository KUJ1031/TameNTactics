using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanseSelfOnUlt : IPassiveSkill
{
    public void OnUseUlt(Monster self)
    {
        self.RemoveStatusEffects();
    }
    
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill) {}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMaster : IPassiveSkill
{
    // 도망시 100% 도망 가능
    public bool TryEscape(Monster self, ref bool isGuaranteedEscape)
    {
        isGuaranteedEscape = true;
        return true;
    }

    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnTurnEnd(Monster self) {}
    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill) {}
}

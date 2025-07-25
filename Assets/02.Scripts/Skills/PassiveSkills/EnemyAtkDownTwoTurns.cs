using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtkDownTwoTurns : IPassiveSkill
{
    public void OnBattleStart(Monster self, List<Monster> targets)
    {
        foreach (var target in targets)
        {
            target.ApplyStatus(new AttackDown(2));
        }
    }

    public void OnTurnEnd(Monster self) {}

    public int OnDamaged(Monster self, int damage, Monster actor) { return damage;}

    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}

    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill) {}
}

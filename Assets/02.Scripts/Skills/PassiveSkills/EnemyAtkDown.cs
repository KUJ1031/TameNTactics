using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtkDown : IPassiveSkill
{
    public void OnBattleStart(Monster self, List<Monster> targets)
    {
        foreach (var target in targets)
        {
            int amount = Mathf.RoundToInt(target.CurAttack * 0.05f);
            target.PowerDown(amount);
        }
    }

    public void OnTurnEnd(Monster self) {}

    public int OnDamaged(Monster self, int damage, Monster actor) { return damage;}

    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}

    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill) {}
}

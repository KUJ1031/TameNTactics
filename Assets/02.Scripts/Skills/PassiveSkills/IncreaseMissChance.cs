using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IncreaseMissChance : IPassiveSkill
{
    // 5% 확률로 공격 회피
    public int OnDamaged(Monster self, int damage, Monster actor)
    {
        if (Random.value < 0.05f)
        {
            return 0;
        }
        else return damage;
    }
    
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill) {}
}

using System.Collections.Generic;
using UnityEngine;

public class DefensiveStance : IPassiveSkill
{
    // 공격받는 데미지 10% 감소
    public int OnDamaged(Monster self, int damage, Monster actor)
    {
        int decreaseAmount = Mathf.RoundToInt(damage * 0.1f);
        return damage - decreaseAmount;
    }
    
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill) {}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkUpOnAllyDeath : IPassiveSkill
{
    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam)
    {
        float increaseRate = deadAllyTeam.Count * 0.2f;
        
        int increaseAmount = Mathf.RoundToInt(self.Attack * increaseRate);
        self.PowerUp(increaseAmount);
    }
    
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill) {}
}

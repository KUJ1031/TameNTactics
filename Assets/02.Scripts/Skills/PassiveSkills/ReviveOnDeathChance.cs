using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveOnDeathChance : IPassiveSkill
{
    public void OnDeath(Monster self)
    {
        if (Random.value < 0.5)
        {
            int amount = Mathf.RoundToInt(self.MaxHp * 0.3f);
            self.Heal(amount);
        }
    }
    
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill) {}
}

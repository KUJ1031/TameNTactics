using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 쓰러젔을때 50% 확률로 최대체력의 30%로 부활, 20레벨 50%로 부활
public class ReviveOnDeathChance : IPassiveSkill
{
    public void OnAllyDeath(Monster self, List<Monster> team)
    {
        if (Random.value < 0.5)
        {
            int amount = Mathf.RoundToInt(self.Level >= 20 ? self.CurMaxHp * 0.3f : self.CurMaxHp * 0.5f);
            self.Heal(amount);
        }
    }
    
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

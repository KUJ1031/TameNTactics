using System.Collections.Generic;
using UnityEngine;

public class SelfHealOnTurnEnd : IPassiveSkill
{
    // 턴 끝날때 최대체력의 5% 회복, 15레벨 10% 회복
    public void OnTurnEnd(Monster self)
    {
        int amount = Mathf.RoundToInt(self.Level >= 15 ? self.MaxHp * 0.1f : self.MaxHp * 0.05f);
        self.Heal(amount);
    }

    public void OnBattleStart(Monster self, List<Monster> allies) { }
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

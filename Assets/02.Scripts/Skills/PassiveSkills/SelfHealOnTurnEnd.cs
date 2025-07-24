using System.Collections.Generic;
using UnityEngine;

public class SelfHealOnTurnEnd : IPassiveSkill
{
    // 턴이 끝날때, 본인 최대 체력의 5% 수치만큼 회복됨
    public void OnTurnEnd(Monster self)
    {
        int amount = Mathf.RoundToInt(self.MaxHp * 0.05f);
        self.Heal(amount);
    }

    public void OnBattleStart(Monster self, List<Monster> allies) { }
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill) {}
}

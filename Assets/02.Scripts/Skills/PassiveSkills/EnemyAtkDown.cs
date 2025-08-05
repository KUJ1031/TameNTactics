using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임 시작시 상대 공격력 5% 감소 20레벨 상대 방어력 5% 감소
public class EnemyAtkDown : IPassiveSkill
{
    public void OnBattleStart(Monster self, List<Monster> targets)
    {
        foreach (var target in targets)
        {
            int atkAmount = Mathf.RoundToInt(target.CurAttack * 0.05f);
            target.PowerDown(atkAmount);

            if (self.Level >= 20)
            {
                int defAmount = Mathf.RoundToInt(target.CurDefense * 0.05f);
                target.DefenseDown(defAmount);
            }
        }
    }

    public void OnTurnEnd(Monster self) {}

    public int OnDamaged(Monster self, int damage, Monster actor) { return damage;}

    public void OnAllyDeath(Monster self) {}

    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

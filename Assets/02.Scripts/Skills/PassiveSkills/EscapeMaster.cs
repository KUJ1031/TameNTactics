using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 도망시 100% 도망 가능, 20레벨 게임 시작시 팀 전체 스피드 10% 증가
public class EscapeMaster : IPassiveSkill
{
    public bool TryEscape(Monster self, ref bool isGuaranteedEscape)
    {
        isGuaranteedEscape = true;
        return true;
    }

    public void OnBattleStart(Monster self, List<Monster> monsters)
    {
        if (self.Level >= 20)
        {
            foreach (var monster in monsters)
            {
                int amount = Mathf.RoundToInt(monster.CurSpeed * 0.1f);
                monster.SpeedUpEffect(amount);
            }
        }
    }
    
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnTurnEnd(Monster self) {}
    public void OnAllyDeath(Monster self) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

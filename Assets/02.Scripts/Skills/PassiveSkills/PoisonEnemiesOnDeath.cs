using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 쓰러졌을때 상대 몬스터 전체에게 2턴 중독 부여, 20레벨 4턴 부여
public class PoisonEnemiesOnDeath : IPassiveSkill
{
    public void OnDeath(List<Monster> enemyTeam)
    {
        if (enemyTeam.Count == 0) return;
        
        foreach (var monster in enemyTeam)
        {
            int amount = Mathf.RoundToInt(monster.Level >= 20 ? 4 : 2);
            monster.ApplyStatus(new Poison(amount));
        }
    }
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness){}
}

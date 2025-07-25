using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBoostPerUlt : IPassiveSkill
{
    public void OnUseUlt(Monster self, Monster ultimateUser, List<Monster> team)
    {
        // 사용자가 같은 팀인지 체크
        if (team.Contains(ultimateUser))
        {
            int boostAmount = Mathf.RoundToInt(self.Attack * 0.1f);
            self.PowerUp(boostAmount);
        }
    }

    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill) {}
}

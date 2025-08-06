using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 같은팀(본인 포함) 궁극기를 사용하면 공격력 10% 증가, 20레벨 스피드 10% 증가
public class PowerBoostPerUlt : IPassiveSkill
{
    public void OnUseUlt(Monster self, Monster ultimateUser, List<Monster> team)
    {
        if (team.Contains(ultimateUser))
        {
            int boostAmount = Mathf.RoundToInt(self.Attack * 0.1f);
            self.PowerUp(boostAmount);
            
            if (self.Level >= 20)
            {
                int amount = Mathf.RoundToInt(self.CurSpeed * 0.1f);
                self.SpeedUpEffect(amount);
            }
        }
    }

    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self, List<Monster> team) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

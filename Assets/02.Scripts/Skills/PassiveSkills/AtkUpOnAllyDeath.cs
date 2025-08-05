using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkUpOnAllyDeath : IPassiveSkill
{
    // 아군 쓰러질때마다 공격력 20%씩 상승, 20레벨 스피드 20%씩 상승
    public void OnAllyDeath(Monster self)
    {
        int increaseAmount = Mathf.RoundToInt(self.Attack * 0.2f);
        self.PowerUp(increaseAmount);

        if (self.Level >= 20)
        {
            int amount = Mathf.RoundToInt(self.CurSpeed * 0.2f);
            self.SpeedUpEffect(amount);
        }
    }
    
    public void OnBattleStart(Monster self, List<Monster> monsters) {}
    public void OnTurnEnd(Monster self) {}
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 본인이 살아있는 동안 팀 전체 방어력 10% 상승, 20레벨 20% 상승
public class AliveTeamGuard : IPassiveSkill
{
    private bool isActive = false;
    private int lastBuffAmount = 0;

    public void OnTurnEnd(Monster self)
    {
        List<Monster> team = BattleManager.Instance.BattleEntryTeam.Contains(self)
            ? BattleManager.Instance.BattleEntryTeam
            : BattleManager.Instance.BattleEnemyTeam;

        if (self.CurHp > 0)
        {
            if (!isActive)
            {
                ApplyDefenseBuff(team, self);
                isActive = true;
            }
        }
        else
        {
            if (isActive)
            {
                RemoveDefenseBuff(team);
                isActive = false;
            }
        }
    }

    private void ApplyDefenseBuff(List<Monster> team, Monster self)
    {
        float value = self.Level >= 20 ? 0.2f : 0.1f;
        int amount = Mathf.RoundToInt(self.CurDefense * value);
        lastBuffAmount = amount;

        foreach (var monster in team)
        {
            monster.BattleDefenseUp(amount);
        }
    }

    private void RemoveDefenseBuff(List<Monster> team)
    {
        foreach (var monster in team)
        {
            monster.BattleDefenseDown(lastBuffAmount);
        }

        lastBuffAmount = 0;
    }

    public void OnBattleStart(Monster self, List<Monster> monsters)
    {
        isActive = false;
        lastBuffAmount = 0;
    }

    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}
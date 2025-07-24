using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllyTypeBoost : IPassiveSkill
{
    private bool applied = false;

    // 턴이 시작될때,
    // 우리팀중 본인과 같은 타입이 2명 이상일 시 발동, 우리팀중 본인과 같은 타입은 모두 공격력 10% 증가
    public void OnBattleStart(Monster self, List<Monster> allies)
    {
        if (applied) return;

        int sameTypeCount = allies.Count(m => m.type == self.type);
        
        if (sameTypeCount > 1)
        {
            int amount = Mathf.RoundToInt(self.Attack * 0.1f);
            
            foreach (var monster in allies)
            {
                if (monster.type == self.type)
                {
                    monster.PowerUp(amount);
                }
            }
            
            applied = true;
        }
    }

    public void OnTurnEnd(Monster self) { }
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self, List<Monster> deadAllyTeam) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill) {}
}

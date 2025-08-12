using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AllyTypeBoost : IPassiveSkill
{
    // 우리팀중 본인과 같은 타입이 2명 이상일 시 발동, 우리팀중 본인과 같은 타입은 모두 공격력 10% 증가, 20레벨 20% 증가
    public void OnBattleStart(Monster self, List<Monster> allies)
    {
        int sameTypeCount = allies.Count(m => m.type == self.type);
        
        if (sameTypeCount > 1)
        {
            float value = self.Level >= 20 ? 0.2f : 0.1f;
            int amount = Mathf.RoundToInt(self.CurAttack * value);
            
            foreach (var monster in allies)
            {
                if (monster.type == self.type)
                {
                    monster.PowerUp(amount);
                }
            }
        }
    }

    public void OnTurnEnd(Monster self) { }
    public int OnDamaged(Monster self, int damage, Monster actor) { return damage; }
    public void OnAllyDeath(Monster self, List<Monster> team) {}
    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

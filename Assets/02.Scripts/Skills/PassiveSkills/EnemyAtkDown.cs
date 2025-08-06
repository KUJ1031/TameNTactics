using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임 시작시 상대 공격력 5% 감소 20레벨 상대 방어력 5% 감소
public class EnemyAtkDown : IPassiveSkill
{
    public void OnBattleStart(Monster self, List<Monster> targets)
    {
        List<Monster> enemies = BattleManager.Instance.BattleEntryTeam == targets
            ? BattleManager.Instance.BattleEnemyTeam
            : BattleManager.Instance.BattleEntryTeam;
        
        foreach (var target in enemies)
        {
            int atkAmount = Mathf.RoundToInt(target.CurAttack * 0.05f);
            Debug.Log($"{target.monsterName}이놈의 공격력 원래{target.CurAttack}이건데");
            target.PowerDown(atkAmount);
            Debug.Log($"{target.monsterName}이놈의 공격력이{target.CurAttack}이렇게 바뀜");

            if (self.Level >= 20)
            {
                int defAmount = Mathf.RoundToInt(target.CurDefense * 0.05f);
                Debug.Log($"{target.monsterName}이놈의 방어력 원래{target.CurDefense}이건데");
                target.BattleDefenseDown(defAmount);
                Debug.Log($"{target.monsterName}이놈의 방어력이{target.CurDefense}이렇게 바뀜");
            }
        }
    }

    public void OnTurnEnd(Monster self) {}

    public int OnDamaged(Monster self, int damage, Monster actor) { return damage;}

    public void OnAllyDeath(Monster self) {}

    public void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness) {}
}

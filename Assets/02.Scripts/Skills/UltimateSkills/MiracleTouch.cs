using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiracleTouch : ISkillEffect
{
    private SkillData skillData;

    public MiracleTouch(SkillData data)
    {
        skillData = data;
    }
    
    // 같은팀 한명 100% 회복, 모든 상태이상 제거, 25레벨 전체 스텟 10% 상승
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;
        
        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            if (target.CurHp > 0)
            {
                BattleManager.Instance.ReviveMonsters(caster, target, target.CurMaxHp);
                target.RemoveStatusEffects();
                if (caster.Level >= 25)
                {
                    int atkUp = Mathf.RoundToInt(target.CurAttack * 0.1f);
                    int defUp = Mathf.RoundToInt(target.CurDefense * 0.1f);
                    int spdUp = Mathf.RoundToInt(target.CurSpeed * 0.1f);
                    int criUp = 10;

                    target.PowerUp(atkUp);
                    target.BattleDefenseUp(defUp);
                    target.SpeedUpEffect(spdUp);
                    target.BattleCritChanceUp(criUp);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyTeamDoubleAttack : ISkillEffect
{
    private SkillData skillData;

    public BossEnemyTeamDoubleAttack(SkillData data)
    {
        skillData = data;
    }
    
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            BattleManager.Instance.StartCoroutine(BattleManager.Instance.BossAttack(target, caster, skillData));
        }
    }
}

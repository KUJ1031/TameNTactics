using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillEffectController
{
    public static void PlayEffect(SkillData skill, MonsterCharacter caster, List<MonsterCharacter> targets)
    {
        if (skill.skillEffectPrefab == null || targets == null || targets.Count == 0)
            return;

        foreach (var target in targets)
        {
            Vector3 pos = GetEffectPosition(skill.targetScope, caster, target);
            GameObject effect = Object.Instantiate(skill.skillEffectPrefab, pos, Quaternion.identity);
            Object.Destroy(effect, 1f);
        }
    }

    private static Vector3 GetEffectPosition(TargetScope scope, MonsterCharacter caster, MonsterCharacter target)
    {
        if (scope == TargetScope.Self) return caster.transform.position + Vector3.up * 1f;
        else return target.transform.position + Vector3.up * 1f;
    }
}
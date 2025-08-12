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

    public static GameObject PlayEffect(SkillData skill, Vector3 position)
    {
        if (skill.skillEffectPrefab == null)
            return null;

        GameObject effect = Object.Instantiate(skill.skillEffectPrefab, position, Quaternion.identity);
        Object.Destroy(effect, 1f);

        // SpriteRenderer가 있다면 정렬 레이어 수정
        var sr = effect.GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingLayerName = "Effect";
            sr.sortingOrder = 100;
        }

        // ParticleSystem 렌더러도 처리
        var ps = effect.GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            var psRenderer = ps.GetComponent<ParticleSystemRenderer>();
            psRenderer.sortingLayerName = "Effect";
            psRenderer.sortingOrder = 100;
        }

        return effect;
    }


    private static Vector3 GetEffectPosition(TargetScope scope, MonsterCharacter caster, MonsterCharacter target)
    {
        if (scope == TargetScope.Self) return caster.transform.position + Vector3.up * 0.5f;
        else return target.transform.position + Vector3.up * 0.5f;
    }
}
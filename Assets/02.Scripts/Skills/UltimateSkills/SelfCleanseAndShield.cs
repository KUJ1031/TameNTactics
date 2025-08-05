using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfCleanseAndShield : ISkillEffect
{
    private SkillData skillData;
    
    public SelfCleanseAndShield(SkillData data)
    {
        skillData = data;
    }
    
    // 자기자신 모든 상태이상 제거, 실드생성(데미지 받아야 사라짐, 1회 방어), 25레벨 도발 2턴 부여
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;
        
        caster.Shield();
        caster.RemoveStatusEffects();

        if (caster.Level >= 25) caster.ApplyBuff(new Taunt(2));
    }
}

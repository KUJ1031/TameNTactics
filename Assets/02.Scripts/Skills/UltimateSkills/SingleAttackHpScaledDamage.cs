using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAttackHpScaledDamage : ISkillEffect
{
    private SkillData skillData;

    public SingleAttackHpScaledDamage(SkillData data)
    {
        skillData = data;
    }
    
    // 단일 공격 타겟 체력에따라 공격력이 달라짐, 25레벨 데미지 1.5배 증가 공격력 달라지는 데미지 배수 올라감
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            int damage = caster.Level >= 25 ? (Mathf.RoundToInt(result.damage * 1.5f)) : result.damage;
            int finalDamage = Mathf.RoundToInt(damage * ChanceToDamage(caster, target));
            
            BattleManager.Instance.DealDamage(target, finalDamage, caster, this.skillData, result.isCritical, result.effectiveness);
        }
    }

    private float ChanceToDamage(Monster caster, Monster target)
    {
        if (caster.Level >= 25)
        {
            if (target.CurHp >= target.CurMaxHp)
            {
                return 2f;
            }
            else if (target.CurHp >= target.CurMaxHp * 0.7f)
            {
                return 1.5f;
            }
            else if (target.CurHp >= target.CurMaxHp * 0.3f)
            {
                return 1f;
            }
            else
            {
                return 0.5f;
            }
        }
        else
        {
            if (target.CurHp >= target.CurMaxHp)
            {
                return 1.5f;
            }
            else if (target.CurHp >= target.CurMaxHp * 0.7f)
            {
                return 1f;
            }
            else if (target.CurHp >= target.CurMaxHp * 0.3f)
            {
                return 0.7f;
            }
            else
            {
                return 0.3f;
            }
        }
    }
}

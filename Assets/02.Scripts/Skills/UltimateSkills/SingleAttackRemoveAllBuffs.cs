using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAttackRemoveAllBuffs : ISkillEffect
{
    private SkillData skillData;

    public SingleAttackRemoveAllBuffs(SkillData data)
    {
        skillData = data;
    }
    
    // 단일공격 상대 스텟버프 초기화, 15레벨 데미지 1.5배 30% 확률로 아무 상태이상 적용
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            int damage = caster.Level >= 15 ? Mathf.RoundToInt(result.damage * 1.5f) : result.damage;
            
            BattleManager.Instance.DealDamage(target, damage, caster, this.skillData, result.isCritical);
            target.InitializeBattleStats();
            target.RemoveBuffEffects();

            if (caster.Level >= 15)
            {
                if (Random.value < 0.3f)
                {
                    switch (Random.Range(0, 6))
                    {
                        case 0:
                            target.ApplyStatus(new Sleep(2));
                            break;
                        case 1:
                            target.ApplyStatus(new Stun(2));
                            break;
                        case 2:
                            target.ApplyStatus(new Burn(2));
                            break;
                        case 3:
                            target.ApplyStatus(new Poison(2));
                            break;
                        case 4:
                            target.ApplyStatus(new Paralysis(2));
                            break;
                        case 5:
                            target.ApplyStatus(new HealBlock(2));
                            break;
                    }
                }
            }
        }
    }
}

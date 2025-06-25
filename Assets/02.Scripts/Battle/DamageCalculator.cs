using UnityEngine;

public static class DamageCalculator
{
    public class DamageResult
    {
        public float damage;
        public bool isCritical;
        public float effectiveness;
    }
    
    // 데미지 계산!(공격 관련이라 힐도 이거 적용된다는거.. 추후에 디벨롭 하겠슴돠)
    public static DamageResult CalculateDamage(MonsterData attacker, MonsterData target, SkillData skill)
    {
        float baseDamage = attacker.attack * skill.skillPower;
        float defenseFactor = 100f / (target.defense + 100f);
        bool isCrit = Random.value < attacker.criticalChance / 100f;
        float crit = isCrit ? 1.5f : 1f;
        float effectiveness = TypeChart.GetEffectiveness(attacker, target);

        float finalDamage = baseDamage * defenseFactor * crit * effectiveness;

        return new DamageResult
        {
            damage = finalDamage,
            isCritical = isCrit,
            effectiveness = effectiveness
        };
    }
}

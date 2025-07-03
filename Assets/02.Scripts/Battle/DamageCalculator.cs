using UnityEngine;

public static class DamageCalculator
{
    public class DamageResult
    {
        public int damage;
        public bool isCritical;
        public float effectiveness;
    }

    // 데미지 계산 (힐은 나중에 따로 구현 가능)
    public static DamageResult CalculateDamage(Monster attacker, Monster target, SkillData skill)
    {
        // 공격력, 방어력, 치명타 확률 등은 Monster 인스턴스 내 변수 사용
        float baseDamage = attacker.CurAttack * skill.skillPower;
        float defenseFactor = 100f / (target.Defense + 100f);
        bool isCrit = Random.value < attacker.CriticalChance / 100f;
        float critMultiplier = isCrit ? 1.5f : 1f;

        // 타입 상성 계산은 MonsterData를 넘겨서 처리
        float effectiveness = TypeChart.GetEffectiveness(attacker, target);

        float finalDamage = baseDamage * defenseFactor * critMultiplier * effectiveness;

        return new DamageResult
        {
            damage = Mathf.RoundToInt(finalDamage),
            isCritical = isCrit,
            effectiveness = effectiveness
        };
    }
}

using UnityEngine;

public static class DamageCalculator
{
    public static float CalculateDamage(MonsterData attacker, MonsterData target)
    {
        float baseDamage = attacker.attack * (100 / (target.defense + 100));
        float crit = Random.value < attacker.criticalChance / 100 ? 1.5f : 1f;
        float effectiveness = TypeChart.GetEffectiveness(attacker, target);
        
        return baseDamage * crit * effectiveness;
    }
}

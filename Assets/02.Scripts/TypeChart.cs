using UnityEngine;

public static class TypeChart
{
    // 상성표입니다!
    public static float GetEffectiveness(MonsterData attackerType, MonsterData targetType)
    {
        if (attackerType.type == MonsterType.Fire)
        {
            if (targetType.type == MonsterType.Water) return 0.5f;
            if (targetType.type == MonsterType.Grass) return 1.5f;
            if (targetType.type == MonsterType.Steel) return 1.5f;
        }
        
        else if (attackerType.type == MonsterType.Water)
        {
            if (targetType.type == MonsterType.Ground) return 0.5f;
            if (targetType.type == MonsterType.Fire) return 1.5f;
            if (targetType.type == MonsterType.Steel) return 1.5f;
        }
        
        else if (attackerType.type == MonsterType.Grass)
        {
            if (targetType.type == MonsterType.Steel) return 0.5f;
            if (targetType.type == MonsterType.Water) return 1.5f;
            if (targetType.type == MonsterType.Ground) return 1.5f;
        }
        
        else if (attackerType.type == MonsterType.Ground)
        {
            if (targetType.type == MonsterType.Grass) return 0.5f;
            if (targetType.type == MonsterType.Fire) return 1.5f;
            if (targetType.type == MonsterType.Water) return 1.5f;
        }

        else if (attackerType.type == MonsterType.Steel)
        {
            if (targetType.type == MonsterType.Fire) return 0.5f;
            if (targetType.type == MonsterType.Grass) return 1.5f;
            if (targetType.type == MonsterType.Ground) return 1.5f;
        }

        return 1f;
    }
}

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 숲에서 몬스터 팀을 생성하는 클래스 (상태 보존형)
/// </summary>
public class UnknownForestEnemyTeamGenerator
{
    private List<MonsterData> monsterPool;
    private int minLevel;
    private int maxLevel;

    public UnknownForestEnemyTeamGenerator(List<MonsterData> monsterPool, int minLevel, int maxLevel)
    {
        this.monsterPool = monsterPool;
        this.minLevel = minLevel;
        this.maxLevel = maxLevel;
    }

    /// <summary>
    /// 몬스터 팀 생성
    /// </summary>
    /// <param name="count">생성할 수</param>
    public List<Monster> GenerateRandomTeam(int count)
    {
        List<Monster> team = new();

        if (monsterPool == null || monsterPool.Count == 0)
        {
            Debug.LogWarning("[UnknownForestEnemyTeamGenerator] monsterPool이 비어 있음.");
            return team;
        }

        for (int i = 0; i < count; i++)
        {
            MonsterData data = monsterPool[Random.Range(0, monsterPool.Count)];
            Monster m = new();
            m.SetMonsterData(data);
            m.SetLevel(Random.Range(minLevel, maxLevel + 1));
            team.Add(m);
        }

        return team;
    }

    public List<Monster> GenerateFixedTeam()
    {
        List<Monster> team = new();

        if (monsterPool == null || monsterPool.Count == 0)
        {
            Debug.LogWarning("[UnknownForestEnemyTeamGenerator] monsterPool이 비어 있음.");
            return team;
        }

        foreach (MonsterData data in monsterPool)
        {
            Monster m = new();
            m.SetMonsterData(data);
            m.SetLevel(Random.Range(minLevel, maxLevel + 1)); // 레벨은 기존 랜덤 유지
            team.Add(m);
        }

        return team;
    }
}

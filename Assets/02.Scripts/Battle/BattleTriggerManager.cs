using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전투 관련 정보를 저장하고 공유하는 매니저 (전투 씬 전환 전후에 사용)
/// </summary>
public class BattleTriggerManager : Singleton<BattleTriggerManager>
{
    // 직전 충돌한 적 몬스터 (전투 유발용)
    private Monster lastMonster;

    // 직렬화된 적 팀과 플레이어 팀
    private List<SerializableMonsterInfo> serializedEnemyTeam;
    private List<SerializableMonsterInfo> serializedPlayerTeam;
    private List<SerializableMonsterInfo> serializedBenchMonsters;

    // MonsterData → Prefab 매핑 정보 (전투 씬에서 몬스터 생성용)
    private Dictionary<MonsterData, GameObject> prefabMap = new();

    // ====== 몬스터 저장 및 조회 ======

    public void SetLastMonster(Monster monster)
    {
        lastMonster = monster;
    }

    public Monster GetLastMonster()
    {
        return lastMonster;
    }

    public void SetEnemyTeam(List<Monster> team)
    {
        serializedEnemyTeam = new List<SerializableMonsterInfo>();
        foreach (var monster in team)
        {
            if (monster != null)
                serializedEnemyTeam.Add(new SerializableMonsterInfo(monster));
        }
    }

    public List<SerializableMonsterInfo> GetSerializedEnemyTeam()
    {
        Debug.Log($"적 팀 정보: {serializedEnemyTeam?.Count ?? 0}마리");

        foreach (var m in serializedEnemyTeam)
        {
            if (m == null || m.monsterData == null)
            {
                Debug.LogWarning("적 팀에 null 또는 monsterData 누락된 항목 있음");
                continue;
            }
            Debug.Log($"적 몬스터: {m.monsterData.monsterName}, 레벨: {m.level}");
        }

        return serializedEnemyTeam;
    }

    public void SetPlayerTeam(List<Monster> team)
    {
        serializedPlayerTeam = new List<SerializableMonsterInfo>();
        foreach (var monster in team)
        {
            if (monster != null)
                serializedPlayerTeam.Add(new SerializableMonsterInfo(monster));
        }
    }

    public List<SerializableMonsterInfo> GetSerializedPlayerTeam()
    {
        Debug.Log($"플레이어 팀 정보: {serializedPlayerTeam?.Count ?? 0}마리");

        foreach (var m in serializedPlayerTeam)
        {
            if (m == null || m.monsterData == null)
            {
                Debug.LogWarning("플레이어 팀에 null 또는 monsterData 누락된 항목 있음");
                continue;
            }
            Debug.Log($"플레이어 몬스터: {m.monsterData.monsterName}, 레벨: {m.level}");
        }

        return serializedPlayerTeam;
    }

    public void SetBenchMonsters(List<Monster> team)
    {
        serializedBenchMonsters = new List<SerializableMonsterInfo>();
        foreach (var monster in team)
        {
            if (monster != null)
                serializedBenchMonsters.Add(new SerializableMonsterInfo(monster));
        }
    }

    public List<SerializableMonsterInfo> GetSerializedBenchMonsters() => serializedBenchMonsters;

    // ====== 프리팹 매핑 ======

    public void SetMonsterPrefabMap(Dictionary<MonsterData, GameObject> map)
    {
        prefabMap = map;
    }

    public GameObject GetPrefabByData(MonsterData data)
    {
        if (data == null)
        {
            Debug.LogWarning("[BattleTriggerManager] 요청된 MonsterData가 null입니다.");
            return null;
        }

        if (prefabMap.TryGetValue(data, out var prefab))
            return prefab;
        return null;
    }

    // ====== 초기화 ======

    public void ResetBattleData()
    {
        lastMonster = null;
        serializedEnemyTeam = null;
        serializedPlayerTeam = null;
        serializedBenchMonsters = null;
        prefabMap = new();
    }
}

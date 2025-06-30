using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnBattleAllMonsters : MonoBehaviour
{
    public AllyMonsterSpawner allySpawner;
    public EnemyMonsterSpawner enemySpawner;

    private void Start()
    {
        // Serializable → Monster로 변환
        var serialized = BattleTriggerManager.Instance.GetSerializedPlayerTeam();
        Debug.Log($"[전투씬] 직렬화된 플레이어 팀 수: {serialized.Count}");
        foreach (var info in serialized)
        {
            Debug.Log($"[전투씬] 직렬화된 몬스터: {info.monsterData.monsterName}, Lv: {info.level}, HP: {info.curHp}");
        }
        var enemies = CreateMonsterList(BattleTriggerManager.Instance.GetSerializedEnemyTeam());
        var allies = CreateMonsterList(BattleTriggerManager.Instance.GetSerializedPlayerTeam());

        Debug.Log($"전투 시작: 플레이어 팀 {allies.Count}, 적 팀 {enemies.Count}");
        Debug.Log($"플레이어 팀 몬스터: {string.Join(", ", allies.Select(m => m.monsterData.monsterName))}");
        Debug.Log($"적 팀 몬스터: {string.Join(", ", enemies.Select(m => m.monsterData.monsterName))}");

        allySpawner.SpawnAllies(allies);

        BattleManager.Instance.enemyTeam = enemies;
    }

    private List<Monster> CreateMonsterList(List<SerializableMonsterInfo> infos)
    {
        List<Monster> monsters = new();

        foreach (var info in infos)
        {
            var prefab = BattleTriggerManager.Instance.GetPrefabByData(info.monsterData);
            if (prefab == null)
            {
                Debug.LogWarning($"[CreateMonsterList] 프리팹 없음: {info.monsterData.monsterName}");
                continue;
            }

            var go = Instantiate(prefab);
            var monster = go.GetComponent<Monster>();
            if (monster == null)
            {
                Debug.LogWarning($"[CreateMonsterList] Monster 컴포넌트 없음: {info.monsterData.monsterName}");
                continue;
            }

            monster.monsterData = info.monsterData;
            monster.SetLevel(info.level);
            monster.TakeDamage(monster.MaxHp - info.curHp);

            monsters.Add(monster);

            Debug.Log($"[CreateMonsterList] 몬스터 생성: {monster.monsterData.monsterName}, 레벨: {info.level}");
        }

        Debug.Log($"[CreateMonsterList] 총 몬스터 수: {monsters.Count}");
        return monsters;
    }

}

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
        var enemies = CreateMonsterList(BattleTriggerManager.Instance.GetSerializedEnemyTeam());
        var allies = CreateMonsterList(BattleTriggerManager.Instance.GetSerializedPlayerTeam());

        Debug.Log($"플레이어 팀 몬스터: {string.Join(", ", allies.Select(m => m.monsterData.monsterName))}");
        Debug.Log($"적 팀 몬스터: {string.Join(", ", enemies.Select(m => m.monsterData.monsterName))}");

        allySpawner.SpawnAllies(allies);
        enemySpawner.SpawnEnemies(enemies);

        BattleManager.Instance.enemyTeam = enemies;
    }

    private List<Monster> CreateMonsterList(List<SerializableMonsterInfo> infos)
    {
        List<Monster> monsters = new();

        foreach (var info in infos)
        {
            // Resources 폴더에서 이름으로 프리팹 자동 로딩
            var prefab = Resources.Load<GameObject>($"Monsters/{info.monsterData.monsterName}");
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
        }

        return monsters;
    }

}

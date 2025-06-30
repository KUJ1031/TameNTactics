using System.Collections.Generic;
using UnityEngine;

public class EnemyMonsterSpawner : MonoBehaviour
{
    [Header("적 몬스터 소환 위치 (최대 3)")]
    public GameObject defaultMonsterPrefab;
    public Transform[] spawnPositions;

    public void SpawnEnemies(List<Monster> enemies)
    {
        for (int i = 0; i < enemies.Count && i < spawnPositions.Length; i++)
        {
            var data = enemies[i];
            var go = Instantiate(defaultMonsterPrefab, spawnPositions[i].position, Quaternion.identity);
            var monster = go.GetComponent<Monster>();
            monster = data;

            monster.ApplyMonsterData(); // 외형 등 갱신
            //monster.LoadMonsterBaseStatData(); // 능력치 갱신
        }
    }
}

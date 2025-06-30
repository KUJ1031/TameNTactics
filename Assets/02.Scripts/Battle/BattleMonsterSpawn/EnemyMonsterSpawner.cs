using System.Collections.Generic;
using UnityEngine;

public class EnemyMonsterSpawner : MonoBehaviour
{
    [Header("적 몬스터 생성 위치 (최대 3개)")]
    public Transform[] spawnPositions;
    public void SpawnEnemies(List<Monster> enemies)
    {
        for (int i = 0; i < enemies.Count && i < spawnPositions.Length; i++)
        {
            Monster enemy = enemies[i];
            enemy.transform.position = spawnPositions[i].position;
            enemy.transform.rotation = spawnPositions[i].rotation;
        }
    }
}

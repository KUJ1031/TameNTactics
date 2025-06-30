using System.Collections.Generic;
using UnityEngine;

public class AllyMonsterSpawner : MonoBehaviour
{
    [Header("플레이어 몬스터 소환 위치 (최대 3)")]
    public Transform[] spawnPositions;

    public void SpawnAllies(List<Monster> allies)
    {
        for (int i = 0; i < allies.Count && i < spawnPositions.Length; i++)
        {
            Monster ally = allies[i];
            ally.transform.position = spawnPositions[i].position;
            ally.transform.rotation = spawnPositions[i].rotation;
        }
    }
}

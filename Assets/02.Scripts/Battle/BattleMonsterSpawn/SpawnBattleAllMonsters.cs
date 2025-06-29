using UnityEngine;

public class SpawnBattleAllMonsters : MonoBehaviour
{
    [Header("Spawner 연결")]
    public AllyMonsterSpawner allySpawner;
    public EnemyMonsterSpawner enemySpawner;

    private void Start()
    {
        var alliesData = BattleTriggerManager.Instance.GetPlayerTeamData();
        var enemiesData = BattleTriggerManager.Instance.GetEnemyTeamData();

        allySpawner.SpawnAllies(alliesData);
        enemySpawner.SpawnEnemies(enemiesData);
    }

}

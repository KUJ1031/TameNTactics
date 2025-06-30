using UnityEngine;

public class SpawnBattleAllMonsters : MonoBehaviour
{
    [Header("Spawner 연결")]
    public AllyMonsterSpawner allySpawner;
    public EnemyMonsterSpawner enemySpawner;

    private void Start()
    {
        var allies = BattleTriggerManager.Instance.GetPlayerTeam();
        var enemies = BattleTriggerManager.Instance.GetEnemyTeam();

        allySpawner.SpawnAllies(allies);
        enemySpawner.SpawnEnemies(enemies);
    }

}

using UnityEngine;
using System.Linq;

public class PlayerBattleTrigger : MonoBehaviour
{
    public BattleManager battleManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Monster monster = other.GetComponent<Monster>();
        if (monster == null) return;

        MonsterData enemyData = monster.GetData();
        if (enemyData == null) return;

        // 충돌한 몬스터 저장
        BattleTriggerManager.Instance.SetLastMonster(monster);


        // 적 팀 구성
        var factory = monster.transform.GetComponentInParent<MonsterFactory>();
        if (factory == null) return;

        var enemyTeam = factory.GetRandomEnemyTeam(); // List<Monster>
        BattleTriggerManager.Instance.SetEnemyTeam(enemyTeam);
        Debug.Log($"적 팀 구성 완료: {string.Join(", ", enemyTeam.Select(m => m.monsterData.monsterName))}");

        var player = PlayerManager.Instance.player;
        BattleTriggerManager.Instance.SetPlayerTeam(player.battleEntry);
        var playerDataList = player.battleEntry.Select(m => m.monsterData).ToList();

        BattleTriggerManager.Instance.SetBenchMonsters(player.benchEntry);

        if (battleManager != null)
        {
            
            battleManager.InitializeTeams();
            // battleManager.StartBattle();
        }       

        Destroy(other.gameObject); // 충돌한 적 제거
    }
}

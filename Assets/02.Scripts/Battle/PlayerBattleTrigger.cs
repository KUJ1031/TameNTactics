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
        BattleTriggerManager.Instance.SetLastMonster(enemyData);

        // 적 팀 구성
        var factory = monster.transform.GetComponentInParent<MonsterFactory>();
        if (factory == null) return;

        var enemyTeam = factory.GetRandomEnemyTeam();
        BattleTriggerManager.Instance.SetEnemyTeam(enemyTeam);

        // Player에서 전투팀 및 벤치 몬스터 가져오기
        var player = PlayerManager.Instance.player;
        BattleTriggerManager.Instance.SetPlayerTeam(player.battleEntry);
        BattleTriggerManager.Instance.SetBenchMonsters(player.benchEntry);
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleBattleScene");
        if (battleManager != null)
        {
            battleManager.InitializeTeams();
            // battleManager.StartBattle();
        }

        

        Destroy(other.gameObject); // 충돌한 적 제거
    }
}

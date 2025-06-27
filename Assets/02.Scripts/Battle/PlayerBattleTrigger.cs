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

        BattleTriggerManager.Instance.SetLastMonster(enemyData);

        var factory = monster.transform.GetComponentInParent<MonsterFactory>();
        if (factory == null) return;

        var enemyTeam = factory.GetRandomEnemyTeam();
        BattleTriggerManager.Instance.SetEnemyTeam(enemyTeam);
        BattleTriggerManager.Instance.SetPlayerTeam(EntryManager.Instance.selectedEntries);
        BattleTriggerManager.Instance.SetBenchMonsters(EntryManager.Instance.benchMonsters);
        BattleTriggerManager.Instance.SetLastMonster(monster.GetData());

        if (battleManager != null)
        {
            battleManager.InitializeTeams();
            //battleManager.StartBattle();
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleBattleScene");
        Destroy(other.gameObject);
    }
}

using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    public MonsterData enemyMonster;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 충돌 시 BattleManager에 전투 시작 요청
            BattleManager.Instance.StartBattle(enemyMonster);
        }
    }
}
using UnityEngine;

public class PlayerBattleTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 닿은 오브젝트가 Monster인지 확인
        Monster monster = other.GetComponent<Monster>();
        if (monster == null) return;

        MonsterData enemyData = monster.GetData();
        if (enemyData == null) return;

        // 적 몬스터 정보 저장
        BattleTriggerManager.Instance.SetLastMonster(enemyData);

        // 전투 시작 (필요 시 주석 해제)
       // BattleManager.Instance.StartBattle();

        // 몬스터 오브젝트 제거
        Destroy(other.gameObject); // 혹은 전투가 끝난 후에 처리
    }
}

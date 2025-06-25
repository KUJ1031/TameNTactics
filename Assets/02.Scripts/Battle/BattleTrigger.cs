using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    // 충돌 시 전투에 사용할 적 몬스터 데이터
    public MonsterData enemyMonster;

    // 중복 트리거 방지를 위한 플래그
    private bool hasTriggered = false;

    // 플레이어가 트리거 존에 들어올 때 호출됨
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 이미 트리거되었으면 아무 동작도 하지 않음
        if (hasTriggered) return;

        // 플레이어와 충돌했는지 확인
        if (other.CompareTag("Player"))
        {
            hasTriggered = true; // 중복 실행 방지

            // 1. 적 몬스터 정보를 BattleTriggerManager에 저장
            BattleTriggerManager.Instance.SetLastMonster(enemyMonster);

            // 2. 전투를 시작할 수 있도록 BattleManager 호출 준비
            //BattleManager.Instance.StartBattle(); // 현재는 주석 처리 중

            // 3. 트리거 오브젝트 제거 (전투 후 재진입 방지)
            Destroy(gameObject);
        }
    }
}

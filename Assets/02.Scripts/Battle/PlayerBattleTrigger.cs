using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerBattleTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //충돌 객체 정보 가지고오기
        MonsterFactory factory = other.GetComponentInParent<MonsterFactory>();
        if (factory == null) return;

        MonsterCharacter character = other.GetComponent<MonsterCharacter>();
        if (character == null) return;

        Monster monster = character.monster;
        if (monster == null) return;


        // 적 팀 구성
        List<Monster> enemyTeam = factory.GetRandomEnemyTeam(monster);

        //적 팀 배틀메니저로
        BattleManager.Instance.enemyTeam = enemyTeam;

        Destroy(other.gameObject); // 충돌한 적 제거

        RuntimePlayerSaveManager.Instance.SaveCurrentGameState(PlayerManager.Instance.player); // 현재 플레이어 상태 저장

        //씬이동
        UnityEngine.SceneManagement.SceneManager.LoadScene("BattleUITest");
    }

    public void DisableTriggerTemporarily(float disableTime)
    {
        Debug.Log("트리거 없애기 진입");
        StartCoroutine(DisableTriggerCoroutine(disableTime));
    }

    private IEnumerator DisableTriggerCoroutine(float time)
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;

        yield return new WaitForSeconds(time);

        collider.enabled = true;
    }
}

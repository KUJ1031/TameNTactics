using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerBattleTrigger : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DisableTriggerCoroutine(3f));
    }

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

        SceneManager.sceneLoaded += OnBattleSceneLoaded;

        //씬이동
        SceneManager.LoadScene("BattleScene");

    }

    private void OnBattleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BattleScene")
        {
            GameObject attackObj = GameObject.Find("AttackPosition");
            if (attackObj != null)
            {
                BattleManager.Instance.AttackPosition = attackObj.transform;
            }
            else
            {
                Debug.LogWarning("AttackPosition 오브젝트를 찾지 못했음.");
            }

            SceneManager.sceneLoaded -= OnBattleSceneLoaded;
        }
    }

    public void DisableTriggerTemporarily(float disableTime)
    {
        Debug.Log("트리거 없애기 진입");
        StartCoroutine(DisableTriggerCoroutine(disableTime));
    }

    private IEnumerator DisableTriggerCoroutine(float time)
    {
        BoxCollider2D collider = GetComponentInChildren<BoxCollider2D>();
      //  collider.enabled = false;

        yield return new WaitForSeconds(time);

      //  collider.enabled = true;
    }
}

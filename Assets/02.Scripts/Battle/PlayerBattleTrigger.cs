using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;

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

        List<Monster> enemyTeam;
        // 적 팀 구성
        if (!PlayerManager.Instance.player.playerBattleTutorialCheck)
        {
            Debug.Log("배틀 튜토리얼 중이므로 고정된 적 팀 사용");
            enemyTeam = factory.GetFixedEnemyTeam(monster);
        }
        else
        {
            enemyTeam = factory.GetRandomEnemyTeam(monster);
        }

        //적 팀 배틀메니저로
        BattleManager.Instance.enemyTeam = enemyTeam;

        // 배틀씬 진입 연출
        StartCoroutine(PlayBattleEntryEffectAndLoadScene(other.transform));

        //Destroy(other.gameObject); // 충돌한 적 제거

        //RuntimePlayerSaveManager.Instance.SaveCurrentGameState(PlayerManager.Instance.player); // 현재 플레이어 상태 저장

        //SceneManager.sceneLoaded += OnBattleSceneLoaded;

        ////씬이동
        //SceneManager.LoadScene("BattleScene");

    }

    private void OnBattleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BattleScene")
        {
            CameraController.Instance.ResetZoom();
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

    #region PlayBattleEffect

    private IEnumerator PlayBattleEntryEffectAndLoadScene(Transform monsterTr)
    {
        if (PlayerManager.Instance != null && PlayerManager.Instance.playerController != null)
            PlayerManager.Instance.playerController.isInputBlocked = true;

        CameraController.Instance.Zoom(2.5f, 0.5f);
        yield return new WaitForSecondsRealtime(0.2f);

        // 슬로우 모션
        Time.timeScale = 0.3f;
        yield return new WaitForSecondsRealtime(0.2f);

        // 페이드 아웃
        yield return StartCoroutine(FadeOutCoroutine(0.5f));
        Time.timeScale = 1f;

        RuntimePlayerSaveManager.Instance.SaveCurrentGameState(PlayerManager.Instance.player);
        Destroy(monsterTr.gameObject);

        SceneManager.sceneLoaded += OnBattleSceneLoaded;
        SceneManager.LoadScene("BattleScene");
    }

    #endregion

    #region FadeOut

    private IEnumerator FadeOutCoroutine(float duration = 0.5f)
    {
        Image fadeImage = FieldUIManager.Instance.FadePanel.GetComponent<Image>();

        if (fadeImage == null) yield break;

        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;

        float time = 0f;
        while (time < duration)
        {
            color.a = Mathf.Lerp(0f, 1f, time / duration);
            fadeImage.color = color;
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
    }

    #endregion
}

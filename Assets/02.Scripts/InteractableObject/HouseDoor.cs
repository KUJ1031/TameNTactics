using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class HouseDoor : MonoBehaviour
{
    public GameObject indoorGroup;
    public GameObject outdoorGroup;

    public float fadeDuration = 1f;

    private bool isInside = false;
    private Coroutine fadeCoroutine;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D");
        if (other.CompareTag("Player") && !isInside)
        {
            isInside = true;
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeGroups(1f, 0f)); // indoor fade in, outdoor fade out
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("OnTriggerExit2D");
        if (other.CompareTag("Player") && isInside)
        {
            isInside = false;

            // battleEntry 안에 있는 모든 몬스터가 죽었는지 체크
            var battleEntry = PlayerManager.Instance.player.battleEntry;
            bool allDead = battleEntry.Count > 0 && battleEntry.TrueForAll(mon => mon.CurHp <= 0);
            if (allDead)
            {
                PlayerManager.Instance.playerController.isInputBlocked = true; // 플레이어 입력 차단
                DialogueManager.Instance.StartDialogue("나", PlayerManager.Instance.playerImage[PlayerManager.Instance.player.playerGender], 1550);
                Debug.Log("모든 전투 엔트리 몬스터가 죽었습니다.");
            }

            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeGroups(0f, 1f)); // indoor fade out, outdoor fade in
        }
    }

    IEnumerator FadeGroups(float indoorTargetAlpha, float outdoorTargetAlpha)
    {
        indoorGroup.SetActive(true);
        outdoorGroup.SetActive(true);

        float timer = 0f;
        float indoorStartAlpha = GetGroupAlpha(indoorGroup);
        float outdoorStartAlpha = GetGroupAlpha(outdoorGroup);

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            float indoorAlpha = Mathf.Lerp(indoorStartAlpha, indoorTargetAlpha, t);
            float outdoorAlpha = Mathf.Lerp(outdoorStartAlpha, outdoorTargetAlpha, t);

            SetGroupAlpha(indoorGroup, indoorAlpha);
            SetGroupAlpha(outdoorGroup, outdoorAlpha);

            yield return null;
        }

        SetGroupAlpha(indoorGroup, indoorTargetAlpha);
        SetGroupAlpha(outdoorGroup, outdoorTargetAlpha);

        if (indoorTargetAlpha <= 0.01f)
            indoorGroup.SetActive(false);

        if (outdoorTargetAlpha <= 0.01f)
            outdoorGroup.SetActive(false);
    }


    float GetGroupAlpha(GameObject group)
    {
        var renderer = group.GetComponentInChildren<TilemapRenderer>();
        if (renderer != null)
            return renderer.material.color.a;
        return 1f;
    }

    void SetGroupAlpha(GameObject group, float alpha)
    {
        foreach (var r in group.GetComponentsInChildren<TilemapRenderer>())
        {
            Color c = r.material.color;
            c.a = alpha;
            r.material.color = c;
        }
    }
}

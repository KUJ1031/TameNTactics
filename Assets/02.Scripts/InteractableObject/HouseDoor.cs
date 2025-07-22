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
        if (other.CompareTag("Player") && !isInside)
        {
            isInside = true;
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeGroups(1f, 0f)); // indoor fade in, outdoor fade out
            Debug.Log("어서오세요~ 실내로 입장하셨습니다!");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isInside)
        {
            isInside = false;
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeGroups(0f, 1f)); // indoor fade out, outdoor fade in
            Debug.Log("나가셨어요~ 바깥바람 쐬고 오시게나!");
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

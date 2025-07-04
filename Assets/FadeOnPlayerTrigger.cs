using UnityEngine;
using System.Collections;

public class FadeOnPlayerTrigger : MonoBehaviour
{
    private SpriteRenderer sr;
    public float fadeAlpha = 0.4f;
    public float fadeDuration = 0.5f;  // 페이드 시간 (초)

    private float originalAlpha;
    private Coroutine fadeCoroutine;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            originalAlpha = sr.color.a;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && sr != null)
        {
            StartFade(fadeAlpha);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && sr != null)
        {
            StartFade(originalAlpha);
        }
    }

    private void StartFade(float targetAlpha)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeToAlpha(targetAlpha));
    }

    private IEnumerator FadeToAlpha(float targetAlpha)
    {
        float startAlpha = sr.color.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            Color c = sr.color;
            c.a = newAlpha;
            sr.color = c;
            yield return null;
        }

        // 마지막에 정확히 설정
        Color finalColor = sr.color;
        finalColor.a = targetAlpha;
        sr.color = finalColor;

        fadeCoroutine = null;
    }
}

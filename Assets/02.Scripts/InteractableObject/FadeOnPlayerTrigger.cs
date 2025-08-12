using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class FadeOnPlayerTrigger : MonoBehaviour
{
    public enum FadeTargetType
    {
        Sprite,
        Tilemap
    }

    [Header("페이드 대상 타입")]
    public FadeTargetType targetType;

    [Header("플레이어가 안에 있을 때 알파")]
    public float fadeAlpha = 0.4f;

    [Header("페이드 전환 시간")]
    public float fadeDuration = 0.5f;

    // 내부 참조
    private SpriteRenderer spriteRenderer;
    private Tilemap tilemap;
    private float originalAlpha;
    private Coroutine fadeCoroutine;

    void Start()
    {
        switch (targetType)
        {
            case FadeTargetType.Sprite:
                spriteRenderer = GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                    originalAlpha = spriteRenderer.color.a;
                break;

            case FadeTargetType.Tilemap:
                tilemap = GetComponent<Tilemap>();
                if (tilemap != null)
                    originalAlpha = tilemap.color.a;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            StartFade(fadeAlpha);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            StartFade(originalAlpha);
    }

    private void StartFade(float targetAlpha)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeToAlpha(targetAlpha));
    }

    private IEnumerator FadeToAlpha(float targetAlpha)
    {
        float startAlpha = GetCurrentAlpha();
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            SetAlpha(newAlpha);
            yield return null;
        }

        SetAlpha(targetAlpha);
        fadeCoroutine = null;
    }

    // 현재 알파값 가져오기
    private float GetCurrentAlpha()
    {
        switch (targetType)
        {
            case FadeTargetType.Sprite:
                return spriteRenderer != null ? spriteRenderer.color.a : 1f;
            case FadeTargetType.Tilemap:
                return tilemap != null ? tilemap.color.a : 1f;
            default:
                return 1f;
        }
    }

    // 알파값 적용하기
    private void SetAlpha(float a)
    {
        switch (targetType)
        {
            case FadeTargetType.Sprite:
                if (spriteRenderer != null)
                {
                    Color c = spriteRenderer.color;
                    c.a = a;
                    spriteRenderer.color = c;
                }
                break;

            case FadeTargetType.Tilemap:
                if (tilemap != null)
                {
                    Color c = tilemap.color;
                    c.a = a;
                    tilemap.color = c;
                }
                break;
        }
    }

}


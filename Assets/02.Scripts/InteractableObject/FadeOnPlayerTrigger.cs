using UnityEngine;
using System.Collections;

public class FadeOnPlayerTrigger : MonoBehaviour
{
    [Header("페이드 알파값 (플레이어가 안에 있을 때 투명도)")]
    public float fadeAlpha = 0.4f;

    [Header("페이드 전환 시간 (초)")]
    public float fadeDuration = 0.5f;

    // SpriteRenderer 참조
    private SpriteRenderer sr;

    // 원래 알파값 저장용
    private float originalAlpha;

    // 현재 실행 중인 페이드 코루틴
    private Coroutine fadeCoroutine;

    void Start()
    {
        // SpriteRenderer 가져오기 및 원래 알파 저장
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            originalAlpha = sr.color.a;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 트리거 안으로 들어오면 페이드 처리
        if (other.CompareTag("Player") && sr != null)
        {
            StartFade(fadeAlpha);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 플레이어가 트리거에서 나가면 원래 투명도로 복귀
        if (other.CompareTag("Player") && sr != null)
        {
            StartFade(originalAlpha);
        }
    }

    // 기존 페이드 코루틴 중지 후 새로 시작
    private void StartFade(float targetAlpha)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeToAlpha(targetAlpha));
    }

    // Sprite의 알파값을 부드럽게 변경하는 코루틴
    private IEnumerator FadeToAlpha(float targetAlpha)
    {
        float startAlpha = sr.color.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);

            // 알파값만 변경
            Color c = sr.color;
            c.a = newAlpha;
            sr.color = c;

            yield return null;
        }

        // 최종 알파값 보정
        Color finalColor = sr.color;
        finalColor.a = targetAlpha;
        sr.color = finalColor;

        fadeCoroutine = null;
    }
}

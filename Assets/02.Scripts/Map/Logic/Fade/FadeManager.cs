using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : Singleton<FadeManager>
{
    [Header("페이드 이미지")]
    public Image fadeImage; // 캔버스에 붙은 Image 컴포넌트를 할당

    [Header("페이드 속도")]
    public float fadeDuration = 1f;

    private Coroutine currentFadeCoroutine;

    protected override void Awake()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            SetAlpha(0f); // 시작 시 투명
        }
    }

    // alpha 값을 바로 설정
    private void SetAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
        }
    }

    /// <summary>
    /// 검은 화면으로 천천히 덮기 (1 = 검정)
    /// </summary>
    public void FadeOut(System.Action onComplete = null)
    {
        if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine);
        currentFadeCoroutine = StartCoroutine(Fade(0f, 1f, onComplete));
    }

    /// <summary>
    /// 천천히 화면을 밝히기 (0 = 투명)
    /// </summary>
    public void FadeIn(System.Action onComplete = null)
    {
        if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine);
        currentFadeCoroutine = StartCoroutine(Fade(1f, 0f, onComplete));
    }

    public void FadeOutThenIn(float delay, System.Action onFadeOutComplete, System.Action onFadeInComplete)
    {
        if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine);
        currentFadeCoroutine = StartCoroutine(FadeOutInRoutine(delay, onFadeOutComplete, onFadeInComplete));
    }

    private IEnumerator FadeOutInRoutine(float delay, System.Action onFadeOutComplete, System.Action onFadeInComplete)
    {
        // 페이드 아웃
        yield return Fade(0f, 1f, null);

        // 어두운 상태에서 이동 등 처리
        onFadeOutComplete?.Invoke();

        yield return new WaitForSeconds(delay);

        // 페이드 인
        yield return Fade(1f, 0f, onFadeInComplete);
    }

    public void FadeOutThenIn(float delay = 1f, System.Action onComplete = null)
    {
        if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine);
        currentFadeCoroutine = StartCoroutine(FadeOutInRoutine(delay, onComplete));
    }

    private IEnumerator FadeOutInRoutine(float delay, System.Action onComplete)
    {
        // 페이드 아웃
        yield return Fade(0f, 1f, null);

        // 암전 유지 시간
        yield return new WaitForSeconds(delay);

        // 페이드 인
        yield return Fade(1f, 0f, onComplete);
    }

    public void FadeOutWhiteWithSlow(
        float targetTimeScale = 0.1f,
        float slowDuration = 3.5f,
        System.Action onFadeMid = null,    // 페이드 중간 시점 호출
        System.Action onFadeComplete = null // 페이드 완료 후 호출
    )
    {
        if (fadeImage != null)
            fadeImage.color = new Color(1f, 1f, 1f, fadeImage.color.a); // 흰색 설정

        if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine);
        currentFadeCoroutine = StartCoroutine(FadeOutWhiteSlowRoutine(targetTimeScale, slowDuration, onFadeMid, onFadeComplete));
    }

    private IEnumerator FadeOutWhiteSlowRoutine(
        float targetTimeScale,
        float slowDuration,
        System.Action onFadeMid,
        System.Action onFadeComplete
    )
    {
        float startScale = Time.timeScale;
        float elapsed = 0f;

        // 흰색 페이드 아웃 시작
        Coroutine fadeCoroutine = StartCoroutine(Fade(0f, 1f, null));

        bool midCalled = false;

        while (elapsed < slowDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(startScale, targetTimeScale, elapsed / slowDuration);

            if (!midCalled && elapsed >= slowDuration * 0.5f)
            {
                midCalled = true;
                onFadeMid?.Invoke();
            }

            yield return null;
        }

        Time.timeScale = targetTimeScale;

        yield return fadeCoroutine;

        onFadeComplete?.Invoke();
    }





    private IEnumerator Fade(float start, float end, System.Action onComplete)
    {
        fadeImage.gameObject.SetActive(true); // 항상 켜기

        float time = 0f;
        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            float alpha = Mathf.Lerp(start, end, t);
            SetAlpha(alpha);
            time += Time.deltaTime;
            yield return null;
        }

        SetAlpha(end);

        if (end == 0f)
            fadeImage.gameObject.SetActive(false); // 완전히 투명하면 비활성화

        onComplete?.Invoke();
    }


}

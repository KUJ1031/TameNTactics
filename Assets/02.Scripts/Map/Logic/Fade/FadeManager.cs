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

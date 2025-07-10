using System.Collections;
using TMPro;
using UnityEngine;

public class MapNameDisplayManager : MonoBehaviour
{
    public static MapNameDisplayManager Instance { get; private set; }

    public TextMeshProUGUI mapNameText;     // 작게 표시되는 상시 맵 이름
    public TextMeshProUGUI bigMapNameText;  // 크게 표시되고 페이드아웃 되는 이름
    public float displayDuration = 3f;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ShowMapName(string name)
    {
        mapNameText.text = name;

        bigMapNameText.text = name;
        bigMapNameText.gameObject.SetActive(true);

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(FadeInOutCoroutine(displayDuration));
    }

    private IEnumerator FadeInOutCoroutine(float visibleDuration)
    {
        float fadeDuration = 1f;
        Color originalColor = bigMapNameText.color;

        // 1. 페이드인: alpha 0 -> 1
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            bigMapNameText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // 2. 표시 유지 시간
        yield return new WaitForSeconds(visibleDuration);

        // 3. 페이드아웃: alpha 1 -> 0
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            bigMapNameText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        bigMapNameText.gameObject.SetActive(false);
        bigMapNameText.color = originalColor;
    }

}

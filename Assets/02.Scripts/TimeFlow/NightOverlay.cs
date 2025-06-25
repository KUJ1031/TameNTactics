using UnityEngine;

public class NightOverlay : MonoBehaviour
{
    public CanvasGroup overlay; // 혹은 SpriteRenderer.color.a
    public float cycleDuration = 60f;
    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        float percentOfDay = (timer % cycleDuration) / cycleDuration;
        float t = Mathf.Sin((percentOfDay - 0.25f) * Mathf.PI * 2f) * 0.5f + 0.5f;
        overlay.alpha = Mathf.Lerp(0.5f, 1f, 1 - t); // 낮에는 투명, 밤에는 반투명
    }
}
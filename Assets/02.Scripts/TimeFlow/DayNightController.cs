using UnityEngine;
using UnityEngine.UI;

public class DayNightController : MonoBehaviour
{
    [Header("UI 오버레이")]
    public Image overlayImage; // Canvas 아래에 있는 Panel 또는 Image 오브젝트

    [Header("주기 설정")]
    public float cycleDuration = 60f; // 전체 하루 길이 (초 단위)

    [Header("색상 설정")]
    public Color dayColor = new Color(0f, 0f, 0f, 0f);      // 낮에는 완전 투명
    public Color nightColor = new Color(0f, 0f, 0f, 0.5f);   // 밤에는 반투명한 검정

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        // 현재 시간 비율 (0~1)
        float percentOfDay = (timer % cycleDuration) / cycleDuration;

        // 곡선적인 밝기 변화: 아침-정오-저녁
        float t = Mathf.Sin((percentOfDay - 0.25f) * Mathf.PI * 2f) * 0.5f + 0.5f;

        // 낮과 밤 색상 보간
        if (overlayImage != null)
        {
            overlayImage.color = Color.Lerp(nightColor, dayColor, t);
        }
    }
}

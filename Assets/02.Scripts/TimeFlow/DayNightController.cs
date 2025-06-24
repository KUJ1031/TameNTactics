using UnityEngine;

public class DayNightController : MonoBehaviour
{
    public Camera cam;
    public Color dayColor = new Color(0.5f, 0.8f, 1f); // 하늘색
    public Color nightColor = new Color(0.05f, 0.05f, 0.2f); // 어두운 남색
    public float cycleDuration = 60f; // 낮/밤 주기

    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        float percentOfDay = (timer % cycleDuration) / cycleDuration;
        float t = Mathf.Sin((percentOfDay - 0.25f) * Mathf.PI * 2f) * 0.5f + 0.5f;
        cam.backgroundColor = Color.Lerp(nightColor, dayColor, t);
    }
}

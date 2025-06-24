using TMPro;
using UnityEngine;

public class GameTimeFlow : MonoBehaviour
{
    public float dayLengthInSeconds = 2880f; // 게임 내 하루 = 48분
    private float timer;

    public TextMeshProUGUI timeText;

    void Update()
    {
        timer += Time.deltaTime;

        // 1시간은 하루의 1/24, 즉 전체 주기 / 24
        float secondsPerGameHour = dayLengthInSeconds / 24f;
        float gameHours = (timer / secondsPerGameHour) % 24f;

        int hours = Mathf.FloorToInt(gameHours);
        int minutes = Mathf.FloorToInt((gameHours - hours) * 60f);

        string timeString = string.Format("{0:00}:{1:00}", hours, minutes);
        Debug.Log("현재 게임 시간: " + timeString);

        if (this.timeText != null)
        {
            this.timeText.text = timeString;
        }
    }
}

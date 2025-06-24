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

        float secondsPerGameHour = dayLengthInSeconds / 24f;
        float gameHours = (timer / secondsPerGameHour) % 24f;

        int hours24 = Mathf.FloorToInt(gameHours);
        int minutes = Mathf.FloorToInt((gameHours - hours24) * 60f);

        // 12시간제로 변환
        string period = hours24 < 12 ? "AM" : "PM";
        int hours12 = hours24 % 12;
        if (hours12 == 0) hours12 = 12; // 0시는 12AM, 12시는 12PM

        string timeString = string.Format("{0:00}:{1:00} {2}", hours12, minutes, period);
        Debug.Log("현재 게임 시간: " + timeString);

        if (this.timeText != null)
        {
            this.timeText.text = timeString;
        }
    }
}

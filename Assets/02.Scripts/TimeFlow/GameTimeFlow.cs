using TMPro;
using UnityEngine;

/// <summary>
/// 게임 내 시간 흐름을 시뮬레이션하고,
/// 이를 UI에 12시간제로 표시하는 클래스
/// </summary>
public class GameTimeFlow : MonoBehaviour
{
    [Header("하루가 흐르는 실제 시간(초 단위)")]
    public float dayLengthInSeconds = 2880f; // 게임 내 하루 = 48분

    private float timer; // 실제 경과 시간 누적

    [Header("시간을 표시할 TextMeshProUGUI")]
    public TextMeshProUGUI timeText;

    void Update()
    {
        // 매 프레임마다 실제 시간 누적
        timer += Time.deltaTime;

        // 게임 내 1시간이 흐르기 위한 실제 시간 계산
        float secondsPerGameHour = dayLengthInSeconds / 24f;

        // 현재 게임 시간 (0~24 사이의 부동소수점 숫자)
        float gameHours = (timer / secondsPerGameHour) % 24f;

        // 정수 시(hour)와 분(minute) 계산
        int hours24 = Mathf.FloorToInt(gameHours);
        int minutes = Mathf.FloorToInt((gameHours - hours24) * 60f);

        // 24시간제를 12시간제로 변환
        string period = hours24 < 12 ? "AM" : "PM";
        int hours12 = hours24 % 12;
        if (hours12 == 0) hours12 = 12; // 0시는 12AM, 12시는 12PM

        // 시계 형식 문자열 생성
        string timeString = string.Format("{0:00}:{1:00} {2}", hours12, minutes, period);

        // UI에 시간 표시
        if (this.timeText != null)
        {
            this.timeText.text = timeString;
        }
    }
}

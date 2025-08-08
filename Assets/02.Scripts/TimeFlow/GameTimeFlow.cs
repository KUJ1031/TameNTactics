using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 게임 내 시간 흐름을 시뮬레이션하고,
/// 이를 UI에 12시간제로 표시하는 클래스
/// </summary>
public class GameTimeFlow : Singleton<GameTimeFlow>
{
    [Header("시간 흐름 설정")]
    public float dayLengthInSeconds = 2880f; // 게임 내 하루 = 48분
    internal float timer;
    internal string timeString;

    [Header("시간 UI")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI playTimeText;

    [Header("낮/밤 오버레이 - 이미지 방식")]
    public Image overlayImage; // Canvas에 어두운 Image 사용
    public Color dayColor = new Color(0, 0, 0, 0);
    public Color nightColor = new Color(0, 0, 0, 0.5f);

    private float visualTimeOffset;

    private void Start()
    {
        timer = PlayerManager.Instance.player.playerLastGameTime;
        visualTimeOffset = dayLengthInSeconds / 2f; // 12시 시작용 offset
    }

    private void Update()
    {
        timer += Time.deltaTime;

        float displayTime = (timer + visualTimeOffset) % dayLengthInSeconds;
        float normalizedTime = displayTime / dayLengthInSeconds;

        UpdateTimeDisplay();
        UpdateDayNightOverlay();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var canvas = GameObject.Find("UI")?.transform;
        if (canvas != null)
        {
            timeText = TransformUtil.FindDeepChild(canvas, "InGameTimeText")?.GetComponent<TextMeshProUGUI>();
            playTimeText = TransformUtil.FindDeepChild(canvas, "PlayTimeText")?.GetComponent<TextMeshProUGUI>();
            overlayImage = TransformUtil.FindDeepChild(canvas, "DayNightOverlay")?.GetComponent<Image>();
        }
    }

    public void UpdateTimeDisplay()
    {
        float secondsPerGameHour = dayLengthInSeconds / 24f;
        float visualTimer = (timer + visualTimeOffset) % dayLengthInSeconds;
        float gameHours = (visualTimer / secondsPerGameHour) % 24f;

        int hours24 = Mathf.FloorToInt(gameHours);
        int minutes = Mathf.FloorToInt((gameHours - hours24) * 60f);

        string period = hours24 < 12 ? "AM" : "PM";
        int hours12 = hours24 % 12;
        if (hours12 == 0) hours12 = 12;

        timeString = string.Format("{0:00}:{1:00} {2}", hours12, minutes, period);

        if (timeText != null) timeText.text = timeString;

        // 총 플레이 시간 텍스트
        if (playTimeText != null)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
            playTimeText.text = timeSpan.ToString(@"hh\:mm\:ss");
        }
    }


    private void UpdateDayNightOverlay()
    {
        if (overlayImage == null) return;

        float visualTimer = (timer + visualTimeOffset) % dayLengthInSeconds;
        float percentOfDay = visualTimer / dayLengthInSeconds;

        float brightness = 0f;

        if (percentOfDay >= 0.2f && percentOfDay <= 0.8f)
        {
            float dayProgress = (percentOfDay - 0.2f) / 0.6f;
            brightness = Mathf.Sin(dayProgress * Mathf.PI);
        }

        float alpha = Mathf.Lerp(0.9f, 0f, brightness);
        overlayImage.color = new Color(0f, 0f, 0f, alpha);
    }





    public float GetCurrentTimer() => timer;

    public void SetTimer(float newTime)
    {
        timer = newTime;
        PlayerManager.Instance.player.playerLastGameTime = newTime;
        UpdatePlayTimeText(PlayerManager.Instance.player.totalPlaytime);
    }

    public void UpdatePlayTimeText(int totalPlaytime)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(totalPlaytime);
        if (playTimeText != null)
        {
            playTimeText.text = "플레이 시간: " + timeSpan.ToString(@"hh\:mm\:ss");
        }
    }
}

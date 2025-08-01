using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; // Linq 사용 시 필요

public class LoadUI : MonoBehaviour
{
    public GameObject startEntrySlotPrefab; // 프리팹
    public Transform contentParent;         // ScrollView > Viewport > Content

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI ownedMonsterCountText;
    public TextMeshProUGUI gameTimeText;
    public TextMeshProUGUI playerLastGameTimeText;
    public TextMeshProUGUI playerLastStageText;
    public TextMeshProUGUI goldText;
    public Image playerImage;

    public Image displayImage;   // 이미지 보여줄 UI
    public Button leftButton;    // 왼쪽 버튼
    public Button rightButton;   // 오른쪽 버튼
    public Image[] images;      // 이미지 배열

    private int currentIndex = 0;

    private void Start()
    {
        if (images.Length > 0)
            displayImage = images[currentIndex];

        leftButton.onClick.AddListener(OnClickLeft);
        rightButton.onClick.AddListener(OnClickRight);

        // 저장된 데이터에서 게임 시간만 불러와 UI 갱신
        TakeLoadDataUI();
    }

    private void OnEnable()
    {
        currentIndex = 0;
        UpdateImageVisibility();
    }

    public void TakeLoadDataUI()
    {
        Player loadedPlayer = PlayerSaveManager.Instance.LoadPlayerData();

        if (loadedPlayer != null)
        {

            // 플레이어 이름 설정
            nameText.text = $"{loadedPlayer.playerName}";

            // 소유한 몬스터 수 설정
            ownedMonsterCountText.text = $"소유한 몬스터 수 : {loadedPlayer.ownedMonsters.Count}마리";



            // 플레이어 마지막 위치 설정
            playerLastStageText.text = $"{loadedPlayer.playerLastStage}";
            Debug.Log($"마지막 위치 : {loadedPlayer.playerLastStage}");

            // 플레이어 플레잉 타임 설정
            float totalSeconds = loadedPlayer.totalPlaytime;
            int hours = (int)(totalSeconds / 3600);
            int minutes = (int)((totalSeconds % 3600) / 60);
            int seconds = (int)(totalSeconds % 60);

            gameTimeText.text = $"플레이 타임 : {hours:D2}시간 {minutes:D2}분 {seconds:D2}초";

            // 플레이어 마지막 게임 시간 설정
            string timeString = GetGameTimeFormatted(loadedPlayer.playerLastGameTime, GameTimeFlow.Instance.dayLengthInSeconds);
            playerLastGameTimeText.text = $"마지막 게임 시간 : {timeString}";

            // 골드 설정
            goldText.text = $"골드 : {loadedPlayer.gold}";

            // 플레이어 이미지 설정 (예시로 기본 이미지 사용)
            Debug.Log($"플레이어 성별 : {loadedPlayer.playerGender}");
            playerImage.sprite = PlayerManager.Instance.playerImage[loadedPlayer.playerGender];

            CreateEntrySlots(loadedPlayer.entryMonsters);
        }
        else
        {
            gameTimeText.text = "게임 시간이 없습니다.";
        }
    }

    private void CreateEntrySlots(List<Monster> entryMonsters)
    {
        // 기존 슬롯 제거
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var monster in entryMonsters)
        {
            GameObject slot = Instantiate(startEntrySlotPrefab, contentParent);

            // 슬롯 내부 컴포넌트 가져오기
            Image monsterImage = slot.transform.Find("MonsterImage")?.GetComponent<Image>();
            TextMeshProUGUI nameText = slot.transform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI levelText = slot.transform.Find("LevelText")?.GetComponent<TextMeshProUGUI>();

            // 데이터 넣기
            if (monsterImage != null) monsterImage.sprite = monster.monsterData.monsterImage;
            if (nameText != null) nameText.text = monster.monsterName;
            if (levelText != null) levelText.text = $"Lv.{monster.Level}";
        }
    }

    public static string GetGameTimeFormatted(float playerLastGameTime, float dayLengthInSeconds)
    {
        float secondsPerGameHour = dayLengthInSeconds / 24f;
        float gameHours = (playerLastGameTime / secondsPerGameHour) % 24f;

        int hours24 = Mathf.FloorToInt(gameHours);
        int minutes = Mathf.FloorToInt((gameHours - hours24) * 60f);

        string period = hours24 < 12 ? "AM" : "PM";
        int hours12 = hours24 % 12;
        if (hours12 == 0) hours12 = 12;

        return $"{period} {hours12:00}:{minutes:00}";
    }

    public void OnClickLeft()
    {
        if (images.Length == 0) return;

        currentIndex--;
        if (currentIndex < 0) currentIndex = images.Length - 1;

        UpdateImageVisibility();
    }

    public void OnClickRight()
    {
        if (images.Length == 0) return;

        currentIndex++;
        if (currentIndex >= images.Length) currentIndex = 0;

        UpdateImageVisibility();
    }

    private void UpdateImageVisibility()
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(!(i == currentIndex));
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class QuestUI : FieldMenuBaseUI
{
    [Header("Quest Info 영역")]
    public Image questImage;
    public TextMeshProUGUI questInfoText;

    [Header("Quest Slot 관련")]
    public GameObject questSlotPrefab;
    public Transform questContentParent;

    [Header("Quest 버튼들")]
    [SerializeField] private Button orderQuestButton;
    [SerializeField] private Button completedButton;
    [SerializeField] private Button nonStartedButton;

    [Header("Quest 이미지")]
    public Sprite[] defaultquestImage;

    [Header("알림 텍스트")]
    [SerializeField] private TextMeshProUGUI alertText;

    private List<GameObject> currentSlots = new List<GameObject>();

    private void Start()
    {
        orderQuestButton.onClick.AddListener(ShowOngoingQuests);
        completedButton.onClick.AddListener(ShowCompletedQuests);
        nonStartedButton.onClick.AddListener(ShowNotStartedQuests);
    }

    private void OnEnable()
    {
        ShowOngoingQuests();
    }

    private void ClearSlots()
    {
        foreach (var slot in currentSlots)
        {
            Destroy(slot);
        }
        currentSlots.Clear();

        // 이미지 초기화 : 투명화 + sprite null 처리
        questImage.sprite = null;
        SetQuestImageVisible(false);

        questInfoText.text = "";
        alertText.gameObject.SetActive(false);
    }


    public void CreateQuestSlot(QuestData data)
    {
        GameObject slotObj = Instantiate(questSlotPrefab, questContentParent);
        QuestSlotUI slot = slotObj.GetComponent<QuestSlotUI>();
        slot.Setup(data, this);
        currentSlots.Add(slotObj);
    }

    public void ShowQuestInfo(QuestData data)
    {
        var player = PlayerManager.Instance.player;
        var questList = QuestManager.Instance.GetQuestList();
        int questIndex = questList.IndexOf(data);

        if (data.prerequisiteQuestIndex.HasValue)
        {
            int prereqIndex = data.prerequisiteQuestIndex.Value;
            bool prereqCleared = player.playerQuestClearCheck.TryGetValue(prereqIndex, out bool cleared) && cleared;

            if (!prereqCleared)
            {
                // sprite 명확히 null로 초기화
                questImage.sprite = null;
                SetQuestImageVisible(false);

                questInfoText.text = "이 퀘스트는 아직 잠겨 있습니다.";
                return;
            }
        }


        SetQuestImageVisible(true, data.questSprite);
        questInfoText.text = data.GetCurrentDescription(player, questIndex);
    }




    private void SetQuestImageVisible(bool visible, Sprite sprite = null)
    {
        questImage.sprite = visible ? sprite : null;
        Color c = questImage.color;
        c.a = visible ? 1f : 0f;
        questImage.color = c;
    }

    private void ShowOngoingQuests()
    {
        ClearSlots();

        var player = PlayerManager.Instance.player;
        var questList = QuestManager.Instance.GetQuestList();

        for (int i = 0; i < questList.Count; i++)
        {
            bool started = player.playerQuestStartCheck.ContainsKey(i) && player.playerQuestStartCheck[i];
            bool cleared = player.playerQuestClearCheck.ContainsKey(i) && player.playerQuestClearCheck[i];

            if (started && !cleared)
            {
                CreateQuestSlot(questList[i]);
            }
        }

        if (currentSlots.Count == 0)
        {
            alertText.gameObject.SetActive(true);
            alertText.text = "수주 중인 퀘스트가 없습니다.";
        }
    }

    private void ShowCompletedQuests()
    {
        ClearSlots();

        var player = PlayerManager.Instance.player;
        var questList = QuestManager.Instance.GetQuestList();

        for (int i = 0; i < questList.Count; i++)
        {
            bool cleared = player.playerQuestClearCheck.ContainsKey(i) && player.playerQuestClearCheck[i];
            if (cleared)
            {
                CreateQuestSlot(questList[i]);
            }
        }

        if (currentSlots.Count == 0)
        {
            alertText.gameObject.SetActive(true);
            alertText.text = "완료된 퀘스트가 없습니다.";
        }
    }

    private void ShowNotStartedQuests()
    {
        ClearSlots();

        var player = PlayerManager.Instance.player;
        var questList = QuestManager.Instance.GetQuestList();

        for (int i = 0; i < questList.Count; i++)
        {
            bool started = player.playerQuestStartCheck.TryGetValue(i, out bool isStarted) && isStarted;
            bool cleared = player.playerQuestClearCheck.TryGetValue(i, out bool isCleared) && isCleared;

            if (!started && !cleared)
            {
                CreateQuestSlot(questList[i]);
            }
        }

        if (currentSlots.Count == 0)
        {
            alertText.gameObject.SetActive(true);
            alertText.text = "미수주 퀘스트가 없습니다.";
        }
    }
}

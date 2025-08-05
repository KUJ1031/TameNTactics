using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class QuestUI : MonoBehaviour
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

    private List<GameObject> currentSlots = new List<GameObject>();

    private void Start()
    {
        orderQuestButton.onClick.AddListener(ShowOngoingQuests);
        completedButton.onClick.AddListener(ShowCompletedQuests);
        nonStartedButton.onClick.AddListener(ShowNotStartedQuests);

        ShowOngoingQuests();  // 초기에는 수주중인 퀘스트만 표시
    }

    private void ClearSlots()
    {
        foreach (var slot in currentSlots)
        {
            Destroy(slot);
        }
        currentSlots.Clear();
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
        questImage.sprite = data.questSprite != null ? data.questSprite : null;
        questInfoText.text = data.detailedDescription;
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
    }

}

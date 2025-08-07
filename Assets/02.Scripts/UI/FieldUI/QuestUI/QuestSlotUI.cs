using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlotUI : MonoBehaviour
{
    public Image questIconImage;
    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI questExplainText;
    public TextMeshProUGUI questCurrentText;

    private QuestData currentData;
    private QuestUI questUI;

    public void Setup(QuestData data, QuestUI ui)
    {
        currentData = data;
        questUI = ui;

        int questIndex = QuestManager.Instance.GetQuestList().IndexOf(data);
        var player = PlayerManager.Instance.player;

        // 선행 퀘스트 체크
        bool isLocked = data.prerequisiteQuestIndex.HasValue &&
                        (!player.playerQuestClearCheck.TryGetValue(data.prerequisiteQuestIndex.Value, out bool cleared) || !cleared);

        if (isLocked)
        {
            // 잠긴 상태 UI 처리
            questIconImage.color = new Color(0, 0, 0, 0.5f); // 어둡게
            questNameText.text = "???";
            questExplainText.text = "???";
            questCurrentText.text = "잠김";
            questCurrentText.color = Color.gray;
            return;
        }

        // 잠금이 아니면 정상 표시
        questIconImage.sprite = data.questSprite;
        questIconImage.color = Color.white;

        questNameText.text = data.questName;
        questExplainText.text = data.slotDescription;

        bool isStarted = player.playerQuestStartCheck.TryGetValue(questIndex, out bool started) && started;
        bool isCleared = player.playerQuestClearCheck.TryGetValue(questIndex, out bool isClear) && isClear;

        QuestStatus status = isCleared ? QuestStatus.Completed :
                            isStarted ? QuestStatus.InProgress :
                            QuestStatus.NotStarted;

        questCurrentText.text = status switch
        {
            QuestStatus.NotStarted => "미시작",
            QuestStatus.InProgress => "진행 중",
            QuestStatus.Completed => "완료됨",
            _ => ""
        };

        questCurrentText.color = status switch
        {
            QuestStatus.NotStarted => Color.red,
            QuestStatus.InProgress => Color.blue,
            QuestStatus.Completed => Color.black,
            _ => Color.gray
        };
    }

    public void OnClickSlot()
    {
        if (currentData == null || questUI == null)
            return;

        int index = QuestManager.Instance.GetQuestList().IndexOf(currentData);
        var player = PlayerManager.Instance.player;

        if (currentData.prerequisiteQuestIndex.HasValue)
        {
            int preIndex = currentData.prerequisiteQuestIndex.Value;
            bool isUnlocked = player.playerQuestClearCheck.TryGetValue(preIndex, out bool cleared) && cleared;

            if (!isUnlocked)
            {
                // 경고창 띄우기
                questUI.questInfoText.text = "<color=red>이 퀘스트는 아직 잠겨 있습니다.\n선행 퀘스트를 완료해야 합니다.</color>";
                return;
            }
        }

        // 잠금이 아니라면 정보 출력
        questUI.ShowQuestInfo(currentData);
    }

}

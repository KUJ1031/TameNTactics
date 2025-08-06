using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    public QuestUI questUI; // 연결된 QuestUI

    private List<QuestData> questList = new();

    private void Start()
    {
        AddQuest(new QuestData(
            "[전투의 기본]", "튜토리얼을 진행하자.",
            "앞을 향해 나아가자.",
            "허겁지겁 위협을 피해 도망쳐왔지만...\n눈 앞에 또 다시 몬스터가 있다.\n\n...더 이상 물러날 곳이 없다. 맞서 싸워야만 한다.",
            QuestStatus.NotStarted,
            questUI.defaultquestImage[0],
            null // 선행 없음
        ));

        AddQuest(new QuestData(
            "[레거의 편지]", "편지를 찾아야 한다.",
            "미지의 숲 앞에 있는 NPC인 [핑거]를 찾아가자.",
            "미지의 숲에 들어가려는 찰나, 핑거가 부탁해왔다.\n\"동생의 편지를 찾아줘.\"\n\n그의 편지는 어디에 있는걸까?\n...미지의 수풀 속을 잘 찾아보면 나올지도..?",
            QuestStatus.NotStarted,
            questUI.defaultquestImage[1],
            0 // [전투의 기본] 완료 필요
        ));

        AddQuest(new QuestData(
            "[끊어진 다리]", "목수는 어디에?",
            "끊어진 다리 앞에 있는 NPC인 [파인]을 찾아가자.",
            "길을 연결해주는 다리가 끊어져 있다...\nNPC의 말에 따르면, 어떤 다리든 수리할 수 있는 목수가 있다고 하는데...어디에 있는걸까?\n\n초보 사냥터 위쪽에 잠겨있는 장소가 수상하다...",
            QuestStatus.NotStarted,
            questUI.defaultquestImage[2],
            0 // [전투의 기본] 완료 필요
        ));

        AddQuest(new QuestData(
            "[떠돌이 상인]", "신기한 물건을 판다고?",
            "마을에 가끔씩, 이상한 상인이 출몰한다고 한다.\n듣던 바로는 신기한 물건들을 이것저것 판다고 하는데...\n마을 어딘가에 갑자기 나온다고 하니, 한 번 찾아가보자.\n\n[떠돌이 상인]은 매 홀수(1, 3..)시에 나타난다고 한다.",
            "마을에 가끔씩, 이상한 상인이 출몰한다고 한다.\n듣던 바로는 신기한 물건들을 이것저것 판다고 하는데...\n마을 어딘가에 갑자기 나온다고 하니, 한 번 찾아가보자.\n\n[떠돌이 상인]은 매 홀수(1, 3..)시에 나타난다고 한다.",
            QuestStatus.NotStarted,
            questUI.defaultquestImage[3],
            0 // [전투의 기본] 완료 필요
        ));

        AddQuest(new QuestData(
            "[빼앗긴 나무]", "보스를 토벌하자.",
            "길을 막아서는 엘리트 몬스터들을 무찌르자.",
            "드디어 목수를 찾아냈다!\n그런데... 기껏 베어놓은 나무를 뺏길 위기에 처해있다.\n\"저 녀석이 내 나무를 가져가려고 해!!\"\n\n보스를 퇴치하고, 목수를 데려가 다리를 재건해야 한다.",
            QuestStatus.NotStarted,
            questUI.defaultquestImage[4],
            2 // [끊어진 다리] 완료 필요
        ));
    }

    public bool IsQuestStarted(int questID)
    {
        if (questID < 0 || questID >= questList.Count)
            return false;

        return questList[questID].questStatus == QuestStatus.InProgress || questList[questID].questStatus == QuestStatus.Completed;
    }

    public void AddQuest(QuestData data)
    {
        questList.Add(data);
        questUI.CreateQuestSlot(data);
    }

    public List<QuestData> GetQuestList()
    {
        return questList;
    }
}

[System.Serializable]
public class QuestData
{
    public string questName;
    public string slotDescription;

    public string notStartedDescription;      // 수주 전 설명
    public string inProgressDescription;      // 수주 중 설명
    public string completedDescription;       // 완료 후 설명 (선택사항)

    public QuestStatus questStatus;
    public Sprite questSprite;
    public int? prerequisiteQuestIndex;

    public QuestData(
        string name,
        string slotDesc,
        string notStartedDesc,
        string inProgressDesc,
        QuestStatus status,
        Sprite sprite,
        int? prerequisite = null
    )
    {
        questName = name;
        slotDescription = slotDesc;
        notStartedDescription = notStartedDesc;
        inProgressDescription = inProgressDesc;
        questStatus = status;
        questSprite = sprite;
        prerequisiteQuestIndex = prerequisite;
    }



    // 동적으로 현재 상태의 설명 반환
    public string GetCurrentDescription(Player player, int questIndex)
    {
        bool started = player.playerQuestStartCheck.TryGetValue(questIndex, out bool isStarted) && isStarted;
        bool cleared = player.playerQuestClearCheck.TryGetValue(questIndex, out bool isCleared) && isCleared;

        if (cleared) return completedDescription ?? inProgressDescription;
        if (started) return inProgressDescription;
        return notStartedDescription;
    }
}

public enum QuestStatus
{
    NotStarted,
    InProgress,
    Completed
}

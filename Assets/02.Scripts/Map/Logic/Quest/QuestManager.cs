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
            "허겁지겁 위협을 피해 도망쳐왔지만...\n눈 앞에 또 다시 몬스터가 있다.\n\n...더 이상 물러날 곳이 없다. 맞서 싸워야만 한다.",
            QuestStatus.NotStarted,
            questUI.defaultquestImage[0],
            null // 선행 없음
        ));

        AddQuest(new QuestData(
            "[레거의 편지]", "편지를 찾아야 한다.",
            "미지의 숲에 들어가려는 찰나, 핑거가 부탁해왔다.\n\"동생의 편지를 찾아줘.\"\n\n그의 편지는 어디에 있는걸까?\n...미지의 수풀 속을 잘 찾아보면 나올지도..?",
            QuestStatus.NotStarted,
            questUI.defaultquestImage[1],
            0 // [전투의 기본] 완료 필요
        ));

        AddQuest(new QuestData(
            "[끊어진 다리]", "목수는 어디에?",
            "길을 연결해주는 다리가 끊어져 있다...\nNPC의 말에 따르면, 어떤 다리든 수리할 수 있는 목수가 있다고 하는데...\n어디에 있는걸까?\n\n초보 사냥터 위쪽에 잠겨있는 장소가 수상하다...",
            QuestStatus.NotStarted,
            questUI.defaultquestImage[2],
            0 // [전투의 기본] 완료 필요
        ));

        AddQuest(new QuestData(
            "[떠돌이 상인]", "신기한 물건을 판다고?",
            "마을에 가끔씩, 이상한 상인이 출몰한다고 한다.\n듣던 바로는 신기한 물건들을 이것저것 판다고 하는데...\n마을 어딘가에 갑자기 나온다고 하니, 한 번 찾아가보자.\n\n[떠돌이 상인]은 매 홀수(1, 3..)시에 나타난다고 한다.",
            QuestStatus.NotStarted,
            questUI.defaultquestImage[3],
            0 // [전투의 기본] 완료 필요
        ));

        AddQuest(new QuestData(
            "[붙잡힌 목수]", "보스를 토벌하자.",
            "드디어 목수를 찾아냈다!\n그런데... 갇혀있던 것도 모자라 착취까지 당하고 있었다.\n\"제발 살려줘! 뭐든지 해줄테니까!!\"\n\n보스를 퇴치하고, 목수를 구출하여 다리를 재건해야 한다.",
            QuestStatus.NotStarted,
            questUI.defaultquestImage[4],
            2 // [끊어진 다리] 완료 필요
        ));
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
    public string questName;                    // 퀘스트 이름
    public string slotDescription;              // 퀘스트 슬롯에 표시될 간단 설명
    public string detailedDescription;          // 퀘스트 클릭 시 보여줄 상세 설명
    public QuestStatus questStatus;             // 퀘스트 상태 (NotStarted, InProgress, Completed)
    public Sprite questSprite;                  // 퀘스트 대표 이미지
    public int? prerequisiteQuestIndex;         // 선행 퀘스트 인덱스 (없으면 null)

    public QuestData(
        string name,
        string slotDesc,
        string detailDesc,
        QuestStatus status,
        Sprite sprite,
        int? prerequisite = null
    )
    {
        questName = name;
        slotDescription = slotDesc;
        detailedDescription = detailDesc;
        questStatus = status;
        questSprite = sprite;
        prerequisiteQuestIndex = prerequisite;
    }
}

public enum QuestStatus
{
    NotStarted,
    InProgress,
    Completed
}

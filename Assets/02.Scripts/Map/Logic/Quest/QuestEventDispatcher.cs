using System;

public static class QuestEventDispatcher
{
    public static Action<int> OnQuestStarted;  // 퀘스트 ID를 넘기는 콜백
}

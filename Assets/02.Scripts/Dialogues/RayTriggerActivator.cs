using UnityEngine;

public class RayTriggerActivator : MonoBehaviour
{
    private RaycastEventTrigger trigger;
    public int questIDToWatch = 2; // 감시할 퀘스트 ID

    private void Awake()
    {
        trigger = GetComponentInParent<RaycastEventTrigger>();
    }

    private void OnEnable()
    {
        QuestEventDispatcher.OnQuestStarted += HandleQuestStarted;
    }

    private void OnDisable()
    {
        QuestEventDispatcher.OnQuestStarted -= HandleQuestStarted;
    }

    private void HandleQuestStarted(int questID)
    {
        if (trigger == null)
        {
            Debug.LogWarning($"{gameObject.name}: trigger is null!");
            return;
        }

        Debug.Log($"{gameObject.name}: Found trigger on {trigger.gameObject.name}");

        if (questID == questIDToWatch)
        {
            trigger.canRepeat = false;
            Debug.Log($"Quest {questID} started, setting canRepeat to false for {trigger.gameObject.name}. New value: {trigger.canRepeat}");
        }
    }
}

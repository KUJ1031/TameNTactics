using UnityEngine;

public class RayTriggerActivator : MonoBehaviour
{
    private RaycastEventTrigger trigger;
    public int questIDToWatch = 2; // 감시할 퀘스트 ID

    private void Awake()
    {
        trigger = GetComponentInParent<RaycastEventTrigger>();
    }

    private void Start()
    {
        // 퀘스트가 이미 시작되었는지 체크해서 canRepeat을 미리 끔
        if (QuestManager.Instance != null && QuestManager.Instance.IsQuestStarted(questIDToWatch))
        {
            HandleQuestStarted(questIDToWatch);
        }
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

        if (questID == questIDToWatch)
        {
            trigger.canRepeat = false;

            var uniqueID = trigger.gameObject.name + trigger.transform.position.ToString();
            DialogueSaveSystem.SaveRayTriggerState(uniqueID, true);

            Debug.Log($"Quest {questID} started, setting canRepeat to false for {trigger.gameObject.name}. New value: {trigger.canRepeat}");
        }
    }
}

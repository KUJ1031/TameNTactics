using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAlertManager : Singleton<EventAlertManager>
{
    public EventAlertPopup eventAlertPopup;
    public EventAlertSlot alertSlotPrefab; // 알림 슬롯 프리팹
    public Transform alertSlotsParent;

    private Queue<EventAlertRequest> alertQueue = new Queue<EventAlertRequest>();
    public static List<EventAlertRequest> pendingAlerts = new List<EventAlertRequest>();

    private bool isDisplaying = false;

    void Start()
    {
        // 씬 전환 후 대기 중인 알림 실행
        foreach (var request in pendingAlerts)
        {
            Instance.SetEventAlert(
                request.alertType,
                request.itemData,
                request.name,
                request.quantity
            );
        }
        pendingAlerts.Clear();
    }
    public void SetEventAlert(EventAlertType alertType, ItemData itemData = null, string name = null, int quantity = 0)
    {
        EventAlertRequest request = new EventAlertRequest(alertType, itemData, name, quantity);
        alertQueue.Enqueue(request);

        if (!isDisplaying)
            StartCoroutine(ProcessAlertQueue());
    }

    private IEnumerator ProcessAlertQueue()
    {
        isDisplaying = true;

        // 팝업 부모 활성화
        eventAlertPopup.gameObject.SetActive(true);

        while (alertQueue.Count > 0)
        {
            EventAlertRequest request = alertQueue.Dequeue();
            yield return StartCoroutine(DisplayAlert(request));
        }

        // 팝업 부모 비활성화
        eventAlertPopup.gameObject.SetActive(false);
        isDisplaying = false;
    }


    private IEnumerator DisplayAlert(EventAlertRequest request)
    {
        EventAlertSlot instance = Instantiate(alertSlotPrefab, alertSlotsParent);
        RectTransform rect = instance.GetComponent<RectTransform>();

        // 초기 위치 세팅 전에 비활성화 (렌더링 차단)
        instance.gameObject.SetActive(false);

        // 알림 내용 설정
        switch (request.alertType)
        {
            case EventAlertType.Wanderer_Appear:
                instance.alertImage.sprite = eventAlertPopup.wandererImage;
                instance.eventAlertType.text = "[이벤트]";
                instance.eventAlertText.text = $"맵 어딘가에 <color=#D27905>[떠돌이 상인]</color>이 나타났습니다! (30분)";
                break;

            case EventAlertType.Wanderer_DisAppear:
                instance.alertImage.sprite = eventAlertPopup.wandererImage;
                instance.eventAlertType.text = "[이벤트]";
                instance.eventAlertText.text = "시간이 다 되어 <color=#D27905>[떠돌이 상인]</color>이 사라졌습니다!";
                break;
            case EventAlertType.GetItem:
                instance.alertImage.sprite = request.itemData.itemImage;
                instance.eventAlertType.text = "[아이템]";
                instance.eventAlertText.text = $"[<color=#D93333>{request.name}</color>] 아이템을 {request.quantity}개 획득하였습니다.\n인벤토리에서 확인해주세요.";
                break;
            case EventAlertType.RemoveItem:
                instance.alertImage.sprite = request.itemData.itemImage;
                instance.eventAlertType.text = "[아이템]";
                instance.eventAlertText.text = $"[<color=#D93333>{request.name}</color>] 아이템을 {request.quantity}개 제거하였습니다.";
                break;
            case EventAlertType.SendItem:
                instance.alertImage.sprite = request.itemData.itemImage;
                instance.eventAlertType.text = "[아이템]";
                instance.eventAlertText.text = $"[<color=#D93333>{request.name}</color>] 아이템을 {request.quantity}개 건네주었습니다.";
                break;
            case EventAlertType.QuestStart:
                instance.alertImage.sprite = eventAlertPopup.questImage;
                instance.eventAlertType.text = "[퀘스트]";
                instance.eventAlertText.text = $"<color=#D27905>{request.name}</color> 퀘스트가 시작되었습니다!";
                break;
            case EventAlertType.QuestClear:
                instance.alertImage.sprite = eventAlertPopup.questImage;
                instance.eventAlertType.text = "[퀘스트]";
                instance.eventAlertText.text = $"<color=#D27905>{request.name}</color> 퀘스트가 완료되었습니다!";
                break;
            case EventAlertType.Save:
                instance.alertImage.sprite = eventAlertPopup.saveImage;
                instance.eventAlertType.text = "[저장]";
                instance.eventAlertText.text = "게임이 저장되었습니다.";
                break;
            case EventAlertType.LevelUp:
                instance.alertImage.sprite = eventAlertPopup.levelUpImage; // 레벨업 전용 아이콘
                instance.eventAlertType.text = "[레벨업]";
                instance.eventAlertText.text =
                    $"<color=#3ED90D>{request.name}</color>이(가) {request.quantity}레벨이 되었습니다!";
                break;
        }

        Vector2 startPos = new Vector2(rect.anchoredPosition.x, 200);
        Vector2 endPos = new Vector2(rect.anchoredPosition.x, 0);
        Vector2 exitPos = new Vector2(rect.anchoredPosition.x, 200);
        float duration = 0.5f;

        // 위치 세팅 후에 활성화
        rect.anchoredPosition = startPos;
        instance.gameObject.SetActive(true);

        // 슬라이드 인
        float elapsed = 0;
        while (elapsed < duration)
        {
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        rect.anchoredPosition = endPos;

        yield return new WaitForSeconds(3f);

        // 슬라이드 아웃
        elapsed = 0;
        while (elapsed < duration)
        {
            rect.anchoredPosition = Vector2.Lerp(endPos, exitPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(instance.gameObject);
    }

    public static void AddPendingAlert(EventAlertType alertType, ItemData itemData = null, string name = null, int quantity = 0)
    {
        pendingAlerts.Add(new EventAlertRequest(alertType, itemData, name, quantity));
    }

}

public class EventAlertRequest
{
    public EventAlertType alertType;
    public ItemData itemData;
    public string name;
    public int quantity;

    public EventAlertRequest(EventAlertType alertType, ItemData itemData, string name, int quantity)
    {
        this.alertType = alertType;
        this.itemData = itemData;
        this.name = name;
        this.quantity = quantity;
    }
}
public enum EventAlertType
{
    None,
    Wanderer_Appear,
    Wanderer_DisAppear,
    GetItem,
    RemoveItem,
    SendItem,
    QuestStart,
    QuestClear,
    Save,
    LevelUp
}

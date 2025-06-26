using TMPro;
using UnityEngine;

public class EntryButton : MonoBehaviour
{
    [Header("버튼 연결 요소들")]
    public MonsterData monsterData; // Fireman 등
    public EntryManager entryManager;
    public TextMeshProUGUI buttonText; // UI 버튼 텍스트

    private void Start()
    {
        // 초기 UI 갱신
        UpdateButtonUI();

        // 출전 상태 변경 시 UI 갱신 구독
        if (entryManager != null)
        {
            entryManager.OnEntryChanged += UpdateButtonUI;
        }
    }

    private void OnDestroy()
    {
        // 메모리 누수 방지 위해 이벤트 구독 해제
        if (entryManager != null)
        {
            entryManager.OnEntryChanged -= UpdateButtonUI;
        }
    }

    public void OnClick()
    {
        entryManager.ToggleEntry(monsterData);
        // 버튼 클릭 시 UI 갱신은 OnEntryChanged가 처리하지만
        // 그래도 바로 반영하고 싶으면 여기서도 호출 가능
        UpdateButtonUI();
    }

    public void UpdateButtonUI()
    {
        if (entryManager.IsInEntry(monsterData))
            buttonText.text = "UnEquip";
        else
            buttonText.text = "Equip";
    }
}

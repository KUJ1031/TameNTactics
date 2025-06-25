using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 몬스터 한 마리를 ScrollView에 슬롯으로 표시하는 클래스
/// - 이미지, 이름 표시
/// - Equip / UnEquip 버튼 표시 및 클릭 처리
/// - EntryManager와 연동하여 상태 유지
/// </summary>
public class MonsterSlot : MonoBehaviour
{
    [Header("UI 연결")]
    public Image monsterImageUI;                  // 몬스터 이미지 출력
    public TextMeshProUGUI monsterNameText;       // 몬스터 이름 출력
    public Button actionButton;                   // Equip / UnEquip 버튼
    public TextMeshProUGUI actionButtonText;      // 버튼 내 텍스트

    private MonsterData currentData;              // 현재 슬롯에 할당된 몬스터 데이터

    /// <summary>
    /// 버튼에 클릭 리스너 연결
    /// </summary>
    void Start()
    {
        actionButton.onClick.AddListener(OnClick);
    }

    /// <summary>
    /// 몬스터 데이터를 슬롯에 설정하고 UI 갱신
    /// </summary>
    public void SetData(MonsterData data)
    {
        currentData = data;

        // UI 요소 반영
        monsterImageUI.sprite = data.monsterImage;
        monsterNameText.text = data.monsterName;

        UpdateButtonUI(); // 버튼 상태 반영
    }

    /// <summary>
    /// Equip/UnEquip 버튼 클릭 시 호출됨
    /// </summary>
    void OnClick()
    {
        EntryManager.Instance.ToggleEntry(currentData); // 출전 토글
        UpdateButtonUI(); // 즉시 UI 반영
    }

    /// <summary>
    /// 현재 상태에 따라 버튼 텍스트 갱신
    /// </summary>
    void UpdateButtonUI()
    {
        if (EntryManager.Instance.IsInEntry(currentData))
            actionButtonText.text = "UnEquip";
        else
            actionButtonText.text = "Equip";
    }

    /// <summary>
    /// 슬롯이 활성화되면 이벤트 구독
    /// </summary>
    void OnEnable()
    {
        EntryManager.Instance.OnEntryChanged += UpdateButtonUI;
    }

    /// <summary>
    /// 슬롯이 비활성화되면 이벤트 구독 해제 (메모리 누수 방지)
    /// </summary>
    void OnDisable()
    {
        EntryManager.Instance.OnEntryChanged -= UpdateButtonUI;
    }
}

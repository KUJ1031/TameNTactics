using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 출전 가능한 몬스터 관리 클래스 (싱글톤)
/// - 최대 출전 수 제한
/// - 현재 출전 중인 몬스터 리스트 관리
/// - ScrollView에 몬스터 목록 표시
/// </summary>
public class EntryManager : Singleton<EntryManager>
{
    [Header("최대 출전 수")]
    public int maxEntryCount = 3;

    [Header("현재 출전 리스트")]
    public List<MonsterData> allMonsters; // 에디터에서 등록할 전체 몬스터 리스트
    public List<MonsterData> selectedEntries = new List<MonsterData>(); // 현재 출전 중인 몬스터 리스트

    [Header("ScrollView 관련")]
    public Transform contentPanel; // ScrollView의 Content 오브젝트
    public GameObject monsterSlotPrefab; // 몬스터 슬롯 프리팹

    // 출전 목록 변경 시 호출되는 이벤트 (버튼 UI 등 갱신용)
    public event System.Action OnEntryChanged;

    /// <summary>
    /// 게임 시작 시 첫 번째 몬스터 자동 출전 등록 (최소 1명 보장)
    /// </summary>
    protected override void Awake()
    {
        if (selectedEntries.Count == 0 && allMonsters.Count > 0)
        {
            selectedEntries.Add(allMonsters[0]);
            Debug.Log($"{allMonsters[0].monsterName} 자동 출전 등록 (시작 시)");
            OnEntryChanged?.Invoke(); // UI에 반영
        }
    }

    /// <summary>
    /// ScrollView 초기화: 모든 몬스터 슬롯 생성
    /// </summary>
    private void Start()
    {
        InitializeAllSlots();
    }

    /// <summary>
    /// 모든 몬스터를 슬롯으로 생성하여 ScrollView에 배치
    /// </summary>
    void InitializeAllSlots()
    {
        foreach (Transform child in contentPanel)
            Destroy(child.gameObject); // 기존 슬롯 제거

        foreach (var monster in allMonsters)
        {
            GameObject slot = Instantiate(monsterSlotPrefab, contentPanel);
            slot.GetComponent<MonsterSlot>().SetData(monster); // 몬스터 데이터 주입
        }
    }

    /// <summary>
    /// 몬스터 출전 등록 또는 해제 토글 처리
    /// </summary>
    public void ToggleEntry(MonsterData monster)
    {
        bool isSelected = selectedEntries.Contains(monster);

        if (isSelected) // 해제 시도
        {
            if (selectedEntries.Count <= 1)
            {
                Debug.LogWarning("최소 1마리는 출전해야 합니다. 해제할 수 없습니다.");
                return;
            }
            else
            {
                selectedEntries.Remove(monster);
                Debug.Log($"{monster.monsterName} 출전 해제");
            }
        }
        else // 장착 시도
        {
            if (selectedEntries.Count >= maxEntryCount)
            {
                Debug.LogWarning("최대 출전 수 초과");
                return;
            }

            selectedEntries.Add(monster);
            Debug.Log($"{monster.monsterName} 출전 등록");
        }

        // UI에 변경 사항 반영
        OnEntryChanged?.Invoke();
    }

    /// <summary>
    /// 해당 몬스터가 출전 중인지 여부 확인
    /// </summary>
    public bool IsInEntry(MonsterData monster)
    {
        return selectedEntries.Contains(monster);
    }
}

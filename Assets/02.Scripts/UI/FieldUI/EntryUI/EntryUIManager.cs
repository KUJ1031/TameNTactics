using System.Collections.Generic;
using UnityEngine;


public class EntryUIManager : Singleton<EntryUIManager>
{
    [SerializeField] private Transform BattleParent;
    [SerializeField] private Transform BenchParent;
    [SerializeField] private GameObject EntrySlotPrefab;
    [SerializeField] private EntryUI entryUI;

    private List<EntrySlotUI> battleEntrySlots = new();
    private List<EntrySlotUI> benchEntrySlots = new();

    private EntrySlotUI selectedSlot;
    private GameObject placeholder; // 드래그 중 예상 위치에 보여줄 객체

    private const int BATTLE_SLOT_COUNT = 3;
    private const int BENCH_SLOT_COUNT = 2;


    public void SetEntryUISlots()
    {
        ClearAllSlots();
        SetSlots();
        if (battleEntrySlots.Count > 0) // 슬롯이 없을 경우 에러 방지
        {
            SelectSlot(battleEntrySlots[0]);
        }
    }

    /// <summary>
    /// 배틀 및 벤치 슬롯을 Player 데이터 기반으로 설정합니다.
    /// </summary>
    private void SetSlots()
    {
        Player p = PlayerManager.Instance.player;

        MakeSlotsForParent(BattleParent, p.battleEntry, battleEntrySlots, BATTLE_SLOT_COUNT);
        MakeSlotsForParent(BenchParent, p.benchEntry, benchEntrySlots, BENCH_SLOT_COUNT);
    }

    /// <summary>
    /// 지정된 부모에 몬스터 데이터를 기반으로 슬롯 UI를 생성하고 리스트에 추가합니다.
    /// </summary>
    private void MakeSlotsForParent(Transform parent, List<Monster> entryList, List<EntrySlotUI> slotUIList, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Monster mon = i < entryList.Count ? entryList[i] : null;
            MakeSlot(parent, mon, slotUIList);
        }
    }

    /// <summary>
    /// 단일 슬롯 UI를 생성하고 초기화한 후 리스트에 추가합니다.
    /// </summary>
    private void MakeSlot(Transform parent, Monster monster, List<EntrySlotUI> slotList)
    {
        GameObject go = Instantiate(EntrySlotPrefab, parent);
        var slot = go.GetComponent<EntrySlotUI>();

        if (monster != null)
            slot.Init(monster);
        else
            slot.VoidSlotInit();

        slotList.Add(slot);
    }

    /// <summary>
    /// 모든 배틀 및 벤치 슬롯 UI를 파괴하고 관련 리스트를 초기화합니다.
    /// </summary>
    private void ClearAllSlots()
    {
        // 자식 객체들을 역순으로 파괴하여 컬렉션 변경 오류 방지
        for (int i = BattleParent.childCount - 1; i >= 0; i--)
        {
            Destroy(BattleParent.GetChild(i).gameObject);
        }
        for (int i = BenchParent.childCount - 1; i >= 0; i--)
        {
            Destroy(BenchParent.GetChild(i).gameObject);
        }

        battleEntrySlots.Clear();
        benchEntrySlots.Clear();
        selectedSlot = null;
    }

    /// <summary>
    /// 슬롯을 선택하고 UI 상세 정보를 업데이트합니다.
    /// </summary>
    public void SelectSlot(EntrySlotUI slot)
    {
        if (slot == selectedSlot)
            return;

        if (selectedSlot != null)
            selectedSlot.SetSelected(false);

        selectedSlot = slot;
        selectedSlot.SetSelected(true);

        entryUI.SetMonsterDetailUI(selectedSlot.GetMonster());
    }

    /// <summary>
    /// 모든 슬롯 리스트에서 해당 슬롯을 제거합니다.
    /// </summary>
    private void TryRemoveFromAllSlotLists(EntrySlotUI slot)
    {
        battleEntrySlots.Remove(slot);
        benchEntrySlots.Remove(slot);
    }

    #region 드래그

    /// <summary>
    /// 드래그된 슬롯이 드롭되었을 때의 처리를 담당합니다.
    /// </summary>
    public void OnDrop(EntrySlotUI draggedSlot, Transform dropTarget, Vector2 pointerPosition, Transform originalParent)
    {
        ClearPlaceholder(); // 드롭 시 플레이스홀더 제거

        if (dropTarget == null)
        {
            // 유효한 드롭 대상이 없으면 원래 위치로 복구
            RestoreToOriginalParentList(draggedSlot, originalParent);
            return;
        }

        TryRemoveFromAllSlotLists(draggedSlot);

        int insertIndex = GetInsertIndex(dropTarget, pointerPosition);

        // 드롭 대상에 따라 슬롯 처리
        if (dropTarget == BattleParent)
        {
            HandleDrop(BattleParent, battleEntrySlots, draggedSlot, insertIndex);
        }
        else if (dropTarget == BenchParent)
        {
            HandleDrop(BenchParent, benchEntrySlots, draggedSlot, insertIndex);
        }

        // TODO: 실제 게임 데이터 처리 (PlayerManager 등에서 몬스터 위치 업데이트 로직 호출)
        // 예: PlayerManager.Instance.MoveMonsterData(draggedSlot.GetMonster(), originalParent, dropTarget, insertIndex);
    }

    /// <summary>
    /// 드래그 중 예상 위치에 표시할 플레이스홀더를 생성합니다.
    /// </summary>
    public void CreatePlaceholder(Transform parent, int siblingIndex)
    {
        if (placeholder != null)
            Destroy(placeholder);

        placeholder = Instantiate(EntrySlotPrefab, parent);
        placeholder.transform.SetSiblingIndex(siblingIndex);
        var slotUI = placeholder.GetComponent<EntrySlotUI>();
        if (slotUI != null)
        {
            slotUI.VoidSlotInit();
        }
        var cg = placeholder.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 0.6f;
            cg.blocksRaycasts = false;
        }
    }

    /// <summary>
    /// 드래그 중 플레이스홀더의 위치를 업데이트합니다.
    /// </summary>
    public void UpdatePlaceholderPosition(Vector2 pointerPos)
    {
        if (placeholder == null) return;

        Transform dropTarget = GetDropTarget(pointerPos);
        if (dropTarget == null) { return; }

        int index = GetInsertIndex(dropTarget, pointerPos);
        placeholder.transform.SetParent(dropTarget);
        placeholder.transform.SetSiblingIndex(index);
    }

    /// <summary>
    /// 플레이스홀더를 제거합니다.
    /// </summary>
    public void ClearPlaceholder()
    {
        if (placeholder != null)
        {
            Destroy(placeholder);
            placeholder = null;
        }
    }

    /// <summary>
    /// 드래그된 슬롯을 지정된 위치에 삽입합니다.
    /// </summary>
    private void HandleDrop(Transform dropTargetParent, List<EntrySlotUI> entrySlots, EntrySlotUI draggedSlot, int insertIndex)
    {
        draggedSlot.transform.SetParent(dropTargetParent);

        // insertIndex가 유효한 범위 내에 있을 경우 삽입, 아니면 맨 뒤에 추가
        if (insertIndex >= 0 && insertIndex <= entrySlots.Count)
        {
            entrySlots.Insert(insertIndex, draggedSlot);
            draggedSlot.transform.SetSiblingIndex(insertIndex);
        }
        else
        {
            entrySlots.Add(draggedSlot);
            draggedSlot.transform.SetAsLastSibling();
        }
    }

    /// <summary>
    /// 드래그가 취소되었을 때 슬롯을 원래 부모 리스트로 복구합니다.
    /// </summary>
    private void RestoreToOriginalParentList(EntrySlotUI slot, Transform originalParent)
    {
        if (originalParent == BattleParent)
        {
            battleEntrySlots.Add(slot);
        }
        else if (originalParent == BenchParent)
        {
            benchEntrySlots.Add(slot);
        }
    }

    /// <summary>
    /// 포인터 위치에 따라 드롭 가능한 부모를 반환합니다.
    /// </summary>
    public Transform GetDropTarget(Vector2 pointerPosition)
    {
        Player p = PlayerManager.Instance.player;

        if (p.battleEntry.Count >= BATTLE_SLOT_COUNT && p.benchEntry.Count > 0)
        {
            if (IsPointerInYRange(BattleParent, pointerPosition))
                return BattleParent;

            if (IsPointerInYRange(BenchParent, pointerPosition))
                return BenchParent;
        }
        else
        {
            return BattleParent;
        }
        return null;
    }

    /// <summary>
    /// 포인터가 지정된 부모의 Y 범위 내에 있는지 확인합니다.
    /// </summary>
    private bool IsPointerInYRange(Transform parent, Vector2 pointerPos)
    {
        RectTransform rect = parent.GetComponent<RectTransform>();
        if (rect == null) return false;

        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);

        float topY = corners[1].y;
        float bottomY = corners[0].y;

        return pointerPos.y >= bottomY && pointerPos.y <= topY;
    }


    /// <summary>
    /// 드래그 중인 포인터 위치를 기반으로 슬롯이 삽입될 인덱스를 계산합니다.
    /// </summary>
    public int GetInsertIndex(Transform parent, Vector2 pointerScreenPos)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            RectTransform child = parent.GetChild(i) as RectTransform;
            if (child == null) continue;

            Vector3[] corners = new Vector3[4];
            child.GetWorldCorners(corners);
            float top = corners[1].y;
            float bottom = corners[0].y;
            float centerY = (top + bottom) * 0.5f;

            // 마우스가 현재 슬롯의 중앙보다 위에 있으면 그 위치에 반환
            if (pointerScreenPos.y > centerY)
            {
                return i;
            }
        }
        // 모든 자식보다 아래에 있으면 마지막 인덱스 반환
        return parent.childCount;
    }
    #endregion
}
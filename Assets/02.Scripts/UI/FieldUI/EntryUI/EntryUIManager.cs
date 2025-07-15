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

    protected override void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetEntryUISlots()
    {
        ClearAllSlots();
        SetSlots();
        SelectSlot(battleEntrySlots[0]);
    }

    //슬롯 세팅
    private void SetSlots()
    {
        Player p = PlayerManager.Instance.player;

        for (int i = 0; i < 3; i++)
        {
            Monster mon = i < p.battleEntry.Count ? p.battleEntry[i] : null;
            MakeSlot(BattleParent, mon, battleEntrySlots);
        }

        for (int i = 0; i < 2; i++)
        {
            Monster mon = i < p.benchEntry.Count ? p.benchEntry[i] : null;
            MakeSlot(BenchParent, mon, benchEntrySlots);
        }
    }

    //슬롯 만들기
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

    //슬롯 비우기
    private void ClearAllSlots()
    {
        foreach (Transform child in BattleParent)
            Destroy(child.gameObject);
        foreach (Transform child in BenchParent)
            Destroy(child.gameObject);

        battleEntrySlots.Clear();
        benchEntrySlots.Clear();
        selectedSlot = null;
    }

    //슬롯 선택
    public void SelectSlot(EntrySlotUI slot)
    {
        if (slot == selectedSlot)
            return;

        if (selectedSlot != null)
            selectedSlot.SetSelected(false);

        selectedSlot = slot;
        selectedSlot.SetSelected(true);
        Debug.Log(selectedSlot.GetMonster().monsterName);
        entryUI.SetMonsterDetailUI(selectedSlot.GetMonster());
    }

    //슬롯지우기
    private void RemoveFromAllSlotLists(EntrySlotUI slot)
    {
        battleEntrySlots.Remove(slot);
        benchEntrySlots.Remove(slot);
    }


    #region 드래그 앤 드랍

    //드랍되었을 때 처리
    public void OnDrop(EntrySlotUI draggedSlot, Transform dropTarget, Vector2 pointerPosition, Transform originalParent)
    {
        if (dropTarget == null)
        {
            RestoreToOriginalParentList(draggedSlot, originalParent);
            return;
        }

        RemoveFromAllSlotLists(draggedSlot);

        int insertIndex = GetInsertIndex(dropTarget, pointerPosition);

        if (dropTarget == BattleParent)
        {
            HandleDrop(BattleParent, battleEntrySlots, draggedSlot, insertIndex);
        }
        else if (dropTarget == BenchParent)
        {
            HandleDrop(BenchParent, benchEntrySlots, draggedSlot, insertIndex);
        }
        //데이터 처리
    }

    //예상 위치에 보여줄 객체 만들기
    public void CreatePlaceholder(Transform parent, int siblingIndex)
    {
        if (placeholder != null)
            Destroy(placeholder);

        placeholder = Instantiate(EntrySlotPrefab, parent);
        placeholder.transform.SetSiblingIndex(siblingIndex);
        placeholder.GetComponent<EntrySlotUI>().VoidSlotInit();
        var cg = placeholder.GetComponent<CanvasGroup>();
        cg.alpha = 0.6f;
        cg.blocksRaycasts = false;
    }

    //예상 위치 객체 위치 변경
    public void UpdatePlaceholderPosition(Vector2 pointerPos)
    {
        if (placeholder == null) return;

        Transform dropTarget = GetDropTarget(pointerPos);
        if (dropTarget == null) return;

        int index = GetInsertIndex(dropTarget, pointerPos);
        placeholder.transform.SetParent(dropTarget);
        placeholder.transform.SetSiblingIndex(index);
    }

    //예상 위치 객체 삭제
    public void ClearPlaceholder()
    {
        if (placeholder != null)
        {
            Destroy(placeholder);
            placeholder = null;
        }
    }

    //해당 인덱스에 만들기
    private void HandleDrop(Transform dropTargetParent, List<EntrySlotUI> entrySlots, EntrySlotUI draggedSlot, int insertIndex)
    {
        draggedSlot.transform.SetParent(dropTargetParent);

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

    //부모 리스트 원상복구
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

    // 드래그 중 포인터가 어느 부모 위에 있는지 판단
    public Transform GetDropTarget(Vector2 pointerPosition)
    {
        if (IsPointerInYRange(BattleParent, pointerPosition))
            return BattleParent;

        if (IsPointerInYRange(BenchParent, pointerPosition))
            return BenchParent;

        return null;
    }
    //부모 위치 스크린좌표로 확인
    private bool IsPointerInYRange(Transform parent, Vector2 pointerPos)
    {
        RectTransform rect = parent.GetComponent<RectTransform>();

        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);

        float topY = corners[1].y;
        float bottomY = corners[0].y;

        return pointerPos.y >= bottomY && pointerPos.y <= topY;
    }


    //드래그 중 위치로 인덱스 계산
    public int GetInsertIndex(Transform parent, Vector2 pointerScreenPos)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            RectTransform child = parent.GetChild(i) as RectTransform;
            if (child == null) continue;

            // 슬롯의 월드 기준 상하좌표 가져오기
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

        return parent.childCount;
    }
    #endregion
}

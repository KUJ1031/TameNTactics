using System.Collections.Generic;
using UnityEngine;

public class EntryUIManager : Singleton<EntryUIManager>
{
    [SerializeField] private Transform BattleParent;
    [SerializeField] private Transform BenchParent;
    [SerializeField] private GameObject EntrySlotPrefab;

    private List<EntrySlotUI> battleEntrySlots = new();
    private List<EntrySlotUI> benchEntrySlots = new();
    private EntrySlotUI selectedSlot;

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
    //슬롯비우기
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
        {
            //selectedSlot.SetSelected(false);
            //selectedSlot = null;
            return;
        }

        if (selectedSlot != null)
            selectedSlot.SetSelected(false);

        selectedSlot = slot;
        selectedSlot.SetSelected(true);
    }

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

    //슬롯지우기
    private void RemoveFromAllSlotLists(EntrySlotUI slot)
    {
        battleEntrySlots.Remove(slot);
        benchEntrySlots.Remove(slot);
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
        RectTransform battleRect = BattleParent.GetComponent<RectTransform>();
        RectTransform benchRect = BenchParent.GetComponent<RectTransform>();

        //포인터가 요소의 RectTransform 안에 들어왔으면 해당 요소 리턴
        if (RectTransformUtility.RectangleContainsScreenPoint(battleRect, pointerPosition))
            return BattleParent;
        if (RectTransformUtility.RectangleContainsScreenPoint(benchRect, pointerPosition))
            return BenchParent;

        return null;
    }
    //드래그 중 위치로 인덱스 계산
    public int GetInsertIndex(Transform parent, Vector2 pointerPos)
    {
        RectTransform parentRect = parent.GetComponent<RectTransform>();

        //현재 마우스 위치를 부모의 로컬로 변환(부모 안에 있는지 확인)
        Vector2 localPointInParent;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, pointerPos, null, out localPointInParent))
        {
            return -1;
        }

        //부모의 자식 맨 위에서부터 비교한 뒤 특정 자식 의 Y보다 높아지면 해당 인덱스반환
        //없으면 자식의 개수(맨아래)인덱스 반환
        for (int i = 0; i < parent.childCount; i++)
        {
            RectTransform childRect = parent.GetChild(i).GetComponent<RectTransform>();
            if (childRect == null) continue;

            Vector2 localPointInChild;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                childRect,
                pointerPos,
                null,
                out localPointInChild
            );

            if (localPointInChild.y > 0)
            {
                return i;
            }
        }
        return parent.childCount;
    }
}

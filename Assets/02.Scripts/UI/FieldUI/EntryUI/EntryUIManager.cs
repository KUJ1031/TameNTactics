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
        foreach (var slot in battleEntrySlots)
            Destroy(slot.gameObject);
        foreach (var slot in benchEntrySlots)
            Destroy(slot.gameObject);

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

    public void OnDrop(EntrySlotUI draggedSlot, Transform dropTarget)
    {
        // 부모 바꾸기
        draggedSlot.transform.SetParent(dropTarget);

        // 슬롯 리스트 재배치
        if (battleEntrySlots.Contains(draggedSlot))
            battleEntrySlots.Remove(draggedSlot);
        if (benchEntrySlots.Contains(draggedSlot))
            benchEntrySlots.Remove(draggedSlot);

        if (dropTarget == BattleParent)
            battleEntrySlots.Add(draggedSlot);
        else if (dropTarget == BenchParent)
            benchEntrySlots.Add(draggedSlot);

        // 위치 보정
        draggedSlot.transform.SetAsLastSibling();
    }

    // 드래그 중 포인터가 어느 부모 위에 있는지 판단
    public Transform GetDropTarget(Vector2 pointerPosition)
    {
        RectTransform battleRect = BattleParent.GetComponent<RectTransform>();
        RectTransform benchRect = BenchParent.GetComponent<RectTransform>();

        if (RectTransformUtility.RectangleContainsScreenPoint(battleRect, pointerPosition))
            return BattleParent;
        if (RectTransformUtility.RectangleContainsScreenPoint(benchRect, pointerPosition))
            return BenchParent;

        return null;
    }
    //드래그 중 위치로 인덱스 계산
    public int GetInsertIndex(Transform parent, Vector2 pointerPos)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            RectTransform child = parent.GetChild(i).GetComponent<RectTransform>();
            if (child == null) continue;

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parent.GetComponent<RectTransform>(),
                pointerPos,
                null,
                out localPoint
            );

            if (localPoint.y > child.anchoredPosition.y) // 위에 있을 경우
            {
                return i;
            }
        }

        return parent.childCount;
    }
}

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EntryUIManager : Singleton<EntryUIManager>
{
    [SerializeField] private Transform BattleParent;
    [SerializeField] private Transform BenchParent;
    [SerializeField] private GameObject EntrySlotPrefab;
    [SerializeField] private EntryUI entryUI;
    [SerializeField] private Canvas canvas;

    private List<EntrySlotUI> battleEntrySlots = new();
    private List<EntrySlotUI> benchEntrySlots = new();

    private EntrySlotUI selectedSlot;
    private GameObject placeholder; // 드래그 중 예상 위치
    private Transform startDragPernt; // 드래그 시작 부모
    private EntrySlotUI draggedSlot;
    private EntrySlotUI moveTarget;//엔트리 이동되는 슬롯

    private Transform lastPlaceholderParent; // 이전 플레이스홀더 부모 기록

    private const int BATTLE_SLOT_COUNT = 3;
    private const int BENCH_SLOT_COUNT = 2;
    private bool movedVisual = false;

    private Player p = PlayerManager.Instance.player;

    public void SetEntryUISlots()
    {
        ClearAllSlots();
        SetSlots();
        if (battleEntrySlots.Count > 0)
        {
            SelectSlot(battleEntrySlots[0]);
        }
    }

    private void SetSlots()
    {

        MakeSlotsForParent(BattleParent, p.battleEntry, battleEntrySlots, BATTLE_SLOT_COUNT);
        MakeSlotsForParent(BenchParent, p.benchEntry, benchEntrySlots, BENCH_SLOT_COUNT);
    }

    private void MakeSlotsForParent(Transform parent, List<Monster> entryList, List<EntrySlotUI> slotUIList, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Monster mon = i < entryList.Count ? entryList[i] : null;
            MakeSlot(parent, mon, slotUIList);
        }
    }

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

    private void ClearAllSlots()
    {
        for (int i = BattleParent.childCount - 1; i >= 0; i--)
            Destroy(BattleParent.GetChild(i).gameObject);

        for (int i = BenchParent.childCount - 1; i >= 0; i--)
            Destroy(BenchParent.GetChild(i).gameObject);

        battleEntrySlots.Clear();
        benchEntrySlots.Clear();
        selectedSlot = null;
    }

    public void SelectSlot(EntrySlotUI slot)
    {
        if (slot == selectedSlot) return;

        if (selectedSlot != null)
            selectedSlot.SetSelected(false);

        selectedSlot = slot;
        selectedSlot.SetSelected(true);

        entryUI.SetMonsterDetailUI(selectedSlot.GetMonster());
    }

    public void StartDrag(EntrySlotUI slot)
    {
        draggedSlot = slot;
        startDragPernt = slot.transform.parent;
        slot.SetDragVisual(true);
        slot.transform.SetParent(canvas.transform);
        slot.transform.SetAsLastSibling();

        CreatePlaceholder(startDragPernt, slot.transform.GetSiblingIndex());
        lastPlaceholderParent = startDragPernt;
    }

    public void UpdateDrag(Vector2 screenPos)
    {
        if (draggedSlot != null)
            draggedSlot.transform.position = screenPos;

        UpdatePlaceholderPosition(screenPos);
        BalanceEntry();
    }

    public void EndDrag(EntrySlotUI slot, Vector2 screenPos)
    {
        var dropTarget = GetDropTarget(screenPos);

        // 최종 위치 확정
        slot.transform.SetParent(dropTarget);
        slot.transform.SetSiblingIndex(GetInsertIndex(dropTarget, screenPos));

        if (dropTarget != startDragPernt && p.benchEntry.Count > 0)
        {
            if (startDragPernt == BenchParent)
            {
                p.SwapBattleToBench(moveTarget.GetMonster(), slot.GetMonster());
            }
            if (startDragPernt == BattleParent)
            {
                p.SwapBattleToBench(slot.GetMonster(), moveTarget.GetMonster());
            }
        }
        ClearPlaceholder();
        slot.SetDragVisual(false);
        draggedSlot = null;
        startDragPernt = null;
        lastPlaceholderParent = null;
    }

    private void BalanceEntry()
    {

        if (BattleParent.childCount > 3)
        {
            moveTarget = GetLastNonPlaceholder();
            moveTarget.transform.SetParent(BenchParent);
            moveTarget.transform.SetSiblingIndex(0);
        }
        else if (BenchParent.childCount > 2)
        {
            moveTarget = GetFirstNonPlaceholder();
            moveTarget.transform.SetParent(BattleParent);
            moveTarget.transform.SetAsLastSibling();
        }

    }

    //플레이스 홀더,드랍슬롯 제외 배틀엔트리에 최하단 객체
    private EntrySlotUI GetLastNonPlaceholder()
    {
        for (int i = BattleParent.childCount - 1; i >= 0; i--)
        {
            var child = BattleParent.GetChild(i);
            if (child != placeholder && child != draggedSlot?.transform)
            {
                return child.GetComponent<EntrySlotUI>();
            }
        }
        Debug.Log("배틀에서 옮길게없슴");
        return null;
    }

    //플레이스 홀더,드랍슬롯 제외 배틀엔트리에 최하단 객체
    private EntrySlotUI GetFirstNonPlaceholder()
    {
        for (int i = 0; i < BenchParent.childCount; i++)
        {
            var child = BenchParent.GetChild(i);
            if (child != placeholder && child != draggedSlot?.transform && child.GetComponent<Image>().raycastTarget)
            {
                return child.GetComponent<EntrySlotUI>();
            }
        }
        Debug.Log("벤치에서 옮길게없슴");
        return null;
    }



    //플레이스 홀더 만들기
    private void CreatePlaceholder(Transform parent, int siblingIndex)
    {
        if (placeholder != null)
            Destroy(placeholder);

        placeholder = Instantiate(EntrySlotPrefab, parent);
        placeholder.transform.SetSiblingIndex(siblingIndex);

        EntrySlotUI slotUI = placeholder.GetComponent<EntrySlotUI>();
        if (slotUI != null)
            slotUI.VoidSlotInit();
        slotUI.SetDragVisual(true);
    }

    //플레이스 홀더 위치 갱신
    private void UpdatePlaceholderPosition(Vector2 pointerPos)
    {
        if (placeholder == null) return;

        Transform dropTarget = GetDropTarget(pointerPos);
        if (dropTarget == null) return;


        int index = GetInsertIndex(dropTarget, pointerPos);
        placeholder.transform.SetParent(dropTarget);
        placeholder.transform.SetSiblingIndex(index);
    }
    //플레이스 홀더 제거
    private void ClearPlaceholder()
    {
        if (placeholder != null)
        {
            Destroy(placeholder);
            placeholder = null;
        }
    }
    //인덱스 확인
    private int GetInsertIndex(Transform parent, Vector2 pointerScreenPos)
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

            if (pointerScreenPos.y > centerY)
                return i;
        }
        return parent.childCount;
    }

    //부모 판단
    private Transform GetDropTarget(Vector2 pointerPosition)
    {
        if (p.benchEntry.Count == 0)
        {
            return BattleParent;
        }
        else
        {
            if (IsPointerInYRange(BattleParent, pointerPosition))
                return BattleParent;

            if (IsPointerInYRange(BenchParent, pointerPosition))
                return BenchParent;
        }
        return startDragPernt;
    }

    //객체의 y안에 포인터가 있는지 확인
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


}

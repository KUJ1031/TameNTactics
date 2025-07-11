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


    //엔트리 유아이 세팅
    public void SetEntryUISlots()
    {
        SetSlots();
    }

    //슬롯 세팅
    private void SetSlots()
    {
        Player p = PlayerManager.Instance.player;
        for (int i = 0; i < 3; i++)
        {
            Monster monster = i < p.battleEntry.Count ? p.battleEntry[i] : null;
            MakeSlot(BattleParent, monster, battleEntrySlots);
        }
        for (int i = 0; i < 2; i++)
        {
            Monster monster = i < p.benchEntry.Count ? p.benchEntry[i] : null;
            MakeSlot(BenchParent, monster, benchEntrySlots);
        }

    }

    //슬롯 만들기
    private void MakeSlot(Transform parent, Monster monster, List<EntrySlotUI> slotList)
    {
        if (parent == null) { return; }

        GameObject go = Instantiate(EntrySlotPrefab, parent);
        var slot = go.GetComponent<EntrySlotUI>();
        if (monster != null)
        {
            slot.Init(monster);
        }
        else
        {
            slot.VoidSlotInit();
        }
        slotList.Add(slot);
    }

    public void SelectSlot(EntrySlotUI slot)
    {
        selectedSlot = slot;
    }

    //몬스터 슬롯 선택
    public void SelectMonsterSlot(EntrySlotUI slot)
    {
        if(slot == selectedSlot) { return; }
        //이전 선택 해제
        if (selectedSlot != null)
            selectedSlot.SetSelected(false);

        //새 선택 적용
        selectedSlot = slot;
        selectedSlot.SetSelected(true);
    }

}

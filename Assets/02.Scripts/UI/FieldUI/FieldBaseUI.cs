using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FieldBaseUI : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    [SerializeField] private GameObject entrySlotPrefab; // 추가될 EntrySlot 프리팹
    [SerializeField] private Transform entrySlotParent; // EntrySlot을 배치할 부모 오브젝트

    private List<EntrySlot> entrySlots = new(); // EntrySlot 리스트

    void Start()
    {
        menuButton.onClick.AddListener(() => SetFieldBaseUI());
        RefreshEntrySlots();
    }

    public void SetFieldBaseUI() 
    {
        FieldUIManager.Instance.OpenUI<PlayerInfoUI>();
    }


    /// <summary>
    /// Entry 슬롯을 갱신합니다.
    /// </summary>
    public void RefreshEntrySlots()
    {
        var player = PlayerManager.Instance.player;

        var sorted = player.entryMonsters
            .OrderByDescending(mon => player.battleEntry.Any(b => b.monsterID == mon.monsterID))  // 배틀 출전 우선
            .ThenByDescending(mon => mon.IsFavorite)                                               // 즐겨찾기 우선
            .ThenBy(mon => mon.monsterName)                                                        // 이름 오름차순
            .ToList();

        EnsureSlotCount(sorted.Count);

        for (int i = 0; i < sorted.Count; i++)
        {
            entrySlots[i].SetMonster(sorted[i]);
        }
    }

    /// <summary>
    /// entryMonsters의 개수에 맞춰 EntrySlot을 생성하거나 제거합니다.
    /// </summary>
    private void EnsureSlotCount(int count)
    {
        // 슬롯 부족하면 생성
        while (entrySlots.Count < count)
        {
            GameObject go = Instantiate(entrySlotPrefab, entrySlotParent);
            var slot = go.GetComponent<EntrySlot>();
            entrySlots.Add(slot);
        }

        // 슬롯 많으면 제거
        while (entrySlots.Count > count)
        {
            var last = entrySlots[entrySlots.Count - 1];
            entrySlots.RemoveAt(entrySlots.Count - 1);
            Destroy(last.gameObject);
        }
    }

}

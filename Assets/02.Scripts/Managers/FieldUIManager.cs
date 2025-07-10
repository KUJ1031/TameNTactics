using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum PopupType
{
    EntrySwap,  //엔트리 교체
    Confirm,    //예/아니오
    Warning     //예
}

public class FieldUIManager : MonoBehaviour
{
    public static FieldUIManager Instance { get; private set; }

    [SerializeField] private FieldMenuBaseUI[] uiList;
    [SerializeField] private GameObject LeftMenuUI;
    [SerializeField] private GameObject BaseUI;

    //[SerializeField] private GameObject swapPopupPrefab;
    //[SerializeField] private GameObject confirmPopupPrefab;
    //[SerializeField] private Transform uiCanvas;

    [SerializeField] private GameObject entrySlotPrefab; // 추가될 EntrySlot 프리팹
    [SerializeField] private Transform entrySlotParent; // EntrySlot을 배치할 부모 오브젝트

    private List<EntrySlot> entrySlots = new(); // EntrySlot 리스트

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        RefreshEntrySlots();
    }
    //메뉴열기
    public void OpenUI<T>() where T : FieldMenuBaseUI
    {
        BaseUI.SetActive(false);
        LeftMenuUI.SetActive(true);
        foreach (FieldMenuBaseUI ui in uiList)
        {
            if (ui is T) ui.Open();
            else ui.Close();
        }
    }
    //메뉴닫기
    public void CloseAllUI()
    {
        BaseUI.SetActive(true);
        LeftMenuUI.SetActive(false);
        foreach (var ui in uiList)
        {
            ui.Close();
        }
    }

    //Confirm 팝업
    public void OpenConfirmPopup(PopupType type, string message, Action<bool> onConfirmed)
    {
        PopupUIManager.Instance.ShowPanel<ConfirmPopup>("ConfirmPopup", popup =>
        {
            popup.Open(type,message, onConfirmed);
        });
    }

    //EntrySwap 팝업
    public void OpenEntrySwapPopup(Action<Monster> onSwapped)
    {
        PopupUIManager.Instance.ShowPanel<PlayerEntrySwapPopup>("PlayerEntrySwapPopup", popup =>
        {
            popup.Open(onSwapped);
        });
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

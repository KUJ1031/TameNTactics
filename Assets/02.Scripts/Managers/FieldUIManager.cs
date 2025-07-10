using System.Collections.Generic;
using UnityEngine;

public class FieldUIManager : MonoBehaviour
{
    public static FieldUIManager Instance { get; private set; }

    [SerializeField] private FieldMenuBaseUI[] uiList;
    [SerializeField] private GameObject LeftMenuUI;
    [SerializeField] private GameObject BaseUI;

    [SerializeField] private GameObject swapPopupPrefab;
    [SerializeField] private Transform uiCanvas;

    [SerializeField] private GameObject entrySlotPrefab; // 추가될 EntrySlot 프리팹
    [SerializeField] private Transform entrySlotParent; // EntrySlot을 배치할 부모 오브젝트

    private List<EntrySlot> entrySlots = new(); // EntrySlot 리스트

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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

    //팝업만들어 띄우기
    public void SwapEntryMonster(System.Action<Monster> onSwapped)
    {
        GameObject go = Instantiate(swapPopupPrefab, uiCanvas);
        var popup = go.GetComponent<PlayerEntrySwapPopup>();
        popup.Open(onSwapped);
    }

    /// <summary>
    /// Entry 슬롯을 갱신합니다.
    /// </summary>
    public void RefreshEntrySlots()
    {
        var entryMonsters = PlayerManager.Instance.player.entryMonsters;

        // 슬롯 수 조절
        EnsureSlotCount(entryMonsters.Count);

        // 슬롯에 몬스터 설정
        for (int i = 0; i < entryMonsters.Count; i++)
        {
            entrySlots[i].SetMonster(entryMonsters[i]);
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntrySwapPopup : MonoBehaviour
{
    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject monsterSlotPrefab;
    [SerializeField] private Button okButton;
    [SerializeField] private Button closeButton;

    private List<PlayerEntrySwapPopupSlot> slotList = new();
    private Action<Monster> onSwapped;
    private Monster selectedMonster;

    public void Open(Action<Monster> onSwappedCallback)
    {
        this.onSwapped = onSwappedCallback;
        RefreshSlotList();
        gameObject.SetActive(true);
    }

    private void RefreshSlotList()
    {
        // 예시: 플레이어 몬스터 목록 가져오기
        List<Monster> monsters = PlayerManager.Instance.player.ownedMonsters;

        foreach (var mon in monsters)
        {
            GameObject go = Instantiate(monsterSlotPrefab, slotContainer);
            var slot = go.GetComponent<PlayerEntrySwapPopupSlot>();
            slot.Setup(mon);
            slot.OnClick = () =>
            {
                SelectSlot(slot);
            };
            slotList.Add(slot);
        }
    }

    private void SelectSlot(PlayerEntrySwapPopupSlot slot)
    {
        // 아웃라인 관리
        foreach (var s in slotList)
            s.SetSelected(false);

        slot.SetSelected(true);
        selectedMonster = slot.GetMonster();
    }

    public void OnClickOK()
    {
        onSwapped?.Invoke(selectedMonster);
        Destroy(gameObject);
    }

    public void OnClickCancel()
    {
        onSwapped?.Invoke(null);
        Destroy(gameObject);
    }
}

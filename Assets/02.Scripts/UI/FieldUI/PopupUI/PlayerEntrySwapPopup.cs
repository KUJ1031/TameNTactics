using System;
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

    private void Awake()
    {
        okButton.onClick.AddListener(OnClickOK);
        closeButton.onClick.AddListener(OnClickCancel);
        SetOKButtonUsing(false);
    }
    public void Open(Action<Monster> onSwappedCallback)
    {
        onSwapped = onSwappedCallback;
        RefreshSlotList();
        gameObject.SetActive(true);
    }

    private void RefreshSlotList()
    {
        //플레이어 몬스터 목록 가져오기
        List<Monster> monsters = PlayerManager.Instance.player.entryMonsters;

        foreach (Monster mon in monsters)
        {
            GameObject go = Instantiate(monsterSlotPrefab, slotContainer);
            var slot = go.GetComponent<PlayerEntrySwapPopupSlot>();
            slot.Setup(mon);
            //슬롯에 클릭 이벤트 추가
            slot.OnClick = () =>
            {
                SelectSlot(slot);
            };
            slotList.Add(slot);
        }
    }

    //슬롯 선택
    private void SelectSlot(PlayerEntrySwapPopupSlot slot)
    {
        // 아웃라인 관리
        foreach (var entrySlot in slotList)
            entrySlot.SetSelected(false);

        slot.SetSelected(true);
        selectedMonster = slot.GetMonster();
        SetOKButtonUsing(true);
    }

    //확인버튼
    private void OnClickOK()
    {
        onSwapped?.Invoke(selectedMonster);
        Destroy(gameObject);
    }
    //닫기버튼
    private void OnClickCancel()
    {
        onSwapped?.Invoke(null);
        Destroy(gameObject);
    }

    //확인버튼 활성화
    private void SetOKButtonUsing(bool canClick)
    {
        okButton.interactable = canClick;
    }
}

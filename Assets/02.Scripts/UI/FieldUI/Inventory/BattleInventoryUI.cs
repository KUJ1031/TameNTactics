using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleInventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject battleInventoryUIPanel;
    [SerializeField] private GameObject itemDetailInfoPanel;
    [SerializeField] private Image itemDetailImage;
    [SerializeField] private TextMeshProUGUI itemDetailName;
    [SerializeField] private TextMeshProUGUI itemDetailDescription;
    [SerializeField] private Button useItemButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private Transform battleInventoryParents;
    [SerializeField] private BattleInventorySlotUI battleInventoryslotPrefab;

    public ItemInstance selectedItem; // 선택된 아이템 인스턴스

    // 외부에서 아이템 사용 시 콜백 받는 델리게이트
    public System.Action<ItemInstance> OnItemUseConfirmed;
    public System.Action OnInventoryCancelled;

    private void Start()
    {
        if (!PlayerManager.Instance.player.playerBattleTutorialCheck)
        {
            closeButton.gameObject.SetActive(false); // 배틀 튜토리얼 중에는 닫기 버튼 비활성화
        }

        useItemButton.onClick.AddListener(() => UseItem());
        closeButton.onClick.AddListener(() => HideInventoryUI());
    }

    private void OnEnable()
    {
        // 인벤토리 UI가 활성화될 때마다 인벤토리를 새로 고침
        RefreshInventory();
    }
    public void RefreshInventory()
    {
        PlayerManager.Instance.player.UpdateCategorizedItemLists();
        var playerInventory = PlayerManager.Instance.player.consumableItems;

        // 1. 기존 슬롯 삭제
        foreach (Transform child in battleInventoryParents)
        {
            Destroy(child.gameObject);
        }

        // 2. 새 슬롯 생성
        foreach (var item in playerInventory)
        {
            BattleInventorySlotUI slot = Instantiate(battleInventoryslotPrefab, battleInventoryParents);
            slot.itemIcon.sprite = item.data.itemImage; // Assuming itemIcon is a SpriteRenderer or Image component
            slot.itemQuantity.text = item.quantity.ToString(); // Assuming itemQuantity is a TextMeshProUGUI component

            slot.SlotButton.onClick.AddListener(() => OnItemSlotClicked(item));
        }
    }

    private void OnItemSlotClicked(ItemInstance item)
    {
        Debug.Log($"클릭한 아이템: {item.data.itemName}, 수량: {item.quantity}");

        BattleTutorialManager.Instance.InitItemButtonSelected();

        itemDetailImage.sprite = item.data.itemImage; // Assuming itemIcon is a SpriteRenderer or Image component
        itemDetailName.text = item.data.itemName; // Assuming itemName is a TextMeshProUGUI component
        itemDetailDescription.text = item.data.description; // Assuming itemDescription is a TextMeshProUGUI component

        selectedItem = item; // 선택된 아이템 인스턴스 저장
        if (selectedItem.data.itemEffects != null && item.data.itemEffects.Count > 0)
        {
            Debug.Log($"아이템 회복량: {item.data.itemEffects[0].value}");
        }
        ShowitemDetailInfoPanel();
    }

    public void UseItem()
    {
        if (selectedItem == null) return;

        OnItemUseConfirmed?.Invoke(selectedItem);

        RefreshInventory();
        HideInventory();
        HideitemDetailInfoPanel();
    }

    public void HideInventoryUI()
    {
        HideInventory();
        HideitemDetailInfoPanel();
        OnInventoryCancelled?.Invoke(); // ← 콜백 실행
    }

    public void ShowInventory() => battleInventoryUIPanel.SetActive(true);
    public void HideInventory() => battleInventoryUIPanel.SetActive(false);
    public void ShowitemDetailInfoPanel() => itemDetailInfoPanel.SetActive(true);
    public void HideitemDetailInfoPanel() => itemDetailInfoPanel.SetActive(false);
}

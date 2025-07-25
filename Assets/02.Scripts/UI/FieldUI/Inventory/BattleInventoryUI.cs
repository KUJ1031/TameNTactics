using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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

    private void Start()
    {
        useItemButton.onClick.AddListener(() => UseItem());
        closeButton.onClick.AddListener(() => HideInventory());

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

    public void ShowInventory() => battleInventoryUIPanel.SetActive(true);
    public void HideInventory() => battleInventoryUIPanel.SetActive(false);
    public void ShowitemDetailInfoPanel() => itemDetailInfoPanel.SetActive(true);
    public void HideitemDetailInfoPanel() => itemDetailInfoPanel.SetActive(false);
}

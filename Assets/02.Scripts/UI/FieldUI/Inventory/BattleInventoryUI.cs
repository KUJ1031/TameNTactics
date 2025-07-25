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


    private void Start()
    {
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
        ShowInventory();
    }

    public void ShowInventory()
    {
        battleInventoryUIPanel.SetActive(true);
        // Additional logic to populate the inventory slots can be added here
    }

    public void HideInventory()
    {
        battleInventoryUIPanel.SetActive(false);
        // Additional logic to clear the inventory slots can be added here
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : FieldMenuBaseUI
{
    [SerializeField] private Transform slotParent;         // 슬롯들을 넣을 부모 오브젝트
    [SerializeField] private GameObject slotPrefab;        // 슬롯 프리팹
    [SerializeField] private Button useButton, equipButton, unequipButton, dropButton;
    [SerializeField] private Image ItemImage;
    [SerializeField] private Text ItemInfoText;



    private ItemInstance selectedItem;

    public List<ItemInstance> items;

    private void Start()
    {
        useButton.onClick.AddListener(OnUse);
        equipButton.onClick.AddListener(OnEquip);
        unequipButton.onClick.AddListener(OnUnEquip);
        dropButton.onClick.AddListener(OnDrop);

        Refresh();
    }

    public void Refresh()
    {
        // 기존 슬롯 제거
        for (int i = 0; i < slotParent.childCount; i++)
        {
            GameObject child = slotParent.GetChild(i).gameObject;
            Destroy(child);
        }

        // 아이템 슬롯 생성
        for (int i = 0; i < items.Count; i++)
        {
            ItemInstance item = items[i];
            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            InventorySlotUI slotUI = slotObj.GetComponent<InventorySlotUI>();
            slotUI.Init(item, this);
        }

        selectedItem = null;
        UpdateButtons();
    }

    public void SelectItem(ItemInstance item)
    {
        selectedItem = item;
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        if (selectedItem == null)
        {
            useButton.interactable = false;
            equipButton.interactable = false;
            unequipButton.interactable = false;
            dropButton.interactable = false;
            return;
        }

        bool isConsumable = selectedItem.data.type == ItemType.consumable;
        bool isEquipment = selectedItem.data.type == ItemType.equipment;

        useButton.interactable = isConsumable;
        equipButton.interactable = isEquipment && !selectedItem.isEquipped;
        unequipButton.interactable = isEquipment && selectedItem.isEquipped;
        dropButton.interactable = true;
    }

    private void OnUse()
    {
        if (selectedItem == null) return;

        for (int i = 0; i < selectedItem.data.itemEffects.Count; i++)
        {
            ItemEffect effect = selectedItem.data.itemEffects[i];
            Debug.Log($"[사용] {effect.type} +{effect.value}");
        }

        selectedItem.quantity -= 1;
        if (selectedItem.quantity <= 0)
        {
            items.Remove(selectedItem);
        }

        Refresh();
    }

    private void OnEquip()
    {
        if (selectedItem == null) return;

        selectedItem.isEquipped = true;
        Debug.Log($"{selectedItem.data.itemName} 장착됨");

        Refresh();
    }

    private void OnUnEquip()
    {
        if (selectedItem == null) return;

        selectedItem.isEquipped = false;
        Debug.Log($"{selectedItem.data.itemName} 장착 해제됨");

        Refresh();
    }

    private void OnDrop()
    {
        if (selectedItem == null) return;

        items.Remove(selectedItem);
        Debug.Log($"{selectedItem.data.itemName} 버림");

        Refresh();
    }
}

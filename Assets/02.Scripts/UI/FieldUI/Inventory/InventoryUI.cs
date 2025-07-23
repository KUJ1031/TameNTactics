using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class InventoryUI : FieldMenuBaseUI
{
    [SerializeField] private Transform slotParent;         // 슬롯들을 넣을 부모 오브젝트
    [SerializeField] private GameObject slotPrefab;        // 슬롯 프리팹
    [SerializeField] private Button useButton, equipButton, unequipButton, dropButton;
    [SerializeField] private Image ItemImage;
    [SerializeField] private TextMeshProUGUI ItemInfoText;



    private ItemInstance selectedItem;

    public List<ItemInstance> items;


    [SerializeField] private Button consumableButton;
    [SerializeField] private Button equipableButton;
    [SerializeField] private Button gestureButton;

    private ItemType? currentFilter = null;

    [SerializeField] private ItemSelectPopup itemSelectPopup; // 로고 오브젝트

    private void Start()
    {
        useButton.onClick.AddListener(OnUse);
        equipButton.onClick.AddListener(OnEquip);
        unequipButton.onClick.AddListener(OnUnEquip);
        dropButton.onClick.AddListener(OnDrop);

        consumableButton.onClick.AddListener(() => ApplyFilter(ItemType.consumable));
        equipableButton.onClick.AddListener(() => ApplyFilter(ItemType.equipment));
        gestureButton.onClick.AddListener(() => ApplyFilter(ItemType.gesture));

        Refresh();
    }

    private void OnEnable()
    {
        items = PlayerManager.Instance.player.items;
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
        foreach (var item in items)
        {
            if (currentFilter.HasValue && item.data.type != currentFilter.Value)
                continue;

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

        if (selectedItem.data.itemEffects.Exists(e => e.type == ItemEffectType.curHp))
        {
            // 대상 몬스터 선택 창 띄우기
            itemSelectPopup.gameObject.SetActive(true);
            itemSelectPopup.Open(selectedItem);
            return;
        }

        // 즉시 효과 처리용 (예: 골드 증가 등)
        foreach (var effect in selectedItem.data.itemEffects)
        {
            Debug.Log($"[사용] {effect.type} +{effect.value}");
        }

        selectedItem.quantity--;
        if (selectedItem.quantity <= 0)
            items.Remove(selectedItem);

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

    public void DisplayItemInfo(ItemInstance item)
    {
        if (item == null) return;

        ItemImage.sprite = item.data.itemImage;
        ItemInfoText.text = $"{item.data.itemName}\n{item.data.description}\n가격: {item.data.goldValue} G";
    }

    private void ApplyFilter(ItemType type)
    {
        currentFilter = type;
        Refresh(); // 필터 적용 후 다시 렌더링
    }

}

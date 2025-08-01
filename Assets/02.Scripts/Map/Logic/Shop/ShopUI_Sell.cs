using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI_Sell : MonoBehaviour
{
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Button sellButton;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemInfoText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private InventoryUI inventoryUI;

    [SerializeField] private Button consumableButton;
    [SerializeField] private Button equipableButton;
    [SerializeField] private Button gestureButton;

    private ItemInstance selectedItem;

    [SerializeField] private GameObject warringPopup;
    [SerializeField] private TextMeshProUGUI warringPopupText;

    [SerializeField] private Button warringPopupExitButton;

    private enum FilterType { All, Consumable, Equipable, Gesture }
    private FilterType currentFilter = FilterType.All;

    private void Start()
    {
        sellButton.onClick.AddListener(OnSell);

        consumableButton.onClick.AddListener(() => { currentFilter = FilterType.Consumable; Refresh(); });
        equipableButton.onClick.AddListener(() => { currentFilter = FilterType.Equipable; Refresh(); });
        gestureButton.onClick.AddListener(() => { currentFilter = FilterType.Gesture; Refresh(); });

        warringPopupExitButton.onClick.AddListener(() => warringPopup.SetActive(false));

        Refresh(); // 인벤토리에서 불러옴
    }

    private void OnEnable()
    {
        currentFilter = FilterType.Consumable;
        Refresh();
    }

    public void Refresh()
    {
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        List<ItemInstance> playerItems = PlayerManager.Instance.player.items;

        foreach (var item in playerItems)
        {
            if (!PassesFilter(item.data)) continue;

            var obj = Instantiate(slotPrefab, slotParent);
            var slot = obj.GetComponent<ShopItemSlot>();
            slot.Init(item, this); // 판매용 슬롯 초기화
        }

        selectedItem = null;
        UpdateButtons();
        UpdateGoldUI();
    }

    private bool PassesFilter(ItemData data)
    {
        switch (currentFilter)
        {
            case FilterType.Consumable:
                return data.type == ItemType.consumable;
            case FilterType.Equipable:
                return data.type == ItemType.equipment;
            case FilterType.Gesture:
                return data.type == ItemType.gesture;
            case FilterType.All:
            default:
                return true;
        }
    }

    public void SelectItem(ItemInstance item)
    {
        selectedItem = item;
        itemImage.sprite = item.data.itemImage;
        int sellValue = GetSellValue(item.data);
        itemInfoText.text = $"{item.data.itemName}\n{item.data.description}\n판매가: {sellValue} G";
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        sellButton.interactable = selectedItem != null;
    }

    private void UpdateGoldUI()
    {
        goldText.text = $"소지금: {PlayerManager.Instance.player.gold} G";
    }

    private void OnSell()
    {
        var equipment = PlayerManager.Instance.player.playerEquipment;

        if (selectedItem == null) return;
        if (equipment.Count > 0 && equipment[0] != null)
        {
            if (selectedItem.data.itemId == equipment[0].data.itemId)
            {
                warringPopup.SetActive(true);
                warringPopupText.text = "장착 중인 아이템은 판매할 수 없습니다.";
                return;
            }

        }

        var player = PlayerManager.Instance.player;
        int sellValue = GetSellValue(selectedItem.data);
        player.gold += sellValue;

        if (selectedItem.quantity >= 0)
        {
            player.RemoveItem(selectedItem, 1);
        }

        Debug.Log($"[판매] {selectedItem.data.itemName} → {sellValue}G");

        UpdateGoldUI();
        inventoryUI.items = player.items;
        inventoryUI.Refresh();
        Refresh();
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }

    private int GetSellValue(ItemData data)
    {
        return Mathf.FloorToInt(data.goldValue * 0.7f);
    }
}

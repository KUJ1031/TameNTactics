using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Button buyButton;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemInfoText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private InventoryUI inventoryUI; // 인벤토리 UI 참조

    [SerializeField] private Button consumableButton;
    [SerializeField] private Button equipableButton;
    [SerializeField] private Button gestureButton;

    public List<ItemData> ShopItems_Consumable;  // 에디터에서 등록하는 아이템 데이터 목록
    public List<ItemData> ShopItems_Equipable;  // 에디터에서 등록하는 아이템 데이터 목록
    public List<ItemData> ShopItems_Gesture;  // 에디터에서 등록하는 아이템 데이터 목록

    private List<ItemInstance> shopItems = new(); // 실제 상점에 진열할 아이템 인스턴스 리스트
    private ItemInstance selectedItem;

    private void Start()
    {
        buyButton.onClick.AddListener(OnBuy);
        consumableButton.onClick.AddListener(() => LoadCategory(ShopItems_Consumable));
        equipableButton.onClick.AddListener(() => LoadCategory(ShopItems_Equipable));
        gestureButton.onClick.AddListener(() => LoadCategory(ShopItems_Gesture));

        // 기본 카테고리 불러오기
        LoadCategory(ShopItems_Consumable);
    }

    private void LoadCategory(List<ItemData> categoryItems)
    {
        shopItems.Clear();

        foreach (var data in categoryItems)
        {
            shopItems.Add(new ItemInstance(data, 1)); // 기본 수량 1
        }

        Refresh();
    }

    public void Refresh()
    {
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        foreach (var item in shopItems)
        {
            var obj = Instantiate(slotPrefab, slotParent);
            var slot = obj.GetComponent<ShopItemSlot>();
            slot.Init(item, this); // this: ShopUI
        }

        selectedItem = null;
        UpdateButtons();
        UpdateGoldUI();
    }

    public void SelectItem(ItemInstance item)
    {
        selectedItem = item;
        itemImage.sprite = item.data.itemImage;
        itemInfoText.text = $"{item.data.itemName}\n{item.data.description}\n가격: {item.data.goldValue}G";

        UpdateButtons();
    }

    private void UpdateButtons()
    {
        buyButton.interactable = selectedItem != null;
    }

    private void UpdateGoldUI()
    {
        goldText.text = $"소지금: {PlayerManager.Instance.player.gold} G";
    }

    private void OnBuy()
    {
        if (selectedItem == null) return;

        var player = PlayerManager.Instance.player;

        if (player.gold >= selectedItem.data.goldValue)
        {
            player.gold -= selectedItem.data.goldValue;
            // 기존에 같은 itemName 가진 아이템 찾기
            var existing = player.items.Find(i => i.data.itemName == selectedItem.data.itemName);

            if (existing != null)
            {
                // 이미 있으면 수량만 증가
                existing.quantity += 1;
            }
            else
            {
                // 없으면 새 인스턴스 추가
                var newInstance = new ItemInstance(selectedItem.data, 1);
                player.items.Add(newInstance);
            }

            Debug.Log($"[구매] {selectedItem.data.itemName}");

            UpdateGoldUI();
            inventoryUI.items = player.items;
            inventoryUI.Refresh();
        }
        else
        {
            Debug.LogWarning($"[구매 실패] 금액이 부족합니다: {selectedItem.data.itemName}");
        }
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}

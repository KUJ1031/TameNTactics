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
    [SerializeField] private TextMeshProUGUI buyItemText;
    private Coroutine hideBuyTextCoroutine;

    [SerializeField] private Button consumableButton;
    [SerializeField] private Button equipableButton;
    [SerializeField] private Button gestureButton;

    public List<ItemData> ShopItems_Consumable;  // 에디터에서 등록하는 아이템 데이터 목록
    public List<ItemData> ShopItems_Equipable;  // 에디터에서 등록하는 아이템 데이터 목록
    public List<ItemData> ShopItems_Gesture;  // 에디터에서 등록하는 아이템 데이터 목록

    private List<ItemInstance> shopItems = new(); // 실제 상점에 진열할 아이템 인스턴스 리스트
    private ItemInstance selectedItem;

    [SerializeField] private GameObject warringPopup;
    [SerializeField] private TextMeshProUGUI warringPopupText;

    [SerializeField] private Button warringPopupExitButton;

    private void Start()
    {
        buyButton.onClick.AddListener(OnBuy);
        consumableButton.onClick.AddListener(() => LoadCategory(ShopItems_Consumable));
        equipableButton.onClick.AddListener(() => LoadCategory(ShopItems_Equipable));
        gestureButton.onClick.AddListener(() => LoadCategory(ShopItems_Gesture));

        warringPopupExitButton.onClick.AddListener(() => warringPopup.SetActive(false));
        // 기본 카테고리 불러오기
        LoadCategory(ShopItems_Consumable);
    }

    private void OnEnable()
    {
        LoadCategory(ShopItems_Consumable); // 항상 소모품 탭으로 초기화
    }

    private void LoadCategory(List<ItemData> categoryItems)
    {
        shopItems.Clear();

        var playerItems = PlayerManager.Instance.player.items;

        foreach (var data in categoryItems)
        {
            // 장비/제스처 아이템 중 이미 플레이어가 보유한 경우는 건너뜀
            bool alreadyOwned = playerItems.Exists(i =>
                i.data.itemId == data.itemId &&
                (data.type == ItemType.equipment || data.type == ItemType.gesture)
            );

            if (alreadyOwned)
                continue;

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
            // 이미 장비/제스처 아이템을 소지 중인지 체크
            var existing = player.items.Find(i => i.data.itemName == selectedItem.data.itemName);

            if (existing != null)
            {
                if (selectedItem.data.type == ItemType.equipment)
                {
                    warringPopup.SetActive(true);
                    warringPopupText.text = $"{selectedItem.data.itemName}은(는) 이미 소지하고 있습니다.\n 장비 아이템은 1개만 소지할 수 있습니다.";
                    return;
                }
                if (selectedItem.data.type == ItemType.gesture)
                {
                    warringPopup.SetActive(true);
                    warringPopupText.text = $"{selectedItem.data.itemName}은(는) 이미 소지하고 있습니다.\n 제스처 아이템은 1개만 소지할 수 있습니다.";
                    return;
                }
            }

            // 골드 차감 및 아이템 추가
            player.gold -= selectedItem.data.goldValue;
            player.AddItem(selectedItem.data, 1);
            ShowBuyMessage(selectedItem.data.itemName);

            Debug.Log($"[구매] {selectedItem.data.itemName}");

            // 장비 또는 제스처 아이템이면 상점에서 제거
            if (selectedItem.data.type == ItemType.equipment || selectedItem.data.type == ItemType.gesture)
            {
                shopItems.Remove(selectedItem);
                selectedItem = null;
                Refresh(); // 상점 UI 갱신
            }
            else
            {
                UpdateButtons();
            }

            UpdateGoldUI();
            inventoryUI.items = player.items;
            inventoryUI.Refresh();
        }
        else
        {
            warringPopup.SetActive(true);
            warringPopupText.text = $"골드가 부족합니다.\n 현재 골드 : {player.gold}";
        }
    }

    private void ShowBuyMessage(string itemName)
    {
        if (hideBuyTextCoroutine != null)
            StopCoroutine(hideBuyTextCoroutine);

        buyItemText.text = $"[{itemName}] 아이템을 구매하였습니다.";
        buyItemText.gameObject.SetActive(true);

        hideBuyTextCoroutine = StartCoroutine(HideBuyMessageAfterDelay(3f));
    }

    private System.Collections.IEnumerator HideBuyMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        buyItemText.gameObject.SetActive(false);
    }


    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}

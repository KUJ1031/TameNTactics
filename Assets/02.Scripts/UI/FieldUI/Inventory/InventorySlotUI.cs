using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI ItemName;
    [SerializeField] private TextMeshProUGUI quantityText;

    private ItemInstance item;
    private InventoryUI inventoryUI;
    private ShopUI shopUI;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    public void Init(ItemInstance item, InventoryUI inventoryUI)
    {
        this.item = item;
        this.inventoryUI = inventoryUI;
        this.shopUI = null;

        icon.sprite = item.data.itemImage;
        ItemName.text = item.data.itemName;
        quantityText.text = item.quantity > 1 ? item.quantity.ToString() : "";

        GetComponent<Button>().onClick.AddListener(() => inventoryUI.SelectItem(item));
    }

    public void Init(ItemInstance item, ShopUI shopUI)
    {
        this.item = item;
        this.shopUI = shopUI;
        this.inventoryUI = null;

        icon.sprite = item.data.itemImage;
        ItemName.text = item.data.itemName;
        quantityText.text = item.quantity > 1 ? item.quantity.ToString() : "";

        GetComponent<Button>().onClick.AddListener(() => shopUI.SelectItem(item));
    }

    private void OnClick()
    {
        if (inventoryUI != null)
        {
            // InventoryUI에 아이템 정보 띄우기 함수 호출
            inventoryUI.DisplayItemInfo(item);
            inventoryUI.SelectItem(item); // 선택 처리 (원한다면)
        }
        else if (shopUI != null)
        {
            shopUI.SelectItem(item);
        }
    }
}

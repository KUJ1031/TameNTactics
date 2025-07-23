using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text ExplainText;
    [SerializeField] private TMP_Text GoldText;
    [SerializeField] private Button selectButton;

    private ItemInstance currentItem;
    private ShopUI shopUI;
    private ShopUI_Sell shopUI_Sell;

    public void Init(ItemInstance item, ShopUI shop)
    {
        currentItem = item;
        shopUI = shop;

        itemImage.sprite = item.data.itemImage;
        NameText.text = item.data.itemName;
        ExplainText.text = item.data.description;
        GoldText.text = $"{item.data.goldValue} G";

        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(OnClick);
    }

    public void Init(ItemInstance item, ShopUI_Sell shop)
    {
        currentItem = item;
        shopUI_Sell = shop;

        itemImage.sprite = item.data.itemImage;
        NameText.text = item.data.itemName;
        ExplainText.text = item.data.description;
        GoldText.text = $"{item.data.goldValue} G";

        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        shopUI?.SelectItem(currentItem);
        shopUI_Sell?.SelectItem(currentItem);
    }
}
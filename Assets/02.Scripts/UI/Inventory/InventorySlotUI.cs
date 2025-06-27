using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Image equippedMark; // 장착 상태 표시용 (선택 사항)

    private ItemInstance item;
    private InventoryUI ui;

    public void Init(ItemInstance item, InventoryUI ui)
    {
        this.item = item;
        this.ui = ui;

        icon.sprite = item.data.itemImage;
        nameText.text = item.data.itemName;

        countText.text = item.data.maxStack > 1 ? $"x{item.quantity}" : "";

        if (equippedMark != null)
            equippedMark.enabled = item.isEquipped;

        GetComponent<Button>().onClick.AddListener(() =>
        {
            ui.SelectItem(item);
        });
    }
}

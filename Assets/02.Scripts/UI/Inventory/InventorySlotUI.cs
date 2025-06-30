using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;            //아이콘
    [SerializeField] private TMP_Text nameText;     //이름
    [SerializeField] private TMP_Text countText;    //개수
    [SerializeField] private Image equippedMark;    //장착 상태 표시

    private ItemInstance item;
    private InventoryUI ui;

    public void Init(ItemInstance item, InventoryUI ui)
    {
        this.item = item;
        this.ui = ui;

        icon.sprite = item.data.itemImage;
        nameText.text = item.data.itemName;

        countText.text = item.data.maxStack > 1 ? $"x{item.quantity}" : ""; //1보다 클 경우 개수표시

        if (equippedMark != null)
            equippedMark.enabled = item.isEquipped;

        GetComponent<Button>().onClick.AddListener(() =>
        {
            ui.SelectItem(item);
        });
    }
}

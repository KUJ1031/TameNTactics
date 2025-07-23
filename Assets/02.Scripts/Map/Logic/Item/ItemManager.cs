using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    protected override bool IsDontDestroy => false;

    public List<ItemData> allItems;  // 인스펙터에 아이템 모두 등록
    public InventoryUI inventoryUI;

    internal GameObject item;


    public void AddItemToPlayer(ItemData itemData, int quantity = 1)
    {
        var player = PlayerManager.Instance.player;

        var existing = player.items.Find(i => i.data.itemName == itemData.itemName);

        if (existing != null)
            existing.quantity += quantity;
        else
            player.items.Add(new ItemInstance(itemData, quantity));

        Debug.Log($"[획득] {itemData.itemName} x{quantity}");

        // UI 갱신
        if (inventoryUI != null)
        {
            inventoryUI.items = player.items;
            inventoryUI.Refresh();
        }

        // 필요한 추가 처리 예: 사운드, 알림 등
    }

    public ItemData GetItemByName(string itemName)
    {
        return allItems.Find(i => i.itemName == itemName);
    }
}

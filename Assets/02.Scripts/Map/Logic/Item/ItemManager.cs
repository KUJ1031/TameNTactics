using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    protected override bool IsDontDestroy => true;

    public List<ItemData> allItems;  // 인스펙터에 아이템 모두 등록

    internal GameObject item;

    public ItemData GetItemByName(string itemName)
    {
        return allItems.Find(i => i.itemName == itemName);
    }

    public void GetThisItem(ItemData item, int num)
    {
        PlayerManager.Instance.player.AddItem(item, num);
    }
}

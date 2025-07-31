using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    protected override bool IsDontDestroy => true;

    internal List<ItemData> allItems = new();
    public List<ItemData> consumableItems = new();
    public List<ItemData> equipmentItems = new();
    public List<ItemData> gestureItems = new();

    internal GameObject item;

    protected override void Awake()
    {
        AddUniqueItems(consumableItems);
        AddUniqueItems(equipmentItems);
        AddUniqueItems(gestureItems);

        Debug.Log($"[ItemManager] 아이템 초기화 완료. 총 {allItems.Count}개 등록됨.");
    }


    private void AddUniqueItems(List<ItemData> categoryList)
    {
        foreach (var item in categoryList)
        {
            if (!allItems.Contains(item))
            {
                allItems.Add(item);
            }
        }
    }

    public ItemData GetItemByName(string itemName)
    {
        return allItems.Find(i => i.itemName == itemName);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory
{
    public List<ItemInstance> items = new List<ItemInstance>();

    public void AddItem(ItemData data)
    {
        var existing = items.Find(i => i.data == data && data.maxStack > 1);
        if (existing != null)
            existing.quantity++;
        else
            items.Add(new ItemInstance(data));
    }
}


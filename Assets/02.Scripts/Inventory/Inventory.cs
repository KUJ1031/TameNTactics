using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();

    //아이템추가
    public void AddItem(ItemData item, int count = 1)
    {
        int remaining = count;

        //기존 슬롯에 먼저 넣기
        foreach (var slot in slots)
        {
            if (slot.item == item && slot.quantity < item.maxStack)
            {
                remaining = slot.AddItem(item, remaining);
                if (remaining <= 0) return;
            }
        }

        //남은 수량이 있다면 새 슬롯 생성
        while (remaining > 0)
        {
            InventorySlot newSlot = new InventorySlot();
            remaining = newSlot.AddItem(item, remaining);
            slots.Add(newSlot);
        }
    }

    //아이템 삭제(소모,판매 등)
    public void RemoveItem(ItemData item, int count = 1)
    {
        int remaining = count;

        for (int i = slots.Count - 1; i >= 0; i--)
        {
            InventorySlot slot = slots[i];

            if (slot.item == item)
            {
                int removed = slot.RemoveItem(remaining);
                remaining -= removed;

                if (slot.isEmpty) //비었다면 해당 인덱스 삭제
                {
                    slots.RemoveAt(i);
                }

                if (remaining <= 0) return;
            }
        }
    }

}

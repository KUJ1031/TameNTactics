using UnityEngine;
using static UnityEditor.Progress;

public class InventorySlot : MonoBehaviour
{
    public ItemData item;   //아이템종류
    public int quantity;    //수량

    public bool isEmpty => item == null || quantity <= 0;   //비어있는지? 아이템이 없거나 수량이 0이면 true

    //아이템 더하기(더하고 남은수량 반환)
    public int AddItem(ItemData newItem, int count)
    {
        if (!isEmpty && item != newItem)
            return count;

        if (isEmpty)
        {
            item = newItem;
            int toAdd = Mathf.Min(count, newItem.maxStack);
            quantity = toAdd;
            return count - toAdd;
        }
        else
        {
            int space = item.maxStack - quantity;
            int toAdd = Mathf.Min(count, space);
            quantity += toAdd;
            return count - toAdd;
        }
    }

    //아이템 빼기(빼고 남은수량 반환)
    public int RemoveItem(int count)
    {
        int removed = Mathf.Min(count, quantity);
        quantity -= removed;

        if (quantity <= 0)
        {
            item = null;
            quantity = 0;
        }

        return removed;
    }
}

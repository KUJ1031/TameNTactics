using System.Linq;
using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    public ItemData data;      //원본 아이템 데이터

    public bool isEquipped;    //장착여부
    public int quantity;       //스택 수량

    public ItemInstance(ItemData data, int quantity = 1)
    {
        this.data = data;
        this.quantity = quantity;
        this.isEquipped = false;
    }

    public ItemData GetItemToWantType(ItemType itemType)
    {
        return data.type == itemType ? data : null;
    }
}
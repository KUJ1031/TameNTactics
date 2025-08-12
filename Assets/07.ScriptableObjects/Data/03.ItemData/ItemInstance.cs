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

    // 직렬화용 데이터로 변환
    public ItemInstanceSaveData ToSaveData()
    {
        return new ItemInstanceSaveData
        {
            itemId = data.itemId,
            isEquipped = this.isEquipped,
            quantity = this.quantity
        };
    }

    // 저장된 데이터로부터 복원
    public static ItemInstance FromSaveData(ItemInstanceSaveData saveData, ItemDatabase itemDb)
    {
        var itemData = itemDb.GetByNumber(saveData.itemId);
        if (itemData == null)
        {
            Debug.LogError($"ItemDatabase에 아이템ID {saveData.itemId}가 없습니다.");
            return null;
        }

        var instance = new ItemInstance(itemData, saveData.quantity);
        instance.isEquipped = saveData.isEquipped;
        return instance;
    }
}

[System.Serializable]
public class ItemInstanceSaveData
{
    public int itemId;       // ItemData 식별자
    public bool isEquipped;  // 장착 여부
    public int quantity;     // 수량
}
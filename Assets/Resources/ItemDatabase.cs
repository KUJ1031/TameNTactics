using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DB/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> allItems = new List<ItemData>();

    private static ItemDatabase _instance;
    public static ItemDatabase Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<ItemDatabase>("ItemDatabase");
            return _instance;
        }
    }
    public ItemData GetByNumber(int number)
    {
        return allItems.Find(item => item.itemId == number);
    }
}

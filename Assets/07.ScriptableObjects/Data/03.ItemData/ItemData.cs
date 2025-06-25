using System.Collections.Generic;
using UnityEngine;
public struct ItemEffect
{
    ItemEffectType type;
    int value;
}
public enum ItemEffectType
{
    exp,
    maxHp,
    curHp,
    attack,
    defense,
    speed,
    criticalChance
}
public enum ItemType
{
    equipment,  //장착
    consumable, //소모
    resource    //기타
}

[CreateAssetMenu(fileName = "NewItem", menuName = "New Item/Create New Item")]
public class ItemData : ScriptableObject
{

    [Header("기본 정보")]
    public string itemName;     //아이템 이름
    public Sprite itemImage;    //아이템 이미지
    public ItemType type;       //장착,소모,기타

    [Header("효과")]
    public List<ItemEffect> itemEffects;//아이템 효과
    
}

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public ItemEffectType type;
    public int value;
}
public enum ItemEffectType
{
    exp,
    maxHp,
    curHp,
    attack,
    defense,
    speed,
    criticalChance,
    allMonsterCurHp
}
public enum ItemType
{
    equipment,  //장착
    consumable, //소모
    gesture     //제스쳐
}

[CreateAssetMenu(fileName = "NewItem", menuName = "New Item/Create New Item")]
public class ItemData : ScriptableObject
{

    [Header("기본 정보")]
    public string itemName;     //아이템 이름
    public Sprite itemImage;    //아이템 이미지
    public int maxStack;        //최대 중복 스택
    public ItemType type;       //장착,소모,기타
    public string description;  //아이템 설명
    public int goldValue;       //가격

    [Header("효과")]
    public List<ItemEffect> itemEffects;//아이템 효과
    
}

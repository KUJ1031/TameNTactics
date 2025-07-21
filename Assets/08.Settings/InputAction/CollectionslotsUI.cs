using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionslotsUI : MonoBehaviour
{
    [SerializeField] private Image MonsterImage;

    public MonsterData data;

    private void Awake()
    {
        Setup(data);
    }

    public void Setup(MonsterData Data)
    {
        MonsterImage.sprite = Data.monsterImage;
        if (Data.encounterCount > 0)
        {
            MonsterImage.color = Color.white;
        }
        else
        {
            MonsterImage.color = Color.black;
        }
    }

    public MonsterData GetMonsterData()
    {
        return data;
    }

    public void ONclick()
    {
        CollectionuiManager.Instance.Selectslot(this);
    }
}

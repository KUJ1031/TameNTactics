using UnityEngine;
using UnityEngine.UI;

public class CollectionSlotUI : MonoBehaviour
{
    [SerializeField] private Image MonsterImage;
    [SerializeField] private MonsterData data;

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

    public void OnClick()
    {
        CollectionUIManager.Instance.SelectSlot(this);
    }
}

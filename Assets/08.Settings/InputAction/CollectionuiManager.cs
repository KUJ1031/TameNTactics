using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionuiManager : MonoBehaviour
{
    [SerializeField] private CollectionUI collectionUI;
    

    private CollectionslotsUI collectionslotsUI;
    public static CollectionuiManager Instance;
    public List<MonsterData> monsters;
    private List<CollectionslotsUI> allSlots = new();

    

    private void Awake()
    {
        Instance = this;
    }


    public void Selectslot(CollectionslotsUI slots)
    {
        if(collectionslotsUI == slots)
        {
            return;
        }
        collectionslotsUI = slots;
        collectionUI.Setdata(slots.GetMonsterData());
    }

    private void Start()
    {

    }
}

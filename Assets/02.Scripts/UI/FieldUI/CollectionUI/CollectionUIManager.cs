using UnityEngine;

public class CollectionUIManager : Singleton<CollectionUIManager>
{
    [SerializeField] private CollectionUI collectionUI;
    [SerializeField] private CollectionSlotUI startSlot;
    
    private CollectionSlotUI collectionSlotsUI;

    private void Start()
    {
        SelectSlot(startSlot);
    }

    public void SelectSlot(CollectionSlotUI slots)
    {
        if(collectionSlotsUI == slots)
        {
            return;
        }
        collectionSlotsUI = slots;
        collectionUI.SetData(slots.GetMonsterData());
    }



}

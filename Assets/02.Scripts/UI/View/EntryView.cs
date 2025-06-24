using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryView : MonoBehaviour
{
    public List<EntrySlot> entrySlots;

    public void SetEntries(List<MonsterData> entries)
    {
        for (int i = 0; i < entries.Count; i++)
        {
            if (i < entries.Count)
            {
                entrySlots[i].SetMonster(entries[i]);
            }
            else
            {
                entrySlots[i].ClearSlot();
            }
        }
    }
}

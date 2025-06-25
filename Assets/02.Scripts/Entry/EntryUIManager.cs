using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryUIManager : MonoBehaviour
{
    public GameObject EntryPanel; // UI 패널
    public void SetDisplayEntry()
    {
        if (EntryPanel != null)
        {
            EntryPanel.SetActive(true);
        }
    }

    public void HideDisplayEntry()
    {
        if (EntryPanel != null)
        {
            EntryPanel.SetActive(false);
        }
    }
}

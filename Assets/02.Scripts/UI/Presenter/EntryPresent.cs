using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPresent : MonoBehaviour
{
    [SerializeField] private EntryView entryView;

    private void OnEnable()
    {
        EntryManager.Instance.OnEntryChanged += UpdateEntryView;
    }

    private void OnDisable()
    {
        EntryManager.Instance.OnEntryChanged -= UpdateEntryView;
    }

    private void UpdateEntryView()
    {
        var currentEntries = EntryManager.Instance.selectedEntries;
        entryView.SetEntries(currentEntries);
    }
}

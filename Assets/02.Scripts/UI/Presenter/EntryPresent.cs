using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPresent : MonoBehaviour
{
    [SerializeField] private EntryView entryView;

    private void OnEnable()
    {
        if (PlayerManager.Instance != null && PlayerManager.Instance.player != null)
        {
            UpdateEntryView();
            // Player에 OnEntryChanged 이벤트가 있으면 구독하는 것이 좋음
            // PlayerManager.Instance.player.OnEntryChanged += UpdateEntryView;
        }
    }

    private void OnDisable()
    {
        // PlayerManager.Instance.player.OnEntryChanged -= UpdateEntryView;
    }

    private void UpdateEntryView()
    {
        if (PlayerManager.Instance == null || PlayerManager.Instance.player == null) return;

        var currentEntries = PlayerManager.Instance.player.battleEntry; // List<Monster>라고 가정

        // 만약 EntryView가 List<MonsterData>를 받는다면 MonsterData 리스트로 변환 필요
        List<Monster> monsterData = new List<Monster>();
        foreach (var monster in currentEntries)
        {
            if (monster != null)
                monsterData.Add(monster);
        }

        entryView.SetEntries(monsterData);
    }
}

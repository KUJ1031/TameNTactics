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
            // Player 내 리스트 변화를 감지할 이벤트가 없으면 일단 바로 업데이트
            UpdateEntryView();
            // 만약 Player에 OnEntryChanged 같은 이벤트가 있다면 거기에 구독하는 것이 좋음
        }
    }

    private void OnDisable()
    {

    }

    private void UpdateEntryView()
    {
        if (PlayerManager.Instance == null || PlayerManager.Instance.player == null) return;

        var currentEntries = PlayerManager.Instance.player.battleEntry;
        entryView.SetEntries(currentEntries);
    }
}
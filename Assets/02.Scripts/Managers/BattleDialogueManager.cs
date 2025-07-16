using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogueManager : Singleton<BattleDialogueManager>
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private ScrollRect scrollRect;

    // 배틀 내용을 보여줄 메서드
    public void BattleDialogueAppend(string battleDetail)
    {
        UIManager.Instance.battleUIManager.BattleInfoView.BattleDialogue(battleDetail);
        ScrollToBottom();
    }

    private void ScrollToBottom()
    {
        StartCoroutine(ScrollCoroutine());
    }

    private IEnumerator ScrollCoroutine()
    {
        yield return null;
        scrollRect.verticalNormalizedPosition = 0f;
    }
}

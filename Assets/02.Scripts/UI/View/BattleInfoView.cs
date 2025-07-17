using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class BattleInfoView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI battleDialogue;
    [SerializeField] private TextMeshProUGUI endBattleMessage;

    private StringBuilder logBuilder = new();

    // 배틀 중 Dialogue를 출력해줄 메서드
    public void BattleDialogue(string battleLog)
    {
        logBuilder.AppendLine(battleLog);
        battleDialogue.text = logBuilder.ToString();
    }

    // 배틀 끝나고 나서 Dialogue 초기화
    public void ClearBattleDialogue()
    {
        logBuilder.Clear();
        battleDialogue.text = string.Empty;
    }

    public void ShowEndBattleMessage(bool isWin)
    {
        endBattleMessage.gameObject.SetActive(true);

        if (isWin)
        {
            endBattleMessage.text = "승리했습니다!";
        }
        else
        {
            endBattleMessage.text = "패배했습니다...";
        }
    }
}

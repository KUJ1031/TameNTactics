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

    public void UseSkillDialogue(Monster attacker, Monster target, int damage, SkillData skillData)
    {
        bool isAlly = true;

        if (BattleManager.Instance.BattleEntryTeam.Contains(attacker))
        {
            isAlly = true;
        }
        else
        {
            isAlly = false;
        }

        string skillName = skillData.skillName;
        string useSkill = $"{(isAlly ? "우리" : "적")} {attacker.monsterName}의 {skillName} 공격!\n";
        string message = $"{(isAlly ? "" : "적의")} {attacker.monsterName}이(가) {(isAlly ? "적" : "우리")} {target.monsterName}에게 {damage}의 피해를 주었습니다!\n";
        BattleDialogueAppend(useSkill + message);
    }

    public void UseRunFailDialogue()
    {
        BattleDialogueAppend("도망가기 실패...!\n");
    }

    public void ClearBattleDialogue()
    {
        UIManager.Instance.battleUIManager.BattleInfoView.ClearBattleDialogue();
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

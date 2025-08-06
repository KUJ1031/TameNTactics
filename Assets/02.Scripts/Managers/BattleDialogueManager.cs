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

    public void UseItemDialogue(Monster target, int amount, ItemData itemData)
    {
        if (target == null || itemData == null) return;

        string itemName = itemData.itemName;
        string targetName = target.monsterName;

        string dialogue = $"우리 {targetName}에게 {itemName} 사용!\n";
        dialogue += $"{amount}의 체력이 회복되었습니다!\n\n";

        BattleDialogueAppend(dialogue);
    }

    public void PassiveEffectDialogue(Monster caster, SkillData skill)
    {
        if (caster == null) return;

        string casterName = caster.monsterName;

        string dialogue = $"{casterName}의 \'{skill.skillName}\' 패시브 스킬이 발동했다!\n\n";

        BattleDialogueAppend(dialogue);
    }

    public void UseRunFailDialogue()
    {
        BattleDialogueAppend("도망가기 실패...!\n");
    }

    public void ClearBattleEndPanel()
    {
        UIManager.Instance.battleUIManager.BattleInfoView.ClearBattleEndPanel();
    }

    /// <summary>
    /// 배틀 관련 내용이 스크롤에 많아질 경우
    /// 스크롤을 자동으로 아래로 내리도록 하는 코루틴
    /// </summary>
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

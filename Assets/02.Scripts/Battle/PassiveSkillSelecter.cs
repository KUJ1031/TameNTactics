using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PassiveSkillSelecter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private int monsterIndex;
    private SkillData passiveSkillData;

    public void InitializePassiveSkill(int index)
    {
        monsterIndex = index;
        SetMonsterPassiveSkill();
    }

    private void SetMonsterPassiveSkill()
    {
        List<Monster> monsters = BattleManager.Instance.BattleEntryTeam;

        if (monsters == null || monsterIndex < 0 || monsterIndex >= monsters.Count)
        {
            Debug.LogWarning($"[PassiveSkillSelecter] 유효하지 않은 몬스터 인덱스 : {monsterIndex}");
            return;
        }

        Monster monster = monsters[monsterIndex];
        if (monster != null && monster.skills != null && monster.skills.Count > 0)
        {
            if (monster.skills[0].skillType == SkillType.PassiveSkill)
            {
                passiveSkillData = monster.skills[0];
            }
            else return;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (passiveSkillData == null) return;

        UIManager.Instance.battleUIManager.SkillView.ShowPassiveSkillTooltip(passiveSkillData.name, passiveSkillData.description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.battleUIManager.SkillView.HidePassiveSkillTooltip();
    }
}

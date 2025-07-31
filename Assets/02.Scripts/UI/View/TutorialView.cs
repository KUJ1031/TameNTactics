using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialView : MonoBehaviour
{
    [SerializeField] private GameObject emphasize_tutorialPanel;
    [SerializeField] private GameObject emphasize_tutorialMonsterSelectPanel;
    [SerializeField] private GameObject emphasize_tutorialSkillPanel;
    [SerializeField] private GameObject emphasize_tutorialEnemySelectPanel;
    [SerializeField] private GameObject emphasize_inventoryPanel;
    [SerializeField] private GameObject emphasize_tutorialMonsterSelectItemPanel; // 아이템 선택 패널
    [SerializeField] private GameObject emphasize_tutorialMonsterSelectButtonPanel; // 아이템 선택 패널 2 (예: 스킬 선택 등)
    [SerializeField] private GameObject emphasize_itemUsePanel; // 아이템 사용 패널
    [SerializeField] private GameObject emphasize_embracePanel;
    [SerializeField] private GameObject emphasize_tutorialEnemySelectembracePanel;
    [SerializeField] private GameObject emphasize_TalkingButtonPanel; // 포섭 대상 선택 패널 (추가 필요시)
    [SerializeField] private GameObject emphasize_minigamePanel; // 미니게임 패널 (추가 필요시)
    [SerializeField] private GameObject emphasize_runAwayPanel;

    public Guide guide; // 가이드 패널 (추가 필요시)

    public void ShowTutorialPanel()
    {
        emphasize_tutorialPanel.SetActive(true);
    }
    public void HideTutorialPanel()
    {
        emphasize_tutorialPanel.SetActive(false);
    }

    public void ShowTutorialMonsterSelectPanel()
    {
        emphasize_tutorialMonsterSelectPanel.SetActive(true);
    }

    public void HideTutorialMonsterSelectPanel()
    {
        emphasize_tutorialMonsterSelectPanel.SetActive(false);
    }

    public void ShowTutorialSkillPanel()
    {
        emphasize_tutorialSkillPanel.SetActive(true);
    }

    public void HideTutorialSkillPanel()
    {
        emphasize_tutorialSkillPanel.SetActive(false);
    }

    public void ShowTutorialEnemySelectPanel()
    {
        emphasize_tutorialEnemySelectPanel.SetActive(true);
    }

    public void HideTutorialEnemySelectPanel()
    {
        emphasize_tutorialEnemySelectPanel.SetActive(false);
        BattleTutorialManager.Instance.isBattleAttackTutorialEnded = true; // 공격 튜토리얼 완료
    }
    public void ShowInventoryPanel()
    {
        emphasize_inventoryPanel.SetActive(true);
    }
    public void HideInventoryPanel()
    {
        emphasize_inventoryPanel.SetActive(false);
    }

    public void ShowItemSelectPanel()
    {
        emphasize_tutorialMonsterSelectItemPanel.SetActive(true);
    }
    public void HideItemSelectPanel()
    {
        emphasize_tutorialMonsterSelectItemPanel.SetActive(false);
    }

    public void ShowItemSelectButtonPanel()
    {
        emphasize_tutorialMonsterSelectButtonPanel.SetActive(true);
    }

    public void HideItemSelectButtonPanel()
    {
        emphasize_tutorialMonsterSelectButtonPanel.SetActive(false);
    }

    public void ShowItemUsePanel()
    {
        emphasize_itemUsePanel.SetActive(true);
    }
    public void HideItemUsePanel()
    {
        emphasize_itemUsePanel.SetActive(false);
    }
    public void ShowEmbracePanel()
    {
        emphasize_embracePanel.SetActive(true);
    }
    public void HideEmbracePanel()
    {
        emphasize_embracePanel.SetActive(false);
    }

    public void ShowTalkingButtonPanel()
    {
        emphasize_TalkingButtonPanel.SetActive(true);
    }

    public void HideTalkingButtonPanel()
    {
        emphasize_TalkingButtonPanel.SetActive(false);
    }

    public void ShowEmbraceEnemySelectPanel()
    {
        emphasize_tutorialEnemySelectembracePanel.SetActive(true);
    }

    public void HideEmbraceEnemySelectPanel()
    {
        emphasize_tutorialEnemySelectembracePanel.SetActive(false);
    }

    public void ShowMinigamePanel()
    {
        emphasize_minigamePanel.SetActive(true);
    }

    public void HideMinigamePanel()
    {
        emphasize_minigamePanel.SetActive(false);
    }

    public void ShowRunAwayPanel()
    {
        emphasize_runAwayPanel.SetActive(true);
    }
    public void HideRunAwayPanel()
    {
        emphasize_runAwayPanel.SetActive(false);
    }

    public void ShowGuideAttackPanel()
    {
        if (guide != null)
        {
            guide.StartGuide(GuideType.Attack);
        }
        else
        {
            Debug.LogWarning("Guide 컴포넌트가 할당되지 않았습니다.");
        }
    }

    public void ShowGuideInventoryPanel()
    {
        if (guide != null)
        {
            guide.StartGuide(GuideType.Inventory);
        }
        else
        {
            Debug.LogWarning("Guide 컴포넌트가 할당되지 않았습니다.");
        }
    }

    public void ShowGuideEmbracePanel()
    {
        if (guide != null)
        {
            guide.StartGuide(GuideType.Embrace);
        }
        else
        {
            Debug.LogWarning("Guide 컴포넌트가 할당되지 않았습니다.");
        }
    }

    public void ShowGuideEscapePanel()
    {
        if (guide != null)
        {
            guide.StartGuide(GuideType.Escape);
        }
        else
        {
            Debug.LogWarning("Guide 컴포넌트가 할당되지 않았습니다.");
        }
    }


}

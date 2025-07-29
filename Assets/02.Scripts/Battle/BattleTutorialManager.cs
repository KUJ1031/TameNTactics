using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleTutorialManager : Singleton<BattleTutorialManager>
{

    internal bool isBattleTutorialEnded = false; // 배틀 튜토리얼 여부
    [SerializeField] private Sprite npcSprite; // NPC 이미지
    public TutorialView tutorialView; // 배틀 튜토리얼 패널

    internal bool isBattleAttackTutorialEnded = false; // 배틀 공격 튜토리얼 여부
    internal bool isBattleInventoryTutorialEnded = false; // 배틀 아이템 튜토리얼 여부
    internal bool isBattleEscapeTutorialEnded = false; // 배틀 도망 튜토리얼 여부

    internal bool isBattleEmbraceTutorialStarded = false; // 배틀 포섭 튜토리얼 여부
    internal bool isBattleEmbraceTutorialEnded = false; // 배틀 포섭 튜토리얼 여부

    public void InitialBattle()
    {
        if (!isBattleTutorialEnded)
        {
            if (!isBattleAttackTutorialEnded)
            {
                DialogueManager.Instance.OnDialogueLoaded += () =>
                {
                    DialogueManager.Instance.StartDialogue("카이렌", npcSprite, 6000);
                };
            }
            else if (!isBattleInventoryTutorialEnded)
            {
                InitInventorySelected();
            }
            else if (!isBattleEscapeTutorialEnded)
            {
                InitEscapeSelected();
            }
            else if (!isBattleEmbraceTutorialEnded && isBattleEmbraceTutorialStarded)
            {
                InitEmbraceSelected();
            }
            else
            {
                Debug.Log("배틀 튜토리얼이 이미 완료되었습니다.");
            }
        }
    }

    public void AddBattleMember()
    {
        // 카이렌 추가 로직
        if (!isBattleTutorialEnded)
        {
        }
    }

    public void ShowAttackGuide()
    {
        Debug.Log("공격 가이드를 보여줍니다.");
    }

    public void HideAttackGuide()
    {
        Debug.Log("공격 가이드를 숨깁니다.");
    }

    public void ShowSkillGuide()
    {
        Debug.Log("스킬 가이드를 보여줍니다.");
    }

    public void HideSkillGuide()
    {
        Debug.Log("스킬 가이드를 숨깁니다.");
    }

    public void ShowItemGuide()
    {
        Debug.Log("아이템 가이드를 보여줍니다.");
    }

    public void HideItemGuide()
    {
        Debug.Log("아이템 가이드를 숨깁니다.");
    }

    public void ShowEscapeGuide()
    {
        Debug.Log("도망 가이드를 보여줍니다.");
    }

    public void HideEscapeGuide()
    {
        Debug.Log("도망 가이드를 숨깁니다.");
    }

    public void ShowEmbraceGuide()
    {
        Debug.Log("포섭 가이드를 보여줍니다.");
    }

    public void HideEmbraceGuide()
    {
        Debug.Log("포섭 가이드를 숨깁니다.");
    }

    public void ShowMinigameGuide()
    {
        Debug.Log("미니게임 가이드를 보여줍니다.");
    }

    public void HideMinigameGuide()
    {
        Debug.Log("미니게임 가이드를 숨깁니다.");
    }



    public void InitAttackSelected()
    {
        // 공격 선택 초기화 로직
        tutorialView.ShowTutorialPanel();
        
        // 여기에 공격 선택 관련 코드를 추가하세요.
    }

    public void InitMonsterelected_Attack()
    {
        // 공격 선택 초기화 로직
        tutorialView.HideTutorialPanel();
        tutorialView.ShowTutorialMonsterSelectPanel();

        // 여기에 공격 선택 관련 코드를 추가하세요.
    }

    public void InitSkillSelected()
    {
        // 스킬 선택 초기화 로직
        Debug.Log("스킬을 선택합니다.");
        tutorialView.HideTutorialMonsterSelectPanel();
        tutorialView.ShowTutorialSkillPanel();
        // 여기에 스킬 선택 관련 코드를 추가하세요.
    }

    public void InitEnemySelected_Attack()
    {
        // 적 선택 후 공격 초기화 로직
        Debug.Log("적을 선택하고 공격합니다.");
        tutorialView.HideTutorialSkillPanel();
        tutorialView.ShowTutorialEnemySelectPanel();
        // 여기에 적 선택 후 공격 관련 코드를 추가하세요.
    }

    public void EndAttackTutorial()
    {
        // 공격 튜토리얼 종료 로직
        Debug.Log("공격 튜토리얼을 종료합니다.");
        isBattleAttackTutorialEnded = true;
        tutorialView.HideTutorialEnemySelectPanel();
        // 여기에 공격 튜토리얼 종료 관련 코드를 추가하세요.
    }

    public void InitInventorySelected()
    {
        // 아이템 선택 초기화 로직
        Debug.Log("인벤토리를 선택합니다.");
        tutorialView.ShowInventoryPanel();
        // 여기에 아이템 선택 관련 코드를 추가하세요.
    }

    public void InitItemSelected()
    {
        // 아이템 선택 초기화 로직
        Debug.Log("아이템을 선택합니다.");
        tutorialView.HideInventoryPanel();
        tutorialView.ShowItemSelectPanel();
        // 여기에 아이템 선택 관련 코드를 추가하세요.
    }

    public void InitItemButtonSelected()
    {
        tutorialView.HideItemSelectPanel();
        tutorialView.ShowItemSelectButtonPanel();
    }

    public void InitMonsterItemSelected()
    {
        tutorialView.HideItemSelectButtonPanel();
        tutorialView.ShowItemUsePanel();
        isBattleInventoryTutorialEnded = true;
        Debug.Log("아이템을 사용할 몬스터를 선택합니다.");
    }

    public void EndInventoryTutorial()
    {
        // 공격 튜토리얼 종료 로직
        Debug.Log("인벤토리 튜토리얼을 종료합니다.");
        isBattleInventoryTutorialEnded = true;
        tutorialView.HideItemUsePanel();
        // 여기에 공격 튜토리얼 종료 관련 코드를 추가하세요.
    }

    public void InitEscapeSelected()
    {
        // 도망 선택 초기화 로직
        tutorialView.ShowRunAwayPanel();
        Debug.Log("도망을 선택합니다.");
        // 여기에 도망 선택 관련 코드를 추가하세요.
    }

    public void EndEscapeTutorial()
    {
        // 공격 튜토리얼 종료 로직
        Debug.Log("도망 튜토리얼을 종료합니다.");
        isBattleEscapeTutorialEnded = true;
        tutorialView.HideRunAwayPanel();
        // 여기에 공격 튜토리얼 종료 관련 코드를 추가하세요.
    }

    public void InitEmbraceSelected()
    {
        // 포섭 선택 초기화 로직
        Debug.Log("포섭을 선택합니다.");
        tutorialView.ShowEmbracePanel();
        tutorialView.HideEmbraceEnemySelectPanel();
        // 여기에 포섭 선택 관련 코드를 추가하세요.
    }

    public void InitTalkingSelected()
    {
        // 대화 선택 초기화 로직
        Debug.Log("대화를 선택합니다.");
        tutorialView.HideEmbracePanel();
        tutorialView.ShowTalkingButtonPanel();
        // 여기에 대화 선택 관련 코드를 추가하세요.
    }

    public void InitEnemySelected_Embrace()
    {
        // 적 선택 후 공격 초기화 로직
        Debug.Log("적을 선택하고 포섭합니다.");
        tutorialView.HideTalkingButtonPanel();
        tutorialView.ShowEmbraceEnemySelectPanel();

        // 여기에 적 선택 후 공격 관련 코드를 추가하세요.
    }

    public void InitMinigame()
    {
        tutorialView.HideEmbraceEnemySelectPanel();
        tutorialView.ShowMinigamePanel();
        // 적 선택 후 스킬 초기화 로직
        Debug.Log("미니게임을 시작합니다.");
        // 여기에 적 선택 후 스킬 관련 코드를 추가하세요.
    }

    public void EndEmbraceTutorial()
    {
        // 포섭 튜토리얼 종료 로직
        Debug.Log("포섭 튜토리얼을 종료합니다.");
        isBattleEmbraceTutorialEnded = true;
        tutorialView.HideMinigamePanel();
        DialogueManager.Instance.StartDialogue("카이렌", npcSprite, 6400);
        // 여기에 포섭 튜토리얼 종료 관련 코드를 추가하세요.
    }

    public void EndTutorial()
    {
        // 배틀 튜토리얼 종료 로직
        Debug.Log("배틀 튜토리얼을 종료합니다.");
        isBattleTutorialEnded = true;
        Time.timeScale = 0f; // 게임 속도 원래대로
        // 여기에 배틀 튜토리얼 종료 관련 코드를 추가하세요.
    }

    public void RunAwayTry()
    {
        DialogueManager.Instance.StartDialogue("카이렌", npcSprite, 6300);
    }
}

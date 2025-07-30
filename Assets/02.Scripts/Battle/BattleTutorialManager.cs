using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleTutorialManager : Singleton<BattleTutorialManager>
{

    [SerializeField] private Sprite npcSprite; // NPC 이미지
    public TutorialView tutorialView; // 배틀 튜토리얼 패널

    internal bool isBattleAttackTutorialEnded = false; // 배틀 공격 튜토리얼 여부
    internal bool isBattleInventoryTutorialEnded = false; // 배틀 아이템 튜토리얼 여부
    internal bool isBattleEscapeTutorialEnded = false; // 배틀 도망 튜토리얼 여부

    internal bool isBattleEmbraceTutorialStarded = false; // 배틀 포섭 튜토리얼 여부
    internal bool isBattleEmbraceTutorialEnded = false; // 배틀 포섭 튜토리얼 여부

    public SpawnBattleAllMonsters spawnBattleAllMonsters; // 카이렌 등장위한 참조

    public MonsterData reaward_Kairen;

    public void InitialBattle()
    {
        if (!PlayerManager.Instance.player.playerTutorialCheck)
        {
            PlayerManager.Instance.playerController.isInputBlocked = false;
            if (!isBattleAttackTutorialEnded)
            {
                StartCoroutine(WaitUntilDialogueLoadedAndStart());
            }
            else if (!isBattleInventoryTutorialEnded)
            {
                DialogueManager.Instance.StartDialogue("카이렌", npcSprite, 6100);
            }
            else if (!isBattleEscapeTutorialEnded)
            {
                InitEscapeSelected();
            }
            else
            {
                Debug.Log("배틀 튜토리얼이 이미 완료되었습니다.");
            }
        }
    }

    private IEnumerator WaitUntilDialogueLoadedAndStart()
    {
        yield return new WaitUntil(() => DialogueManager.Instance.IsLoaded);
        DialogueManager.Instance.StartDialogue("카이렌", npcSprite, 6000);
    }

    public void AddMemberKairen()
    {
        spawnBattleAllMonsters.CreateMonster(PlayerManager.Instance.player.battleEntry, spawnBattleAllMonsters.allySpawner);
        UIManager.Instance.battleUIManager.SettingMonsterInfo(spawnBattleAllMonsters.allySpawner, spawnBattleAllMonsters.enemySpawner);
        UIManager.Instance.battleUIManager.SettingMonsterPassive(PlayerManager.Instance.player.battleEntry);
        UIManager.Instance.battleUIManager.SettingMonsterSelecter(spawnBattleAllMonsters.allySpawner, spawnBattleAllMonsters.enemySpawner);
    }

    public void InitAttackSelected()
    {
        // 공격 선택 초기화 로직
        if (!PlayerManager.Instance.player.playerTutorialCheck)
        {
            tutorialView.ShowTutorialPanel();
            tutorialView.ShowGuideAttackPanel();
        }

       
        
        // 여기에 공격 선택 관련 코드를 추가하세요.
    }

    public void InitMonsterelected_Attack()
    {
        // 공격 선택 초기화 로직
        if (!PlayerManager.Instance.player.playerTutorialCheck)
        {
            tutorialView.HideTutorialPanel();
            tutorialView.ShowTutorialMonsterSelectPanel();

        }


        // 여기에 공격 선택 관련 코드를 추가하세요.
    }

    public void InitSkillSelected()
    {
        // 스킬 선택 초기화 로직
        if (!PlayerManager.Instance.player.playerTutorialCheck)
        {
            tutorialView.HideTutorialMonsterSelectPanel();
            tutorialView.ShowTutorialSkillPanel();
        }
        // 여기에 스킬 선택 관련 코드를 추가하세요.
    }

    public void InitEnemySelected_Attack()
    {
        // 적 선택 후 공격 초기화 로직
        if (!PlayerManager.Instance.player.playerTutorialCheck)
        {
            tutorialView.HideTutorialSkillPanel();
            tutorialView.ShowTutorialEnemySelectPanel();
        }        
        // 여기에 적 선택 후 공격 관련 코드를 추가하세요.
    }

    public void EndAttackTutorial()
    {
        // 공격 튜토리얼 종료 로직
        if (!PlayerManager.Instance.player.playerTutorialCheck)
        {
            isBattleAttackTutorialEnded = true;
            tutorialView.HideTutorialEnemySelectPanel();
        }

        // 여기에 공격 튜토리얼 종료 관련 코드를 추가하세요.
    }

    public void InitInventorySelected()
    {
        if (!PlayerManager.Instance.player.playerTutorialCheck)
        {
            tutorialView.ShowInventoryPanel();
            tutorialView.ShowGuideInventoryPanel();
        }
    }

    public void InitItemSelected()
    {
        // 아이템 선택 초기화 로직
        if (!PlayerManager.Instance.player.playerTutorialCheck)
        {
            tutorialView.HideInventoryPanel();
            tutorialView.ShowItemSelectPanel();
        }
        // 여기에 아이템 선택 관련 코드를 추가하세요.
    }

    public void InitItemButtonSelected()
    {
        if (!PlayerManager.Instance.player.playerTutorialCheck)
            tutorialView.HideItemSelectPanel();
        tutorialView.ShowItemSelectButtonPanel();
    }

    public void InitMonsterItemSelected()
    {
        if (!PlayerManager.Instance.player.playerTutorialCheck)
        {
            tutorialView.HideItemSelectButtonPanel();
            tutorialView.ShowItemUsePanel();
            isBattleInventoryTutorialEnded = true;
        }

    }

    public void EndInventoryTutorial()
    {
        if (!PlayerManager.Instance.player.playerTutorialCheck)
        {
            isBattleInventoryTutorialEnded = true;
            tutorialView.HideItemUsePanel();
        }

        // 여기에 공격 튜토리얼 종료 관련 코드를 추가하세요.
    }

    public void InitEscapeSelected()
    {
        if (!PlayerManager.Instance.player.playerTutorialCheck)
        {
            tutorialView.ShowRunAwayPanel();
            tutorialView.ShowGuideEscapePanel();
        }
        // 여기에 도망 선택 관련 코드를 추가하세요.
    }

    public void EndEscapeTutorial()
    {
        if (!PlayerManager.Instance.player.playerTutorialCheck)
        {
            isBattleEscapeTutorialEnded = true;
            tutorialView.HideRunAwayPanel();
        }
            // 공격 튜토리얼 종료 로직

        // 여기에 공격 튜토리얼 종료 관련 코드를 추가하세요.
    }

    public void InitEmbraceSelected()
    {
        if (!PlayerManager.Instance.player.playerTutorialCheck)
        {
            tutorialView.ShowEmbracePanel();
            tutorialView.HideEmbraceEnemySelectPanel();
            tutorialView.ShowGuideEmbracePanel();
        }
        // 여기에 포섭 선택 관련 코드를 추가하세요.
    }

    public void InitTalkingSelected()
    {
        if (!PlayerManager.Instance.player.playerTutorialCheck)
        {
            tutorialView.HideEmbracePanel();
            tutorialView.ShowTalkingButtonPanel();
        }
        // 여기에 대화 선택 관련 코드를 추가하세요.
    }

    public void InitEnemySelected_Embrace()
    {
        if (!PlayerManager.Instance.player.playerTutorialCheck)
        {
            tutorialView.HideTalkingButtonPanel();
            tutorialView.ShowEmbraceEnemySelectPanel();
        }
        // 여기에 적 선택 후 공격 관련 코드를 추가하세요.
    }

    public void InitMinigame()
    {
        if (!PlayerManager.Instance.player.playerTutorialCheck)
        {
            tutorialView.HideEmbraceEnemySelectPanel();
            tutorialView.ShowMinigamePanel();
        }
    }

    public void EndEmbraceTutorial()
    {
        if (!PlayerManager.Instance.player.playerTutorialCheck)
        {
            isBattleEmbraceTutorialEnded = true;
            tutorialView.HideMinigamePanel();
            DialogueManager.Instance.StartDialogue("카이렌", npcSprite, 6400);
        }
        // 여기에 포섭 튜토리얼 종료 관련 코드를 추가하세요.
    }

    public void EndTutorial()
    {
        if (!PlayerManager.Instance.player.playerTutorialCheck)
        {
            PlayerManager.Instance.player.playerTutorialCheck = true;
        }
        // 여기에 배틀 튜토리얼 종료 관련 코드를 추가하세요.
    }

    public void RunAwayTry()
    {
        if (!PlayerManager.Instance.player.playerTutorialCheck)
            DialogueManager.Instance.StartDialogue("카이렌", npcSprite, 6300);
    }

    public void MinigameFailed()
    {
        if (!PlayerManager.Instance.player.playerTutorialCheck)
            DialogueManager.Instance.StartDialogue("카이렌", npcSprite, 6200);
    }
}

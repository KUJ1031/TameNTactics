using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : Singleton<TutorialManager>
{
    internal bool gameinit = true;
    [SerializeField] private Transform battleLaterTransform;
    [SerializeField] private Sprite npcSprite;

    [SerializeField] private bool tutorialComplete;

    [SerializeField] private GameObject CompleteTutorialPanel;
    [SerializeField] private Button tutorialPanelExitButton;

    protected override void Awake()
    {
        if (PlayerManager.Instance.player.playerAllTutorialCheck)
        {
            Debug.Log("모든 튜토리얼을 완료했습니다. 튜토리얼 매니저를 제거합니다.");
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        if (PlayerManager.Instance.player.playerBattleTutorialCheck)
        {
            Debug.Log("필드 내 튜토리얼 시작.");
            PlayerManager.Instance.playerController.isInputBlocked = true;
            PlayerManager.Instance.playerController.transform.position = battleLaterTransform.position;
            StartCoroutine(WaitUntilDialogueLoadedAndStart());
            tutorialPanelExitButton.onClick.AddListener(ExitTutorialPanel);
        }

    }
    private IEnumerator WaitUntilDialogueLoadedAndStart()
    {
        yield return new WaitUntil(() => DialogueManager.Instance.IsLoaded);
        DialogueManager.Instance.StartDialogue("카이렌", npcSprite, 6500);
    }

    public void MoveCamToVillage()
    {
        if (PlayerManager.Instance.player.playerBattleTutorialCheck)
        {
            StartCoroutine(ShowVillageThenStartTutorial());
        }
    }

    private IEnumerator ShowVillageThenStartTutorial()
    {
        // 1. 카메라를 마을 위치로 이동 (타겟 변경)
        CameraController.Instance.SwitchTo("VillageCam", true, true);
        Debug.Log("카메라가 마을로 이동했습니다.");

        // 2. 3초간 대기 (마을 풍경 보여주기)
        yield return new WaitForSeconds(3f);

        // 3. 플레이어 위치 battleLaterTransform 위치로 이동
        PlayerManager.Instance.playerController.transform.position = battleLaterTransform.position;

        // 4. 카메라 타겟을 플레이어로 다시 변경
        CameraController.Instance.SwitchTo("PlayerCamera", true, true); // 기존 타겟 유지
        CameraController.Instance.SetTarget(PlayerManager.Instance.playerController.transform);
        Debug.Log("카메라가 플레이어로 돌아왔습니다.");

        // 5. 다시 3초 대기 (카메라 복귀 애니메이션 고려)
        yield return new WaitForSeconds(3f);

        // 6. 대화 시작
        DialogueManager.Instance.StartDialogue("카이렌", npcSprite, 6520);
    }

    public void Guide_MonsterOpen()
    {
        FieldUIManager.Instance.playerGuideUI.gameObject.SetActive(true);
        FieldUIManager.Instance.playerGuideUI.ShowCategory("몬스터");
        DialogueManager.Instance.StartDialogue("카이렌", npcSprite, 6531);
    }

    public void Guide_EntryOpen()
    {
        FieldUIManager.Instance.playerGuideUI.gameObject.SetActive(true);
        FieldUIManager.Instance.playerGuideUI.ShowCategory("엔트리");
        DialogueManager.Instance.StartDialogue("카이렌", npcSprite, 6540);
    }

    public void Guide_BattleOpen()
    {
        FieldUIManager.Instance.playerGuideUI.gameObject.SetActive(true);
        FieldUIManager.Instance.playerGuideUI.ShowCategory("전투");
        DialogueManager.Instance.StartDialogue("카이렌", npcSprite, 6550);
    }

    public void Guide_SaveOpen()
    {
        FieldUIManager.Instance.playerGuideUI.gameObject.SetActive(true);
        FieldUIManager.Instance.playerGuideUI.ShowCategory("저장");
        DialogueManager.Instance.StartDialogue("카이렌", npcSprite, 6560);
    }

    public void Guide_MinimapOpen()
    {
        FieldUIManager.Instance.playerGuideUI.gameObject.SetActive(true);
        FieldUIManager.Instance.playerGuideUI.ShowCategory("미니맵");
        DialogueManager.Instance.StartDialogue("카이렌", npcSprite, 6570);

    }

    public void ExitTutorialPanel()
    {
        Debug.Log("튜토리얼 패널을 닫고, 튜토리얼매니저를 제거합니다.");
        CompleteTutorialPanel.SetActive(false);
        PlayerManager.Instance.playerController.isInputBlocked = false;
        PlayerManager.Instance.player.playerAllTutorialCheck = true;
        PlayerManager.Instance.player.playerQuestClearCheck[0] = true;
        PlayerManager.Instance.player.playerQuestStartCheck[0] = false;
        EventAlertManager.Instance.SetEventAlert(EventAlertType.QuestClear, null, "전투의 기본");
        PlayerManager.Instance.player.AddItem("설득하기", 1);
        Debug.Log("설득 기술을 획득했습니다. 아이템: " + ItemManager.Instance.gestureItems[1].itemName);
        PlayerManager.Instance.player.playerQuestStartCheck[3] = true;
        EventAlertManager.Instance.SetEventAlert(EventAlertType.QuestStart, null, "떠돌이 상인");
        Destroy(gameObject);
    }

    public void TutorialCompeleted()
    {
        Debug.Log("튜토리얼이 완료되었습니다.");
        CompleteTutorialPanel.SetActive(true);
    }
}

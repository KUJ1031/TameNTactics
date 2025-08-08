using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;
using System.Linq;


public class DialogueManager : Singleton<DialogueManager>
{

    protected override bool IsDontDestroy => true;

    [Header("어드레서블")]
    public string csvAddress = "dialogue"; // 어드레서블 이름 (e.g. Assets/AddressableAssetsData/Dialogs/dialogue.csv)

    [Header("UI 연결")]
    public DialogueUI dialogueUI;

    private Dictionary<string, Dictionary<int, DialogueNode>> dialogueTrees = new();
    private Dictionary<int, DialogueNode> currentTree;
    private DialogueNode currentNode;

    private Sprite currentNPCImage;
    private string currentSpeakerName;

    private bool isSkipping = false;
    public bool isCommunicationEneded = false; // 대화 종료 여부

    private Coroutine skipBlinkCoroutine;

    private Dictionary<string, Sprite> speakerSprites = new(); // 이름 → 스프라이트

    public bool IsLoaded { get; private set; } = false;

    public event Action OnDialogueLoaded;
    public event Action OnDialogueEnded;

    protected override void Awake()
    {
        if (dialogueUI == null)
            dialogueUI = FindObjectOfType<DialogueUI>();
        // CSV 데이터 로딩
        Addressables.LoadAssetAsync<TextAsset>(csvAddress).Completed += OnCSVLoaded;
    }

    void Start()
    {
        
    }

    void OnCSVLoaded(AsyncOperationHandle<TextAsset> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            dialogueTrees = DialogueCSVParser.ParseByTreeID(handle.Result);
            Debug.Log("모든 NPC 대사 로드 완료");
            

            IsLoaded = true; // 여기서 로드 완료 플래그 true
            OnDialogueLoaded?.Invoke(); // 이 타이밍에 알림

        }
        else
        {
            Debug.LogError("CSV 어드레서블 로드 실패");
        }
    }

    /// <summary>
    /// 대화 시작: 해당 NPC의 대화 ID로부터 시작
    /// </summary>
    public void StartDialogue(string dialogueTreeId, Sprite npcImage, int startID)
    {
        currentNPCImage = npcImage;
        isCommunicationEneded = false; // 대화 시작 시 초기화

        if (!dialogueTrees.ContainsKey(dialogueTreeId))
        {
            Debug.LogError($"대화 트리 '{dialogueTreeId}'가 없습니다.");
            return;
        }

        currentTree = dialogueTrees[dialogueTreeId];  // 트리 할당 필수!

        if (!currentTree.ContainsKey(startID))
        {
            Debug.LogError($"시작 ID {startID}가 대화 트리 '{dialogueTreeId}'에 없습니다.");
            return;
        }

        ShowNode(currentTree[startID]);
    }

    /// <summary>
    /// 현재 노드 표시
    /// </summary>
    void ShowNode(DialogueNode node)
    {
        //Debug.Log($"노드 표시: ID {node.ID}, 화자 {node.Speaker}, 텍스트 '{node.Text}'");
        currentNode = node;

        // 이벤트 트리거
        if (!string.IsNullOrEmpty(node.EventKey))
        {
            TriggerEvent(node.EventKey);
        }

        Sprite speakerImage = GetSpeakerSprite(node.Speaker);

        dialogueUI.Show(
            node,
            speakerImage,
            node.Speaker
        );

        if (node.Next == -1 && string.IsNullOrEmpty(node.Choice1) && string.IsNullOrEmpty(node.Choice2) && string.IsNullOrEmpty(node.Choice3))
        {
            dialogueUI.skipButton.gameObject.SetActive(false);
        }
    }

    private Sprite GetSpeakerSprite(string speakerName)
    {
        if (speakerSprites.TryGetValue(speakerName, out var sprite))
            return sprite;

        return currentNPCImage; // 기본 이미지 (예: null or "???")
    }

    /// <summary>
    /// 선택지 클릭 시 호출
    /// </summary>
    public void OnSelectChoice(int choiceIndex)
    {
        int nextID = -1;

        if (choiceIndex == 1) nextID = currentNode.Choice1Next;
        else if (choiceIndex == 2) nextID = currentNode.Choice2Next;
        else if (choiceIndex == 3) nextID = currentNode.Choice3Next;
        else nextID = currentNode.Next;

        if (nextID == -1)
        {
            dialogueUI.Hide();
            isCommunicationEneded = true; // 대화 종료 상태로 설정
            OnDialogueEnded?.Invoke();

            if (!string.IsNullOrEmpty(currentNode.LateEventKey))
            {
                // 쉼표 또는 세미콜론으로 구분
                var keys = currentNode.LateEventKey.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var key in keys)
                {
                    TriggerEvent(key.Trim()); // 공백 제거 후 실행
                }
            }

            return;
        }

        if (currentTree.TryGetValue(nextID, out var nextNode))
        {
            ShowNode(nextNode);
        }
        else
        {
            Debug.LogWarning($" ID {nextID} 없음");
            dialogueUI.Hide();
        }
    }

    /// <summary>
    /// 스킵 버튼 클릭 시 호출
    /// </summary>
    public void OnClickSkip()
    {
        if (!isSkipping)
            StartCoroutine(SkipToNextChoiceOrEnd());
    }

    /// <summary>
    /// 선택지 등장 전까지 자동으로 다음 노드로 이동
    /// </summary>
    private IEnumerator SkipToNextChoiceOrEnd()
    {
        isSkipping = true;

        Image skipImage = dialogueUI.skipImage;
        skipImage.gameObject.SetActive(true);
        if (skipImage != null && skipBlinkCoroutine == null)
            skipBlinkCoroutine = StartCoroutine(BlinkSkipImage(skipImage));

        while (currentNode != null && isSkipping)
        {
            // 마지막 노드 도달 시 처리
            if (currentNode.Next == -1)
            {
                // 마지막 노드 보여준 상태 유지 (Hide 안 함)
                StopSkipBlink();
                isSkipping = false;
                dialogueUI.skipButton.gameObject.SetActive(false);
                yield break;
            }

            int nextID = currentNode.Next;

            if (currentTree.TryGetValue(nextID, out var nextNode))
            {
                ShowNode(nextNode);
                yield return new WaitForSeconds(0.2f);

                bool hasChoice = !string.IsNullOrEmpty(nextNode.Choice1) || !string.IsNullOrEmpty(nextNode.Choice2);
                if (hasChoice)
                {
                    StopSkipBlink();
                    isSkipping = false;
                    dialogueUI.skipButton.gameObject.SetActive(false);
                    yield break;
                }
            }
            else
            {
                Debug.LogWarning($"스킵 중: ID {nextID} 없음");
                dialogueUI.Hide();
                StopSkipBlink();
                isSkipping = false;
                dialogueUI.skipButton.gameObject.SetActive(false);
                yield break;
            }
        }

        StopSkipBlink();
        isSkipping = false;
    }
    private IEnumerator BlinkSkipImage(Image image)
    {
        Color originalColor = image.color;
        Text skipText = image.GetComponentInChildren<Text>();
        while (true)
        {
            // 점점 나타남
            for (float a = 0.1f; a <= 1f; a += Time.deltaTime * 1.5f)
            {
                SetAlpha(image, a);
                yield return null;
            }

            // 점점 사라짐
            for (float a = 1f; a >= 0.1f; a -= Time.deltaTime * 1.5f)
            {
                SetAlpha(image, a);
                yield return null;
            }
        }
    }

    private void StopSkipBlink()
    {
        if (skipBlinkCoroutine != null)
        {
            StopCoroutine(skipBlinkCoroutine);
            skipBlinkCoroutine = null;
        }

        if (dialogueUI.skipImage != null)
        {
            var image = dialogueUI.skipImage;
            var text = image.GetComponentInChildren<Text>();

            SetAlpha(image, 0f); // 알파 0으로 설정
            image.gameObject.SetActive(false);
        }
    }

    private void SetAlpha(Image image, float alpha)
    {
        if (image != null)
        {
            var c = image.color;
            c.a = alpha;
            image.color = c;
        }
    }

    public void SetDialogueUI(bool active)
    {
        if (active)
        {
            if (dialogueUI == null)
            {
                Debug.LogError("DialogueUI가 설정되지 않았습니다.");
                return;
            }
            dialogueUI.gameObject.SetActive(true);
        }
        else
        {
            dialogueUI?.gameObject.SetActive(false);
        }
    }

    private void TriggerEvent(string eventKey)
    {
        if (eventKey.Contains(":"))
        {
            string[] split = eventKey.Split(':');
            string command = split[0];
            string arg = split.Length > 1 ? split[1] : null;

            switch (command)
            {
                case "GiveItem":
                    if (string.IsNullOrEmpty(arg))
                    {
                        Debug.LogWarning("[이벤트] 아이템 이름이 없음");
                        return;
                    }

                    ItemData item = ItemManager.Instance.GetItemByName(arg);
                    if (item != null)
                    {
                        PlayerManager.Instance.player.AddItem(item);
                        Debug.Log($"[이벤트] 아이템 지급: {item.itemName}");
                        Destroy(ItemManager.Instance.item); // 아이템 오브젝트 제거
                    }
                    else
                    {
                        Debug.LogError($"[이벤트] 아이템 '{arg}'을(를) 찾을 수 없음");
                    }
                    return;

                    // 추후 확장 가능: PlayEffect:Fire, TriggerQuest:quest_001 등
            }
        }
        switch (eventKey)
        {
            case "TakeMoveCamStartZone":
                Debug.Log("[이벤트] 메인 맵 스타트로 이동");
                Transform playerTransform = PlayerManager.Instance.playerController.transform;
                playerTransform.position -= new Vector3(11f, 0f, 0f);
                CameraController.Instance.SwitchTo("StartCam", true, false); // 타겟 클리어
                break;

            case "TakeMoveCamTutorialZone":
                Debug.Log("[이벤트] 카메라 이동 튜토리얼 존으로 이동");
                CameraController.Instance.SwitchTo("TutorialZoneCam", true, false); // 타겟 클리어
                break;
            case "TakeMoveCamVillage":
                TutorialManager.Instance.MoveCamToVillage();
                break;
            case "TakeMoveCamPlayer":
                Debug.Log("[이벤트] 카메라 플레이어 시점으로 이동");
                CameraController.Instance.SwitchTo("PlayerCamera", true, false); // 기존 타겟 유지
                CameraController.Instance.SetTarget(PlayerManager.Instance.playerController.transform);
                if (!PlayerManager.Instance.player.playerBattleTutorialCheck && TutorialManager.Instance.gameinit)
                {
                    FieldUIManager.Instance.playerGuideUI.gameObject.SetActive(true);
                    FieldUIManager.Instance.playerGuideUI.ShowCategory("이동"); // 플레이어 튜토리얼이 안 끝났으면 UI 표시
                    TutorialManager.Instance.gameinit = false;
                }
                break;
            case "TakeMoveCamShop":
                Debug.Log("[이벤트] 카메라 상점 시점으로 이동");
                CameraController.Instance.SwitchTo("ShopCam", true, false); // 타겟 클리어
                break;
                case "PlayerInputBlock":
                Debug.Log("[이벤트] 플레이어 입력 차단");
                PlayerManager.Instance.playerController.isInputBlocked = true;
                break;
            case "PlayerInputUnBlock":
                Debug.Log("[이벤트] 플레이어 입력 차단 해제");
                PlayerManager.Instance.playerController.isInputBlocked = false;
                break;
            case "Shop_Buy":
                ShopManager.Instance.OpenShopUI();
                break;
            case "Shop_Sell":
                ShopManager.Instance.OpenShopUI_Sell();
                break;
            case "WanderingShop_Buy":
                ShopManager.Instance.OpenWanderingShopUI();
                break;
            case "OwnedMonsters_Healing":
                List<HealedMonsterInfo> ownedHealedList = new List<HealedMonsterInfo>();

                foreach (var monster in PlayerManager.Instance.player.ownedMonsters)
                {
                    if (monster != null)
                    {
                        int beforeHp = monster.CurHp;

                        monster.HealFull(); // 전부 회복 시도

                        int healedAmount = monster.CurHp - beforeHp;

                        if (healedAmount > 0)
                        {
                            var info = new HealedMonsterInfo(
                                monster.monsterData.monsterName,
                                healedAmount,
                                monster.CurHp,
                                monster.MaxHp,
                                monster.monsterData.monsterImage
                            );
                            ownedHealedList.Add(info);
                        }

                        Debug.Log($"[이벤트] {monster.monsterData.monsterName} 회복 완료 (+{healedAmount} HP / {monster.CurHp}/{monster.MaxHp})");
                    }
                }

                if (ownedHealedList.Count > 0)
                {
                    var popup = FindObjectOfType<HealingResultPopup>(true);
                    popup.Show(ownedHealedList);
                }

                break;
            case "EntryMonsters_Healing":
                List<HealedMonsterInfo> entryHealedList = new List<HealedMonsterInfo>();

                foreach (var monster in PlayerManager.Instance.player.entryMonsters)
                {
                    if (monster != null)
                    {
                        int beforeHp = monster.CurHp;

                        monster.HealFull(); // 전부 회복 시도

                        int healedAmount = monster.CurHp - beforeHp;

                        if (healedAmount > 0)
                        {
                            var info = new HealedMonsterInfo(
                                monster.monsterData.monsterName,
                                healedAmount,
                                monster.CurHp,
                                monster.MaxHp,
                                monster.monsterData.monsterImage
                            );
                            entryHealedList.Add(info);
                        }

                        Debug.Log($"[이벤트] {monster.monsterData.monsterName} 회복 완료 (+{healedAmount} HP / {monster.CurHp}/{monster.MaxHp})");
                    }
                }

                if (entryHealedList.Count > 0)
                {
                    var popup = FindObjectOfType<HealingResultPopup>(true);
                    popup.Show(entryHealedList);
                }

                break;
            case "ReInit_HealingShop":
                PlayerManager.Instance.playerController.AutoMove(Vector2.up, 0.5f, 3f, true);
                break;
            case "GetGesture_Scolding":
                PlayerManager.Instance.player.AddItem("호통치기", 1);
                Debug.Log("[이벤트] [호통치기] 제스처 획득");
                break;
            case "Check_GetGesture_Scolding":
                if (PlayerManager.Instance.player.HasItem("호통치기"))
                    StartDialogue("노인", currentNPCImage, 350);
                break;
            case "GetGesture_Ignoring":
                PlayerManager.Instance.player.AddItem("무시하기", 1);
                Debug.Log("[이벤트] [무시하기] 제스처 획득");
                break;
            case "GetGesture_Joking":
                PlayerManager.Instance.player.AddItem("농담하기", 1);
                Debug.Log("[이벤트] [농담하기] 제스처 획득");
                break;
            case "Check_GetGesture_Ignoring":
                if (PlayerManager.Instance.player.HasItem("무시하기"))
                    StartDialogue("토끼용사", currentNPCImage, 1020);
                break;
            case "MoveToPlayer":
                Debug.Log("[이벤트] 플레이어 위치로 이동");
                break;
            case "Quest_FindRegur":
                Debug.Log("[퀘스트 수주] 레거 찾기");
                UnknownForestManager.Instance.unknownForest.isQuest_FindRegueStarted = true;
                break;
            case "GameStart":
                Debug.Log("[이벤트] 게임 시작");
                PlayerManager.Instance.playerController.AutoMove(Vector2.right, 1.5f, 8f, false); // 오른쪽으로 2초간 이동
                break;
            case "HideDialogue":
                Debug.Log("[이벤트] 대화 UI 숨김");
                SetDialogueUI(false);
                break;
            case "ShowDialogue":
                Debug.Log("[이벤트] 대화 UI 표시");
                SetDialogueUI(true);
                break;
            case "HideNpcSprite":
                Debug.Log("[이벤트] NPC 스프라이트 숨김");
                dialogueUI.npcImage.gameObject.SetActive(false);
                break;
            case "ShowNpcSprite":
                Debug.Log("[이벤트] NPC 스프라이트 표시");
                dialogueUI.npcImage.gameObject.SetActive(true);
                break;

            case "Check_Quest_WanderingShop":
                if (!PlayerManager.Instance.player.playerQuestClearCheck[3])
                {
                    PlayerManager.Instance.player.playerQuestClearCheck[3] = true;
                    PlayerManager.Instance.player.playerQuestStartCheck[3] = false;
                    EventAlertManager.Instance.SetEventAlert(EventAlertType.QuestClear, null, "떠돌이 상인");
                }
                break;

            case "Quest_FIndCarpenter":
                PlayerManager.Instance.player.playerQuestStartCheck[2] = true;
                EventAlertManager.Instance.SetEventAlert(EventAlertType.QuestStart, null, "끊어진 다리");
                QuestEventDispatcher.OnQuestStarted?.Invoke(2);
                break;

            case "Quest_Tutorial":
                PlayerManager.Instance.player.playerQuestStartCheck[0] = true;
                EventAlertManager.Instance.SetEventAlert(EventAlertType.QuestStart, null, "전투의 기본");
                break;

            case "AddEntry_Kairen":
                Debug.Log("[이벤트] 카이렌을 엔트리 몬스터로 추가");
                Monster kairen = new Monster();
                kairen.SetMonsterData(BattleTutorialManager.Instance.reaward_Kairen);
                PlayerManager.Instance.player.AddOwnedMonster(kairen);
                PlayerManager.Instance.player.TryAddEntryMonster(kairen, (_, success) =>
                {
                    if (success != null)
                    {
                        PlayerManager.Instance.player.AddBattleEntry(kairen);
                    }
                    });
                BattleTutorialManager.Instance.AddMemberKairen();
                BattleManager.Instance.BattleEntryTeam.Add(kairen);
                BattleManager.Instance.StartBattle();
                BattleTutorialManager.Instance.InitAttackSelected();
                break;
            case "Inventory_Kairen":
                Debug.Log("[이벤트] 인벤토리 열기");
                BattleTutorialManager.Instance.InitInventorySelected();
                break;

            case "End_RunawayGuide":
                Debug.Log("도망가기 가이드 종료");
                BattleTutorialManager.Instance.InitEmbraceSelected();
                BattleTutorialManager.Instance.isBattleEmbraceTutorialStarded = true;
                break;
            case "End_BattleTutorial":
                Debug.Log("튜토리얼 종료");
                BattleTutorialManager.Instance.EndTutorial();
                break;
            case "Guide_MonsterOpen":
                Debug.Log("몬스터 가이드 열기");
                TutorialManager.Instance.Guide_MonsterOpen();
                break;
            case "Guide_EntryOpen":
                Debug.Log("엔트리 가이드 열기");
                TutorialManager.Instance.Guide_EntryOpen();
                break;
            case "Guide_BattleOpen":
                Debug.Log("전투 가이드 열기");
                TutorialManager.Instance.Guide_BattleOpen();
                break;
            case "Guide_SaveOpen":
                Debug.Log("저장 가이드 열기");
                TutorialManager.Instance.Guide_SaveOpen();
                break;
            case "Guide_MinimapOpen":
                Debug.Log("미니맵 가이드 열기");
                TutorialManager.Instance.Guide_MinimapOpen();
                break;
            case "End_AllTutorial":
                TutorialManager.Instance.TutorialCompeleted();
                break;
            case "UnKnownForest_OccurrenceNewEvent":
                if (UnknownForestManager.Instance.currentBush != null)
                {
                    UnknownForestManager.Instance.currentBush.OccurrenceNewEvent();
                }
                else
                {
                    Debug.LogWarning("현재 접촉 중인 수풀이 없습니다.");
                }
                break;
            case "UnKnownForest_GetItem":
                UnknownForestManager.Instance.currentBush.TryDropItem();
                break;
            case "UnKnownForest_Fight":
                Debug.Log("[이벤트] 미지의 숲에서 전투 시작");
                UnknownForestManager.Instance.currentBush.TryBattle();
                break;
            case "UnKnownForest_None":
                break;
            case "Quest_FindRegurInit":
                PlayerManager.Instance.playerController.isInputBlocked = true; // 플레이어 입력 차단
                bool hasLevel8OrAbove = PlayerManager.Instance.player.battleEntry.Any(monster => monster.Level >= 8);

                if (hasLevel8OrAbove)
                {
                    FadeManager.Instance.FadeOutThenIn(
                    1.5f,
                    () =>  // 어두울 때 실행
                    {
                    PlayerManager.Instance.playerController.transform.position = UnknownForestManager.Instance.playerRespawnTransporm.position;
                    },
                    () =>  // 밝아질 때 실행
                    {
                    //플레이어 x축플립
                   // PlayerManager.Instance.playerController.transform.localScale = new Vector3(-1.8f, 1.8f, 1.8f);
                    StartDialogue("핑거", currentNPCImage, 702);
                });
                }
                else
                {
                    StartDialogue("핑거", currentNPCImage, 663);
                }
                break;
            case "Move_InitUnknownForest":
                {
                    FadeManager.Instance.FadeOutThenIn(
                    1.5f,
                    () =>  // 어두울 때 실행
                    {
                        PlayerManager.Instance.playerController.transform.position = UnknownForestManager.Instance.playerRespawnTransporm.position;
                    },
                    () =>  // 밝아질 때 실행
                    {
                        //플레이어 x축플립
                        PlayerManager.Instance.playerController.transform.localScale = new Vector3(-1.8f, 1.8f, 1.8f);
                        PlayerManager.Instance.playerController.isInputBlocked = false; // 플레이어 입력 차단
                    });
                }
                break;
            case "Quest_FindRegurStarted":
                PlayerManager.Instance.playerController.isInputBlocked = false;
                EventAlertManager.Instance.SetEventAlert(EventAlertType.QuestStart, null, "레거의 편지");
                PlayerManager.Instance.player.playerQuestStartCheck[1] = true;
                QuestEventDispatcher.OnQuestStarted?.Invoke(1);
                break;
            case "Check_Quest_FindRegurStarted":
                if (PlayerManager.Instance.player.playerQuestStartCheck[1] == true)
                    StartDialogue("핑거", currentNPCImage, 780);
                break;
            case "Quest_FindRegurLetter":
                FadeManager.Instance.FadeOutThenIn(
                    1.5f,
                    () =>  // 어두울 때 실행
                    {
                        PlayerManager.Instance.playerController.transform.position = UnknownForestManager.Instance.playerRespawnTransporm.position;
                    },
                    () =>  // 밝아질 때 실행
                    {
                        StartDialogue("핑거", currentNPCImage, 730);
                    }
                );
                break;
            case "Quest_FindRegurLetterSended":
                PlayerManager.Instance.player.SendItem("레거의 편지", 1);
                break;
            case "GetItem_Potion_Meat":
                PlayerManager.Instance.player.AddItem("고기", 1);
                Debug.Log("[이벤트] 고기 아이템 획득");
                break;
            case "GetEquip_Armor":
                PlayerManager.Instance.player.AddItem("든든한 갑옷", 1);
                Debug.Log("[이벤트] 든든한 갑옷 아이템 획득");
                break;
            case "Quest_DisappearPinger":
                Destroy(UnknownForestManager.Instance.npc.gameObject);
                dialogueUI.npcImage.gameObject.SetActive(false);
                break;
            case "Quest_FindRegurLetterCleared":
                EventAlertManager.Instance.SetEventAlert(EventAlertType.QuestClear, null, "레거의 편지");
                PlayerManager.Instance.player.playerQuestClearCheck[1] = true;
                PlayerManager.Instance.player.playerQuestStartCheck[1] = false;
                Debug.Log("[레거의 편지] 퀘스트를 클리어하였습니다.");
                PlayerManager.Instance.playerController.isInputBlocked = false; // 플레이어 입력 차단
                break;
            case "Check_Quest_FindCapenter":
                if (PlayerManager.Instance.player.playerQuestStartCheck[2] == true)
                    StartDialogue("나", currentNPCImage, 1503);
                break;
            case "Move_FindCarpenter_Init":
                PlayerManager.Instance.playerController.AutoMove(Vector2.down, 0.5f, 3f, true);
                break;
            case "FightElite_Dean":
                FinalFightManager.Instance.Fight_Dean();
                break;
                case "Disappear_Dean":
                FadeManager.Instance.FadeOutThenIn(1f,() =>  // 어두울 때 실행
                {
                    Destroy(FinalFightManager.Instance.deanObj);
                },
                () =>  // 밝아질 때 실행
                {
                });
                break;
            case "FightElite_Eisen":
                FinalFightManager.Instance.Fight_Eisen();
                break;
            case "Disappear_Eisen":
                FadeManager.Instance.FadeOutThenIn(1f, () =>  // 어두울 때 실행
                {
                    Destroy(FinalFightManager.Instance.eisenObj);
                },
                () =>  // 밝아질 때 실행
                {
                });
                break;
            case "FightElite_Dolan":
                FinalFightManager.Instance.Fight_Dolan();
                break;
            case "Disappear_Dolan":
                FadeManager.Instance.FadeOutThenIn(1f, () =>  // 어두울 때 실행
                {
                    Destroy(FinalFightManager.Instance.dolanObj);
                },
                () =>  // 밝아질 때 실행
                {
                });
                break;
            case "FightElite_Boss":
                FinalFightManager.Instance.Fight_Boss();
                break;




            default:
                Debug.LogWarning($"[이벤트] 알 수 없는 이벤트 키: {eventKey}");
                break;
        }
    }
}

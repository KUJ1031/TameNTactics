using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    [SerializeField] private BattleSelectView battleSelectView;
    [SerializeField] private BattleInfoView battleInfoView;
    [SerializeField] private SkillView skillView;
    [SerializeField] private MenuView menuView;
    [SerializeField] private EmbraceView embraceView;

    [Header("포섭하기 미니게임")]
    [SerializeField] private GameObject miniGamePrefab;

    private bool isSkillPanelOpen = false;
    private Dictionary<Monster, GameObject> monsterBattleInfo = new();

    private Monster selectedMonster;

    void Start()
    {
        battleSelectView.attackButton.onClick.AddListener(OnAttackButtonClick);
        battleSelectView.embraceButton.onClick.AddListener(OnEmbraceButtonClick);
        battleSelectView.runButton.onClick.AddListener(OnRunButtonClick);
    }

    public void OnAttackButtonClick()
    {
        isSkillPanelOpen = true;
        battleSelectView.ShowSkillPanel();
        EventBus.OnAttackModeEnabled?.Invoke();
    }

    public void IntoBattleMenuSelect()
    {
        isSkillPanelOpen = false;
        battleSelectView.HideSkillPanel();
        EventBus.OnAttackModeDisabled?.Invoke();
    }

    // 포섭하기 버튼
    public void OnEmbraceButtonClick()
    {
        Debug.Log("포섭할 몬스터를 선택하세요.");
        StartCoroutine(WaitForMonsterSelection());
    }

    private IEnumerator WaitForMonsterSelection()
    {
        bool selected = false;
        selectedMonster = null;

        while (!selected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent<MonsterCharacter>(out var monsterCharacter))
                    {
                        Monster clickedMonster = monsterCharacter.monster;

                        if (!PlayerManager.Instance.player.ownedMonsters.Contains(clickedMonster))
                        {
                            selectedMonster = monsterCharacter.monster;
                            selected = true;
                            Debug.Log($"선택된 몬스터 : {selectedMonster.monsterName}");
                        }
                        else
                        {
                            Debug.Log("자신이 소유한 몬스터는 포섭할 수 없습니다.");
                        }
                    }
                }
            }
            yield return null;
        }

        embraceView.ShowGuide("스페이스바를 눌러 포섭을 시도하세요!");

        StartEmbraceMiniGame(selectedMonster, 60f);
    }

    private void StartEmbraceMiniGame(Monster targetMonster, float successPercent)
    {
        GameObject miniGameObj = Instantiate(miniGamePrefab);

        // UI 초기화
        embraceView.ShowGuide("스페이스바를 눌러 화살표를 멈추세요!");
        embraceView.HideMessage();

        // MiniGameManager 가져오기
        MiniGameManager miniGameManager = miniGameObj.GetComponent<MiniGameManager>();

        // MiniGame 시작
        miniGameManager.StartMiniGame(successPercent);

        // 결과 판정 코루틴 실행
        StartCoroutine(CheckEmbraceResult(miniGameManager, targetMonster));
    }

    private IEnumerator CheckEmbraceResult(MiniGameManager miniGameManager, Monster targetMonster)
    {
        RotatePoint rotatePoint = miniGameManager.GetComponentInChildren<RotatePoint>();

        bool finished = false;

        // 이 부분은 MiniGameManager의 Update로 이관 예정
        while (!finished)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rotatePoint.SetRotateSpeed(0);

                if (rotatePoint.isInSuccessZone)
                {
                    Debug.Log("포섭 성공!");
                    //PlayerManager.Instance.player.AddOwnedMonster(targetMonster);
                    BattleManager.Instance.CaptureSelectedEnemy(targetMonster);
                    embraceView.ShowSuccessMessage();
                }
                else
                {
                    Debug.Log("포섭 실패...!");
                    embraceView.ShowFailMessage();
                }

                finished = true;
            }

            yield return null;
        }

        yield return new WaitForSeconds(2f);

        Destroy(miniGameManager.gameObject);
    }

    // 내 몬스터 혹은 상대 몬스터 선택 시 강조 표시 이동
    public void SelectMonster()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<MonsterCharacter>(out var monsterCharacter))
                {
                    battleSelectView.MoveSelectMonster(monsterCharacter.transform);
                }
            }
            else return;
        }
    }

    public void ShowMonsterSkills(MonsterData monsterData)
    {
        if (monsterData == null || monsterData.skills == null) return;

        skillView.ShowSkillList(monsterData.skills);
    }


    // 배틀씬 진입 시 몬스터 체력, 궁극기 게이지 세팅
    public void SettingMonsterGauge(Transform ally, Transform enemy)
    {
        List<MonsterCharacter> monsterList = new();

        MonsterCharacter[] allyChildren = ally.GetComponentsInChildren<MonsterCharacter>();
        MonsterCharacter[] enemyChildren = enemy.GetComponentsInChildren<MonsterCharacter>();

        for (int i = 0; i < allyChildren.Length; i++)
        {
            monsterList.Add(allyChildren[i]);
        }

        for (int i = 0; i < enemyChildren.Length; i++)
        {
            monsterList.Add(enemyChildren[i]);
        }

        for (int i = 0; i < monsterList.Count; i++)
        {
            Debug.Log("게이지를 생성합니다.");
            Vector3 screenPos = Camera.main.WorldToScreenPoint(monsterList[i].transform.position);

            GameObject gauge = battleSelectView.InitiateGauge(screenPos);
            monsterBattleInfo.Add(monsterList[i].monster, gauge);
        }
    }

    public void UpdateHpGauge(Monster monster)
    {
        Debug.Log("UpdateHpGauge 진입");
        GameObject gauge = monsterBattleInfo[monster];

        float hpRatio = (float)monster.CurHp / monster.CurMaxHp;

        battleSelectView.SetHpGauge(gauge, hpRatio);
    }

    public void UpdateUltimateGauge(Monster monster)
    {
        Debug.Log("UpdateUltimateGauge 진입");
        GameObject gauge = monsterBattleInfo[monster];

        float ultimateRatio = (float)monster.CurUltimateCost / monster.MaxUltimateCost;

        battleSelectView.SetUltimateGauge(gauge, ultimateRatio);
    }

    // 배틀 중 전투 메세지를 받아올 메서드
    public void GetBattleDialogue(string message)
    {
        battleInfoView.BattleDialogue(message);
    }

    // 배틀 메세지 초기화
    public void ClearBattleDialogue()
    {
        battleInfoView.ClearBattleDialogue();
    }

    private void OnRunButtonClick()
    {
        bool success = BattleManager.Instance.TryRunAway();

        if (success)
        {
            Debug.Log("도망가기 성공! 이전 씬으로 돌아갑니다.");
            SceneManager.LoadScene("MainScene");
            RuntimePlayerSaveManager.Instance.RestoreGameState();
            StartCoroutine(DisableTriggerAfterLoadScene(3f));
        }
        else
        {
            Debug.Log("도망가기 실패!");
        }
    }

    private IEnumerator DisableTriggerAfterLoadScene(float disableTime)
    {
        yield return null;

        PlayerBattleTrigger trigger = FindObjectOfType<PlayerBattleTrigger>();
        if (trigger != null)
        {
            Debug.Log("트리거 있음");
            trigger.DisableTriggerTemporarily(disableTime);
        }
    }
}
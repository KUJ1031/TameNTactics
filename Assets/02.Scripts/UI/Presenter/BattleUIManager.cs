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

    public EmbraceView EmbraceView { get { return embraceView; } }
    public BattleSelectView BattleSelectView { get { return battleSelectView; } }

    [SerializeField] private BattleUIButtonHandler battleUIButtonHandler;

    [Header("포섭하기 미니게임")]
    [SerializeField] private GameObject miniGamePrefab;

    public GameObject MiniGamePrefab { get { return miniGamePrefab; } }

    private Dictionary<Monster, GameObject> monsterBattleInfo = new();

    public void OnAttackButtonClick()
    {
        battleSelectView.HideSelectPanel();
        EventBus.OnAttackModeEnabled?.Invoke();
    }

    public void IntoBattleMenuSelect()
    {
        battleSelectView.HideSkillPanel();
        EventBus.OnAttackModeDisabled?.Invoke();
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

    public void OffSelectMonsterUI()
    {
        battleSelectView.OffSelectMonster();
    }

    public void BattleEndMessage(bool isWin)
    {
        battleInfoView.ShowEndBattleMessage(isWin);
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    [SerializeField] private BattleSelectView battleSelectView;
    [SerializeField] private SkillView skillView;
    [SerializeField] private EntryView entryView;
    [SerializeField] private MenuView menuView;

    public BattleSystem battleSystem;

    private bool isSkillPanelOpen = false;
    private Dictionary<Monster, GameObject> monsterBattleInfo = new();

    void Start()
    {
        battleSelectView.attackButton.onClick.AddListener(OnAttackButtonClick);
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

    // 내 몬스터 혹은 상대 몬스터 선택 시 강조 표시 이동
    public void SelectMonster()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("다른 요소를 클릭함");
                return;
            }

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                MonsterCharacter monsterCharacter = hit.collider.GetComponent<MonsterCharacter>();

                if (monsterCharacter != null)
                {
                    battleSelectView.MoveSelectMonster(monsterCharacter.transform);
                }
            }
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

    public void UpdateGauge(Monster monster, SkillData skillData)
    {
        Debug.Log("UpdateGauge진입");
        GameObject gauge = monsterBattleInfo[monster];

        float hpRatio = (float)monster.CurHp / monster.MaxHp;
        float ultimateRatio = (float)skillData.curUltimateCost / skillData.maxUltimateCost;

        battleSelectView.SetGauge(gauge, hpRatio, ultimateRatio);
    }
}
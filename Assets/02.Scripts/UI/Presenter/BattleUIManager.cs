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
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    [SerializeField] private BattleSelectView battleSelectView;
    [SerializeField] private SkillView skillView;
    [SerializeField] private EntryView entryView;
    [SerializeField] private MenuView menuView;
    [SerializeField] private FieldInfoView fieldInfoView;

    //private List<Monster> playerMonsters = new();
    //private int currentIndex = 0;
    private bool isSkillPanelOpen = false;

    void Start()
    {
        battleSelectView.attackButton.onClick.AddListener(OnAttackButtonClick);
    }

    //void Update()
    //{
    //    HandleMouseClick();
    //    //HandleKeyboardInput();

    //    if (isSkillPanelOpen && Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        CloseSkillPanel();
    //    }
    //}

    //private void OnEnable()
    //{
    //    InitializePlayerMonsters();
    //}

    public void OnAttackButtonClick()
    {
        isSkillPanelOpen = true;
        battleSelectView.ShowSkillPanel();
        EventBus.OnAttackModeEnabled?.Invoke();
    }

    public void CloseSkillPanel()
    {
        isSkillPanelOpen = false;
        battleSelectView.HideSkillPanel();
        EventBus.OnAttackModeDisabled?.Invoke();
    }

    public void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                MonsterData monsterCharacter = hit.collider.GetComponent<MonsterCharacter>().monster.monsterData;

                if (monsterCharacter != null)
                {
                   // battleSelectView.MoveSelectMonster(hit.transform);
                    ShowMonsterSkills(monsterCharacter);
                }
            }
        }
    }

    //public void HandleKeyboardInput()
    //{
    //    if (playerMonsters.Count == 0) return;

    //    if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
    //    {
    //        currentIndex = (currentIndex + 1) % playerMonsters.Count;
    //        MoveSelectMonster(playerMonsters[currentIndex]);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
    //    {
    //        currentIndex = (currentIndex - 1 + playerMonsters.Count) % playerMonsters.Count;
    //        MoveSelectMonster(playerMonsters[currentIndex]);
    //    }
    //}

    //private void MoveSelectMonster(Transform tr)
    //{
    //    Vector3 screenPos = Camera.main.WorldToScreenPoint(tr.position);
    //    selectMonsterImage.position = screenPos;
    //    selectMonsterImage.gameObject.SetActive(true);
    //}

    public void ShowMonsterSkills(MonsterData monsterData)
    {
        if (monsterData == null || monsterData.skills == null) return;

        skillView.ShowSkillList(monsterData.skills);
    }

    //private void InitializePlayerMonsters()
    //{
    //    playerMonsters.Clear();

    //    var player = PlayerManager.Instance?.player;
    //    if (player == null)
    //    {
    //        selectMonsterImage.gameObject.SetActive(false);
    //        return;
    //    }

    //    // player.battleEntry가 List<Monster>가 되어야 함
    //    var battleEntries = player.battleEntry;

    //    // 직접 player.battleEntry를 복사해서 playerMonsters에 넣기
    //    playerMonsters.AddRange(battleEntries);

    //    if (playerMonsters.Count > 0)
    //    {
    //        currentIndex = Mathf.Clamp(currentIndex, 0, playerMonsters.Count - 1);
    //        MoveSelectMonster(playerMonsters[currentIndex]);
    //    }
    //    else
    //    {
    //        selectMonsterImage.gameObject.SetActive(false);
    //    }
    //}
}

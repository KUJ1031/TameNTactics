using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleSelectPresent : MonoBehaviour
{
    [SerializeField] private BattleSelectView battleSelectView;
    [SerializeField] private RectTransform selectMonsterImage;

    private List<Monster> playerMonsters = new List<Monster>();
    private int currentIndex = 0;
    private bool isSkillPanelOpen = false;

    public event Action OnAttackModeEnabled;
    public event Action OnAttackModeDisabled;

    void Start()
    {
        battleSelectView.attackButton.onClick.AddListener(OnAttackButtonClick);
    }

    void Update()
    {
        HandleMouseClick();
        HandleKeyboardInput();

        if (isSkillPanelOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseSkillPanel();
        }
    }

    private void OnEnable()
    {
        InitializePlayerMonsters();
    }

    private void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Monster monster = hit.collider.GetComponent<Monster>();
                if (monster != null && playerMonsters.Contains(monster))
                {
                    currentIndex = playerMonsters.IndexOf(monster);
                    MoveSelectMonster(monster);
                    BattleManager.Instance.SelectPlayerMonster(monster);
                }
            }
        }
    }

    private void HandleKeyboardInput()
    {
        if (playerMonsters.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            currentIndex = (currentIndex + 1) % playerMonsters.Count;
            MoveSelectMonster(playerMonsters[currentIndex]);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            currentIndex = (currentIndex - 1 + playerMonsters.Count) % playerMonsters.Count;
            MoveSelectMonster(playerMonsters[currentIndex]);
        }
    }

    private void MoveSelectMonster(Monster monster)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(monster.transform.position);
        selectMonsterImage.position = screenPos;
        selectMonsterImage.gameObject.SetActive(true);
    }

    private void OnAttackButtonClick()
    {
        isSkillPanelOpen = true;
        battleSelectView.ShowSkillPanel();
        OnAttackModeEnabled?.Invoke();
    }

    private void CloseSkillPanel()
    {
        isSkillPanelOpen = false;
        battleSelectView.HideSkillPanel();
        OnAttackModeDisabled?.Invoke();
    }

    private void InitializePlayerMonsters()
    {
        playerMonsters.Clear();

        var player = PlayerManager.Instance?.player;
        if (player == null)
        {
            selectMonsterImage.gameObject.SetActive(false);
            return;
        }

        // player.battleEntry가 List<Monster>가 되어야 함
        var battleEntries = player.battleEntry;

        // 직접 player.battleEntry를 복사해서 playerMonsters에 넣기
        playerMonsters.AddRange(battleEntries);

        if (playerMonsters.Count > 0)
        {
            currentIndex = Mathf.Clamp(currentIndex, 0, playerMonsters.Count - 1);
            MoveSelectMonster(playerMonsters[currentIndex]);
        }
        else
        {
            selectMonsterImage.gameObject.SetActive(false);
        }
    }
}

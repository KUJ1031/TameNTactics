using System.Collections;
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


    void Start()
    {
        battleSelectView.attackButton.onClick.AddListener(OnAttackButtonClick);
    }
    void Update()
    {
        HandleMouseClick();
        HandleKeyboardInput();

        if (isSkillPanelOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseSkillPanel();
            }
        }
    }

    private void OnEnable()
    {
        // EntryManager 대신 PlayerManager.player에 직접 접근
        // Player 쪽에 OnEntryChanged 이벤트가 없으면 직접 초기화 호출
        InitializePlayerMonsters();

        // 만약 Player 클래스에 OnEntryChanged 이벤트가 있다면
        // PlayerManager.Instance.player.OnEntryChanged += InitializePlayerMonsters;
    }

    private void OnDisable()
    {
        // PlayerManager.Instance.player.OnEntryChanged -= InitializePlayerMonsters;
    }


    private void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("몬스터 클릭 진입");
                Monster monster = hit.collider.GetComponent<Monster>();
                //if (monster != null && playerMonsters.Contains(monster))
                if (monster != null)
                {
                    currentIndex = playerMonsters.IndexOf(monster);
                    MoveSelectMonster(monster);
                    BattleManager.Instance.SelectPlayerMonster(monster.monsterData);
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
    }

    private void CloseSkillPanel()
    {
        isSkillPanelOpen = false;
        battleSelectView.HideSkillPanel();
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

        var battleEntries = player.battleEntry; // 전투 출전 멤버 리스트

        Monster[] allMonsters = FindObjectsOfType<Monster>();

        foreach (var monster in allMonsters)
        {
            if (battleEntries.Contains(monster.GetData()))
            {
                playerMonsters.Add(monster);
            }
        }

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

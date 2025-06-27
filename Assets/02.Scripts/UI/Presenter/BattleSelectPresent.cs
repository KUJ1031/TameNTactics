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

    // Start is called before the first frame update
    void Start()
    {
        battleSelectView.attackButton.onClick.AddListener(OnAttackButtonClick);
    }

    // Update is called once per frame
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
        EntryManager.Instance.OnEntryChanged += InitializePlayerMonsters;
    }

    private void OnDisable()
    {
        EntryManager.Instance.OnEntryChanged -= InitializePlayerMonsters;
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
                }
            }
        }
    }

    // 키보드 입력에 따른 선택 몬스터 UI 이동
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

    // 선택한 몬스터 강조하는 UI
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

        List<MonsterData> selectedEntries = EntryManager.Instance.selectedEntries;
        Monster[] allMonsters = FindObjectsOfType<Monster>();

        foreach (var monster in allMonsters)
        {
            if (selectedEntries.Contains(monster.GetData()))
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

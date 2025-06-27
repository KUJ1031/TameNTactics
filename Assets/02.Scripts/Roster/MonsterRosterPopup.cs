using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterRosterPopup : MonoBehaviour
{
    public static MonsterRosterPopup Instance;

    [Header("UI 연결")]
    public GameObject popupPanel;
    public Image monsterImage;
    public TextMeshProUGUI monsterNameText;

    public Button addCandidateBtn;   // 출전 후보군 추가 (entryMonsters)
    public Button removeCandidateBtn;// 출전 후보군 제거
    public Button releaseBtn;        // 방출 (ownedMonsters)

    private MonsterData currentMonster;

    private Player player;

    private void Awake()
    {
        Instance = this;
        popupPanel.SetActive(false);

        addCandidateBtn.onClick.AddListener(OnClickAddCandidate);
        removeCandidateBtn.onClick.AddListener(OnClickRemoveCandidate);
        releaseBtn.onClick.AddListener(OnClickRelease);

        // Player 참조 확보
        player = PlayerManager.Instance?.player;
        if (player == null)
        {
            Debug.LogWarning("Player가 할당되지 않았습니다. PlayerManager와 Player가 올바르게 초기화 되었는지 확인하세요.");
        }
    }

    public void Open(MonsterData monster)
    {
        currentMonster = monster;

        monsterImage.sprite = monster.monsterImage;
        monsterNameText.text = monster.monsterName;

        UpdateButtons();

        popupPanel.SetActive(true);
    }

    public void Close()
    {
        popupPanel.SetActive(false);
    }

    void UpdateButtons()
    {
        if (player == null)
        {
            addCandidateBtn.gameObject.SetActive(false);
            removeCandidateBtn.gameObject.SetActive(false);
            return;
        }

        bool isCandidate = player.entryMonsters.Contains(currentMonster);

        addCandidateBtn.gameObject.SetActive(!isCandidate);
        removeCandidateBtn.gameObject.SetActive(isCandidate);
    }

    void OnClickAddCandidate()
    {
        if (player == null) return;

        bool added = EntryManager.Instance.ToggleCandidate(currentMonster);
        if (added)
            Debug.Log($"{currentMonster.monsterName} 후보군에 추가됨");
        else
            Debug.LogWarning("추가 실패: 최대치일 가능성 있음");

        UpdateButtons();
    }

    void OnClickRemoveCandidate()
    {
        if (player == null) return;

        bool removed = EntryManager.Instance.ToggleCandidate(currentMonster);
        if (!player.entryMonsters.Contains(currentMonster))
            Debug.Log($"{currentMonster.monsterName} 후보군에서 제거됨");

        UpdateButtons();
    }

    void OnClickRelease()
    {
        if (player == null) return;

        player.ownedMonsters.Remove(currentMonster);
        MonsterRosterManager.Instance.InitializeRoster();  // UI 갱신
        Close();
    }
}

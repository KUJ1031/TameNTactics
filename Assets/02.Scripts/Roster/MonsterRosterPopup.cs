using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class MonsterRosterPopup : MonoBehaviour
{
    public static MonsterRosterPopup Instance;

    [Header("UI 연결")]
    public GameObject popupPanel;
    public Image monsterImage;
    public TextMeshProUGUI monsterNameText;

    public Button addCandidateBtn;   // 출전 후보군 추가
    public Button removeCandidateBtn;// 출전 후보군 제거
    public Button releaseBtn;        // 방출

    private Monster currentMonster;
    private Player player;

    private void Awake()
    {
        Instance = this;
        popupPanel.SetActive(false);

        addCandidateBtn.onClick.AddListener(OnClickAddCandidate);
        removeCandidateBtn.onClick.AddListener(OnClickRemoveCandidate);
        releaseBtn.onClick.AddListener(OnClickRelease);

        player = PlayerManager.Instance?.player;
        if (player == null)
        {
            Debug.LogWarning("Player가 할당되지 않았습니다. PlayerManager와 Player가 올바르게 초기화 되었는지 확인하세요.");
        }
    }

    /// <summary>
    /// 몬스터 정보 팝업 열기 (Monster 객체를 받음)
    /// </summary>
    public void Open(Monster monster)
    {
        if (monster == null)
        {
            Debug.LogWarning("Open 호출 시 monster가 null입니다.");
            return;
        }

        currentMonster = monster;

        monsterImage.sprite = monster.monster.monsterImage;
        monsterNameText.text = monster.monster.monsterName;

        UpdateButtons();

        popupPanel.SetActive(true);
    }

    public void Close()
    {
        popupPanel.SetActive(false);
    }

    /// <summary>
    /// 출전 후보군 상태에 따라 버튼 활성화 상태 업데이트
    /// </summary>
    private void UpdateButtons()
    {
        if (player == null || currentMonster == null)
        {
            SetButtonsActive(false, false);
            return;
        }

        bool isCandidate = player.entryMonsters.Any(m => m == currentMonster);

        SetButtonsActive(!isCandidate, isCandidate);
    }

    private void SetButtonsActive(bool addActive, bool removeActive)
    {
        addCandidateBtn.gameObject.SetActive(addActive);
        removeCandidateBtn.gameObject.SetActive(removeActive);
    }

    private void OnClickAddCandidate()
    {
        if (player == null || currentMonster == null) return;

        bool added = EntryManager.Instance.ToggleCandidate(currentMonster);
        if (added)
            Debug.Log($"{currentMonster.monster.monsterName} 후보군에 추가됨");
        else
            Debug.LogWarning("추가 실패: 최대치일 가능성 있음");

        UpdateButtons();
    }

    private void OnClickRemoveCandidate()
    {
        if (player == null || currentMonster == null) return;

        bool removed = EntryManager.Instance.ToggleCandidate(currentMonster);
        if (!player.entryMonsters.Contains(currentMonster))
            Debug.Log($"{currentMonster.monster.monsterName} 후보군에서 제거됨");

        UpdateButtons();
    }

    private void OnClickRelease()
    {
        if (player == null || currentMonster == null) return;

        if (player.ownedMonsters.Remove(currentMonster))
        {
            Debug.Log($"{currentMonster.monster.monsterName} 방출됨");
            MonsterRosterManager.Instance.InitializeRoster();  // UI 갱신
            Close();
        }
        else
        {
            Debug.LogWarning("몬스터 방출 실패: 소유 몬스터 리스트에 없음");
        }
    }
}

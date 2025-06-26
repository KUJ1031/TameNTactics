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

    public Button addCandidateBtn;   // 출전 후보군 추가
    public Button removeCandidateBtn;// 출전 후보군 제거
    public Button releaseBtn;        // 방출

    private MonsterData currentMonster;

    private void Awake()
    {
        Instance = this;
        popupPanel.SetActive(false);

        addCandidateBtn.onClick.AddListener(OnClickAddCandidate);
        removeCandidateBtn.onClick.AddListener(OnClickRemoveCandidate);
        releaseBtn.onClick.AddListener(OnClickRelease);
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
        var entry = EntryManager.Instance;

        bool isCandidate = entry.allMonsters.Contains(currentMonster);

        addCandidateBtn.gameObject.SetActive(!isCandidate);
        removeCandidateBtn.gameObject.SetActive(isCandidate);
    }

    void OnClickAddCandidate()
    {
        bool success = EntryManager.Instance.AddCandidate(currentMonster);
        if (success)
        {
            Debug.Log($"{currentMonster.monsterName} 후보군에 추가됨");
        }
        else
        {
            Debug.LogWarning("출전 후보군 추가 실패");
        }
        UpdateButtons();
    }

    void OnClickRemoveCandidate()
    {
        EntryManager.Instance.RemoveCandidate(currentMonster);
        UpdateButtons();
    }

    void OnClickRelease()
    {
        MonsterRosterManager.Instance.RemoveMonster(currentMonster);
        Close();
    }
}

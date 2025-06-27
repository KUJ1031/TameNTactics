using System.Collections.Generic;
using UnityEngine;

public class MonsterRosterManager : MonoBehaviour
{
    public static MonsterRosterManager Instance;

    [Header("UI 세팅")]
    public Transform rosterContent;
    public GameObject rosterSlotPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializeRoster();
    }

    /// <summary>
    /// 플레이어가 소유한 몬스터 리스트를 가져옵니다.
    /// </summary>
    private List<MonsterData> GetOwnedMonsters()
    {
        var player = PlayerManager.Instance?.player;
        if (player == null)
        {
            Debug.LogWarning("플레이어 데이터가 없습니다.");
            return new List<MonsterData>();
        }
        return player.ownedMonsters;
    }

    /// <summary>
    /// UI 목록 초기화
    /// </summary>
    public void InitializeRoster()
    {
        foreach (Transform child in rosterContent)
            Destroy(child.gameObject);

        var ownedMonsters = GetOwnedMonsters();

        foreach (var monster in ownedMonsters)
        {
            GameObject slotObj = Instantiate(rosterSlotPrefab, rosterContent);
            var slot = slotObj.GetComponent<MonsterRosterSlot>();
            slot.SetData(monster);
        }
    }

    /// <summary>
    /// 몬스터 추가 (예: 게임 데이터에서 불러올 때)
    /// 플레이어의 ownedMonsters에 추가 후 UI 갱신
    /// </summary>
    public void AddMonster(MonsterData monster)
    {
        var player = PlayerManager.Instance?.player;
        if (player == null) return;

        if (!player.ownedMonsters.Contains(monster))
        {
            player.ownedMonsters.Add(monster);
            InitializeRoster();
        }
    }

    /// <summary>
    /// 몬스터 제거 (방출)
    /// 플레이어의 ownedMonsters에서 제거 후 출전 후보군에서도 제거
    /// </summary>
    public void RemoveMonster(MonsterData monster)
    {
        var player = PlayerManager.Instance?.player;
        if (player == null) return;

        if (player.ownedMonsters.Contains(monster))
        {
            player.ownedMonsters.Remove(monster);

            // 출전 후보군에서도 제거 시도
            EntryManager.Instance.ToggleCandidate(monster);

            InitializeRoster();
        }
    }

    /// <summary>
    /// 출전 후보군에 등록 요청 (EntryManager의 AddCandidate 호출)
    /// </summary>
    public bool RequestAddToEntryCandidates(MonsterData monster)
    {
        return EntryManager.Instance.ToggleCandidate(monster);
    }
}

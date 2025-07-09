using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OwnedMonsterUIManager : MonoBehaviour
{
    public static OwnedMonsterUIManager Instance { get; private set; }
    [SerializeField] private OwnedMonsterUI ownedMonsterUI;

    [SerializeField] private Transform ownedParent;             //owned슬롯이 만들어질 위치
    [SerializeField] private GameObject ownedMonsterSlotPrefab; //owned슬롯 프리팹
    private List<OwnedMonsterSlot> ownedSlotUIList = new();     //만들어진 owned슬롯들
    private OwnedMonsterSlot selectedSlot;                      //선택된 owned슬롯

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        RefreshOwnedMonsterUI();
    }

    //Owned창 새로고침
    public void RefreshOwnedMonsterUI()
    {
        List<Monster> sortedMonsters = GetSortedOwnedMonsters();
        EnsureSlotCount(sortedMonsters.Count);
        UpdateSlotData(sortedMonsters);

        //선택된 슬롯 유지
        if (selectedSlot != null && selectedSlot.gameObject.activeInHierarchy)
        {
            selectedSlot.SetSelected(true);
        }
    }

    //몬스터 정렬
    private List<Monster> GetSortedOwnedMonsters()
    {
        List<Monster> ownedMonsters = PlayerManager.Instance.player.ownedMonsters;
        List<Monster> entry = PlayerManager.Instance.player.entryMonsters;

        return ownedMonsters
            .OrderByDescending(mon => entry.Contains(mon))  // 엔트리 우선
            .ThenByDescending(mon => mon.IsFavorite)        // 즐겨찾기 우선
            .ThenBy(mon => mon.monsterName)                 // 이름 오름차순
            .ToList();
    }

    //슬롯생성
    private void EnsureSlotCount(int requiredCount)
    {
        while (ownedSlotUIList.Count < requiredCount)
        {
            GameObject go = Instantiate(ownedMonsterSlotPrefab, ownedParent);
            var slot = go.GetComponent<OwnedMonsterSlot>();
            ownedSlotUIList.Add(slot);
        }
    }

    //슬롯에 데이터넣기
    private void UpdateSlotData(List<Monster> monsters)
    {
        for (int i = 0; i < ownedSlotUIList.Count; i++)
        {
            if (i < monsters.Count)
            {
                ownedSlotUIList[i].gameObject.SetActive(true);
                ownedSlotUIList[i].Setup(monsters[i]);
            }
            else
            {
                ownedSlotUIList[i].gameObject.SetActive(false);
            }
        }
    }

    //몬스터 슬롯 선택
    public void SelectMonsterSlot(OwnedMonsterSlot slot)
    {
        //중복선택시
        if (selectedSlot == slot)
        {
            ownedMonsterUI.SetLogoVisibility(true);
            selectedSlot.SetSelected(false);
            selectedSlot = null;
            return;
        }

        //이전 선택 해제
        if (selectedSlot != null)
            selectedSlot.SetSelected(false);

        //새 선택 적용
        selectedSlot = slot;
        selectedSlot.SetSelected(true);

        //OwnedMonsterUI 세팅
        Monster monster = selectedSlot.GetMonster();
        ownedMonsterUI.SetSimpleMonsterUI(monster);
        ownedMonsterUI.SetMonsterDetailUIButtons(monster);
    }

    //슬롯 마크 갱신
    public void RefreshSlotFor(Monster monster)
    {
        foreach (var slot in ownedSlotUIList)
        {
            if (slot.GetMonster() == monster)
            {
                slot.RefreshSlot(monster);
                break;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OwnedMonsterUIManager : MonoBehaviour
{
    public static OwnedMonsterUIManager Instance { get; private set; }

    [SerializeField] private Transform owmedParent;             //owmed슬롯이 만들어질 위치
    [SerializeField] private GameObject owmedMonsterSlotPrefab; //owmed슬롯 프리팹
    private List<OwnedMonsterSlot> ownedSlotUIList = new();     //만들어진 owmed슬롯들
    private OwnedMonsterSlot selectedSlot;                      //선택된 owmed슬롯

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void RefreshOwnedMonsterUI()
    {
        List<Monster> ownedMonsters = PlayerManager.Instance.player.ownedMonsters;
        List<Monster> entry = PlayerManager.Instance.player.entryMonsters;

        //몬스터 리스트 정렬
        ownedMonsters = ownedMonsters
            .OrderByDescending(mon => entry.Contains(mon))  //엔트리 우선
            .ThenByDescending(mon => mon.IsFavorite)        //즐겨찾기 우선
            .ThenBy(mon => mon.monsterName)                 //이름 오름차순
            .ToList();

        //이전 슬롯개수과 다르면 생성
        while (ownedSlotUIList.Count < ownedMonsters.Count)
        {
            GameObject go = Instantiate(owmedMonsterSlotPrefab, owmedParent);
            var slot = go.GetComponent<OwnedMonsterSlot>();
            ownedSlotUIList.Add(slot);
        }

        //슬롯 데이터 주입 및 남는것 숨기기 처리
        for (int i = 0; i < ownedSlotUIList.Count; i++)
        {
            if (i < ownedMonsters.Count)
            {
                ownedSlotUIList[i].gameObject.SetActive(true);
                ownedSlotUIList[i].Setup(ownedMonsters[i]);
            }
            else
            {
                ownedSlotUIList[i].gameObject.SetActive(false);  // 남는 슬롯은 비활성화
            }
        }
    }

    //몬스터 슬롯 선택
    public void SelectMonsterSlot(OwnedMonsterSlot slot)
    {
        //이전 선택 해제
        if (selectedSlot != null)
            selectedSlot.SetSelected(false);

        //새 선택 적용
        selectedSlot = slot;
        selectedSlot.SetSelected(true);

        //OwnedMonsterUI에 SimpleMonsterInfo 세팅
        Monster monster = selectedSlot.GetMonster();

    }
}

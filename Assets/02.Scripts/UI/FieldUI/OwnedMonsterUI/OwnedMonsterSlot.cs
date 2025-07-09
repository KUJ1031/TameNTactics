using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OwnedMonsterSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Outline outline;        //외각선(선택표시)
    [SerializeField] private GameObject favoriteMark;   //즐겨찾기 표시
    [SerializeField] private GameObject entryMark;      //엔트리 표시

    private Monster OwnedSlotMonster;

    //생성시 기초 세팅
    public void Setup(Monster monster)
    {
        OwnedSlotMonster = monster;
        outline.enabled = false; // 초기엔 선택 안됨
        RefreshSlot(monster);
        gameObject.GetComponent<Image>().sprite = monster.monsterData.monsterImage;
    }

    //몬스터 마크 갱신
    public void RefreshSlot(Monster monster)
    {
        SetFavoriteMark(monster.IsFavorite);

        List<Monster> entry = PlayerManager.Instance.player.entryMonsters;
        bool isEntry = entry.Contains(monster);
        SetEntryMark(isEntry);
    }

    //몬스터 외각선 변경
    public void SetSelected(bool isSelected)
    {
        outline.enabled = isSelected;
    }

    //몬스터 즐겨찾기 마크변경
    private void SetFavoriteMark(bool isFavorite)
    {
        favoriteMark.SetActive(isFavorite);
    }

    //몬스터 엔트리 마크변경
    private void SetEntryMark(bool isEntry)
    {
        entryMark.SetActive(isEntry);
    }

    //클릭시 선택됨 정보 전달
    public void OnPointerClick(PointerEventData eventData)
    {
        OwnedMonsterUIManager.Instance.SelectMonsterSlot(this);
    }

    //슬롯이 가진 몬스터 리턴
    public Monster GetMonster()
    {
        return OwnedSlotMonster;
    }
}

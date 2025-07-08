using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OwnedMonsterSlot : MonoBehaviour
{
    [SerializeField] private GameObject outline;        //외각선(선택표시)
    [SerializeField] private GameObject favoriteMark;   //즐겨찾기 표시
    [SerializeField] private GameObject entryMark;      //엔트리 표시

    private Monster OwnedSlotMonsterData;

    //생성시 기초 세팅
    public void Setup(Monster monster)
    {
        OwnedSlotMonsterData = monster;
        outline.SetActive(false); // 초기엔 선택 안됨
    }

    //몬스터 외각선 변경
    public void SetSelected(bool isSelected)
    {
        outline.SetActive(isSelected);
    }

    //몬스터 즐겨찾기 마크변경
    public void SetFavoriteMark(bool isFavorite)
    {
        favoriteMark.SetActive(isFavorite);
    }

    //몬스터 엔트리 마크변경
    public void SetEntryMark(bool isEntry)
    {
        entryMark.SetActive(isEntry);
    }

    //클릭시 선택됨 정보 전달
    public void OnClick()
    {
        OwnedMonsterUIManager.Instance.SelectMonsterSlot(this);
    }

    //슬롯이 가진 몬스터 리턴
    public Monster GetMonster()
    {
        return OwnedSlotMonsterData;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OwnedMonsterSlot : MonoBehaviour
{
    [SerializeField] private GameObject outline; //외각선(선택표시)
    private Monster monster;

    //생성시 기초 세팅
    public void Setup(Monster monsterData)
    {
        monster = monsterData;
        outline.SetActive(false); // 초기엔 선택 안됨
    }

    //몬스터 외각선 변경
    public void SetSelected(bool isSelected)
    {
        outline.SetActive(isSelected);
    }
    
    //클릭시 선택됨 정보 전달
    public void OnClick()
    {
        //FieldUIManager.Instance.SelectOwnedMonsterSlot(this);
    }

    //슬롯이 가진 몬스터 리턴
    public Monster GetMonster()
    {
        return monster;
    }
}

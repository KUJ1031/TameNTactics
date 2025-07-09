using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerEntrySwapPopupSlot : MonoBehaviour
{
    [SerializeField] private Image monsterImage;
    [SerializeField] private Outline outline;

    private Monster monster;
    public System.Action OnClick;

    public void Setup(Monster mon)
    {
        monster = mon;
        monsterImage.sprite = mon.monsterData.monsterImage;
        SetSelected(false);
    }

    //슬롯 선택
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }

    public void SetSelected(bool isSelected)
    {
        outline.enabled = isSelected;
    }

    public Monster GetMonster() => monster;
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MonsterRosterSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("UI 연결")]
    public Image monsterImage;

    private Monster monster; // MonsterData 대신 Monster
    public bool IsEmpty { get; private set; } = true;

    public void SetData(Monster data)
    {
        monster = data;
        monsterImage.sprite = data.monsterData.monsterImage; // Monster 안의 MonsterData 접근
        monsterImage.enabled = true;
        IsEmpty = false;
    }

    public void SetEmpty()
    {
        monster = null;
        monsterImage.sprite = null;
        monsterImage.enabled = false;
        IsEmpty = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsEmpty) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // MonsterRosterPopup도 Monster를 받도록 수정 필요
            MonsterRosterPopup.Instance.Open(monster);
        }
    }
}

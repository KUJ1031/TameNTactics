using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MonsterRosterSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("UI 연결")]
    public Image monsterImage;

    private MonsterData monsterData;
    public bool IsEmpty { get; private set; } = true;

    public void SetData(MonsterData data)
    {
        monsterData = data;
        monsterImage.sprite = data.monsterImage;
        monsterImage.enabled = true;
        IsEmpty = false;
    }

    public void SetEmpty()
    {
        monsterData = null;
        monsterImage.sprite = null;
        monsterImage.enabled = false;
        IsEmpty = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsEmpty) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            MonsterRosterPopup.Instance.Open(monsterData);
        }
    }
}

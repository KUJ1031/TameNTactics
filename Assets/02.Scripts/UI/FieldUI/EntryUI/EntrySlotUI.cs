using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntrySlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image monsterImage;
    [SerializeField] private GameObject monsterDatas;
    [SerializeField] private TextMeshProUGUI monsterLevel;
    [SerializeField] private TextMeshProUGUI monsterHP;
    [SerializeField] private Image HPBar;
    [SerializeField] private TextMeshProUGUI monsterExp;
    [SerializeField] private Image ExpBar;
    [SerializeField] private Outline outline;

    private Monster monster;

    public void Init(Monster monster)
    {
        GetComponent<Image>().raycastTarget = true;
        this.monster = monster;
        monsterImage.gameObject.SetActive(true);
        monsterDatas.SetActive(true);
        monsterImage.sprite = monster.monsterData.monsterImage;
        monsterLevel.text = "Lv."+monster.Level.ToString();
        monsterHP.text = $"{monster.CurHp}/{monster.MaxHp}";
        monsterExp.text = $"{monster.CurExp}/{monster.MaxExp}";
        HPBar.fillAmount = monster.CurHp / monster.MaxHp;
        ExpBar.fillAmount = monster.CurExp / monster.MaxExp;
    }

    public void VoidSlotInit()
    {
        GetComponent<Image>().raycastTarget = false;
        monsterImage.gameObject.SetActive(false);
        monsterDatas.SetActive(false);
    }

    //선택표시(아웃라인)
    public void SetSelected(bool isSelected)
    {
        outline.enabled = isSelected;
    }


    //슬롯 몬스터 정보 얻기
    public Monster GetMonster() { return monster; }

    //슬롯 클릭 시
    public void OnPointerClick(PointerEventData eventData)
    {
        EntryUIManager.Instance.SelectSlot(this);
    }
}

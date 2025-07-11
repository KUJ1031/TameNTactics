using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntrySlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Image monsterImage;
    [SerializeField] private GameObject monsterDatas;
    [SerializeField] private TextMeshProUGUI monsterLevel;
    [SerializeField] private TextMeshProUGUI monsterHP;
    [SerializeField] private Image HPBar;
    [SerializeField] private TextMeshProUGUI monsterExp;
    [SerializeField] private Image ExpBar;
    [SerializeField] private Outline outline;

    [Header("드래그용")]
    [SerializeField] private RectTransform rect;
    [SerializeField] private CanvasGroup canvasGroup;

    private Transform canvas;
    private Transform previousParent;

    private Monster monster;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>().transform;
    }

    public void Init(Monster monster)
    {
        GetComponent<Image>().raycastTarget = true;
        this.monster = monster;
        monsterImage.gameObject.SetActive(true);
        monsterDatas.SetActive(true);

        monsterImage.sprite = monster.monsterData.monsterImage;
        monsterLevel.text = $"Lv.{monster.Level}";
        monsterHP.text = $"{monster.CurHp}/{monster.MaxHp}";
        monsterExp.text = $"{monster.CurExp}/{monster.MaxExp}";
        HPBar.fillAmount = monster.CurHp / monster.MaxHp;
        ExpBar.fillAmount = monster.CurExp / monster.MaxExp;
    }
    //빈칸 생성
    public void VoidSlotInit()
    {
        GetComponent<Image>().raycastTarget = false;
        monsterImage.gameObject.SetActive(false);
        monsterDatas.SetActive(false);
    }
    //선택시 자기자신 내보내기
    public void SetSelected(bool isSelected)
    {
        outline.enabled = isSelected;
    }

    public Monster GetMonster() => monster;

    public void OnPointerClick(PointerEventData eventData)
    {
        EntryUIManager.Instance.SelectSlot(this);
    }

    //드래그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        previousParent = transform.parent;
        transform.SetParent(canvas);
        transform.SetAsLastSibling();

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }
    //드래그 중
    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }
    //드래그 끝
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        Transform dropTarget = EntryUIManager.Instance.GetDropTarget(eventData.position);
        if (dropTarget != null)
        {
            int insertIndex = EntryUIManager.Instance.GetInsertIndex(dropTarget, eventData.position);
            transform.SetParent(dropTarget);
            transform.SetSiblingIndex(insertIndex);

            EntryUIManager.Instance.OnDrop(this, dropTarget);
        }
        else
        {
            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponent<RectTransform>().position;
        }
    }
}

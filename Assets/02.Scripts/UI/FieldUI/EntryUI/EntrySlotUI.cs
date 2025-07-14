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

    //클릭
    public void OnPointerClick(PointerEventData eventData)
    {
        EntryUIManager.Instance.SelectSlot(this);
    }

    //드래그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        //드래그 가능한지 확인
        if (monster == null)
        {
            eventData.pointerDrag = null;
            return;
        }

        //시작 부모 저장(드롭할 곳이 없을경우 돌아가기 위함)
        previousParent = transform.parent;
        
        //부모를 맨 위 캔버스로 지정후 맨 아래 요소로(다른 요소보다 앞으로 오기위해)
        transform.SetParent(canvas);
        transform.SetAsLastSibling();

        //반투명 및 클릭 막기
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    //드래그 중
    public void OnDrag(PointerEventData eventData)
    {
        //슬롯의 위치를 마우스로
        rect.position = eventData.position;
        
    }

    //드래그 끝
    public void OnEndDrag(PointerEventData eventData)
    {
        //반투명 및 클릭 막기 초기화
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        //드랍 될 부모 판단
        Transform dropTarget = EntryUIManager.Instance.GetDropTarget(eventData.position);
        if (dropTarget != null)
        {
            int insertIndex = EntryUIManager.Instance.GetInsertIndex(dropTarget, eventData.position);
            transform.SetParent(dropTarget);
            transform.SetSiblingIndex(insertIndex);

            //드랍 되었을 때 처리
            EntryUIManager.Instance.OnDrop(this, dropTarget, eventData.position, previousParent);
        }
        else
        {
            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponent<RectTransform>().position;
        }
    }
}

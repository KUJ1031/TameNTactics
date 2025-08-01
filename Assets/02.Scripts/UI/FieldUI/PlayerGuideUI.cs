using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGuideUI : FieldMenuBaseUI
{
    [Header("범주 버튼들 (이동, 몬스터, 전투 등)")]
    public List<Button> categoryButtons;

    [Header("페이지 내용")]
    public Image guideImage;
    public List<Sprite> guideImages; // 여러 이미지가 있을 경우를 대비
    public TextMeshProUGUI guideTitle;
    public TextMeshProUGUI guideDescription;

    [Header("페이지 이동 버튼")]
    public Button prevButton;
    public Button nextButton;

    [Header("닫기 버튼")]
    public Button closeButton;

    private int currentCategoryIndex = 0;
    private int currentPageIndex = 0;

    [Header("하위 탭 버튼 관련")]
    [SerializeField] private Transform subTabButtonParent;         // 서브탭 버튼들이 들어갈 부모 오브젝트
    [SerializeField] private GameObject subTabButtonPrefab;        // 비활성화된 버튼 프리팹
    private List<Button> currentSubTabButtons = new List<Button>(); // 현재 생성된 버튼들

    // 데이터 구조 예시
    private Dictionary<string, List<GuidePage>> guideData;

    private void Start()
    {
        InitializeGuideData();
        SetupCategoryButtons();
        RefreshUI();
    }

    private void OnEnable()
    {
        if (guideData == null)
        {
            InitializeGuideData();
            SetupCategoryButtons();
        }

        prevButton.onClick.RemoveAllListeners();
        prevButton.onClick.AddListener(OnPrevClicked);

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(OnNextClicked);

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => Close());

        RefreshUI();
    }

    void InitializeGuideData()
    {
        guideData = new Dictionary<string, List<GuidePage>>
        {
            {
                "이동", new List<GuidePage>
                {
                    new GuidePage("조작", guideImages[0], "조작은 기본적으로 WASD로 가능합니다."),
                    new GuidePage("키 설정", guideImages[1], "환경 설정에서 따로 키셋팅을 다르게 설정할 수 있습니다.")
                }
            },
            {
                "몬스터", new List<GuidePage>
                {
                    new GuidePage("몬스터란", guideImages[2], "반향 증후군 (Resonance Syndrome) 전염병으로 인해 마음이 타락해버린 인간이었던 무언가입니다.\n겉의 빨간 테두리로 감염 여부를 확인할 수 있습니다."),
                    new GuidePage("공격성", guideImages[3], "본인들의 영역에 누군가 발을 들이면\n성격에 따라 피하기도, 적극적으로 공격에 나서기도 합니다."),
                    new GuidePage("전투 개시", null, "플레이어와 접촉 시 전투가 발생합니다."),
                    new GuidePage("인간성..?", null, "비록 전염병으로 인해 타락해버린 존재들이지만...\n그들의 인간성은 정말 사라져버린 걸까요?")
                }
            },
            {
                "엔트리", new List<GuidePage>
                {
                    new GuidePage("등록", null, "보유 몬스터 중 최대 5명까지\n현재 엔트리에 등록시켜둘 수 있습니다.\n이들은 전투 발생 시 당신의 명령에 따라 전투에 임합니다."),
                    new GuidePage("엔트리", null, "엔트리는 [배틀 엔트리]와 [벤치 엔트리]로 나뉩니다.\n배틀 엔트리는 최대 3명까지 등록이 가능하며,\n남은 2명은 벤치 엔트리에 등록됩니다."),
                    new GuidePage("전투 시", null, "전투에는 [배틀 엔트리]가 임하게 됩니다.\n[벤치 엔트리]는 전투에 참여할 수 없습니다."),
                    new GuidePage("전투 종료", null, "전투 종료 시 [배틀 엔트리]는 경험치를 획득합니다.\n [벤치 엔트리] 또한 경험치를 일부 획득합니다.")
                }
            },
            {
                "전투", new List<GuidePage>
                {
                    new GuidePage("공격", null, "[공격하기] 버튼을 눌러 상대를 공격할 태세를 갖춥니다.\n고유 스킬을 이용해 상대를 공격할 수 있습니다."),
                    new GuidePage("턴제", null, "공격 시, 공격자와 피격자의 스피드에 따라\n전투 순서가 결정됩니다."),
                    new GuidePage("궁극기", null, "파란 게이지가 전부 차면 강력한 [궁극기]를 사용할 수 있습니다.\n캐릭터별로 궁극기의 효과와 채워야하는 게이지양이 전부 상이합니다."),
                    new GuidePage("포섭", null, "[포섭하기] 버튼을 눌러 현재 보유하고 있는 제스처를\n통해 상대 몬스터를 포섭 시도할 수 있습니다."),
                    new GuidePage("미니 게임", null, "포섭을 시도 시 간단한 미니게임이 진행됩니다.\n플레이어가 사용한 제스처와 상대의 성격에 따라\n미니게임의 난이도가 달라집니다."),
                    new GuidePage("포섭 성공", null, "포섭 성공 시, 해당 몬스터는 아군화되며\n상대에게 턴이 넘어갑니다.\n남아 있는 상대가 없을 경우 플레이어측 승리로 전투가 종료됩니다."),
                    new GuidePage("포섭 실패", null, "포섭 실패 시, 아무 일도 발생하지 않고\n상대에게 턴이 넘어갑니다."),
                    new GuidePage("인벤토리", null, "[인벤토리] 버튼을 눌러 현재 보유하고 있는\n소모 아이템을 사용할 수 있습니다.\n아이템 사용 후에 턴이 상대에게 넘어갑니다."),
                    new GuidePage("도망가기", null, "[도망가기] 버튼을 눌러 일정 확률로\n현재 전투에서 벗어날 수 있습니다.\n도주 실패 시 상대에게 턴이 넘어갑니다."),
                }
            },
            {
                "저장", new List<GuidePage>
                {
                    new GuidePage("저장", null, "환경 설정 내 [저장하기] 버튼을 눌러\n현재 진행상황을 저장할 수 있습니다."),
                    new GuidePage("이어하기", null, "게임 시작 화면에서 [이어하기] 버튼을 눌러\n현재 진행 상황 정보를 확인하고 해당 데이터를 불러올 수 있습니다."),
                    new GuidePage("초기화", null, "진행 중 데이터가 있음에도 불구하고 다시 [처음하기]를 통해 게임을 진행할 수 있습니다.\n진행 중이던 데이터는 사라집니다."),
                }
            },
            {
                "미니맵", new List<GuidePage>
                {
                    new GuidePage("미니맵", null, "미니맵 단축 키(M)를 통해 현재 플레이어의\n위치를 파악할 수 있습니다.")
                }
            },
            {
                "대화", new List<GuidePage>
                {
                    new GuidePage("NPC", null, "필드에는 플레이어와 상호작용할 수 있는 NPC들이\n존재합니다. 몬스터와는 달리 테두리가 파랗습니다."),
                    new GuidePage("대화 시도", null, "상호작용 키(F)를 통해 NPC와 상호작용할 수 있습니다."),
                    new GuidePage("퀘스트", null, "NPC와의 대화 도중 퀘스트가 발생할 수 있습니다.\n수락할 수도 있고, 거절할 수도 있습니다."),
                    new GuidePage("보상?", null, "퀘스트를 잘 수행한다면 좋은 일이 생길 지도 모릅니다. ...그리고 선택지만 잘 골라도 행운이 생길 지도..?"),
                }
            },
            {
                "상점", new List<GuidePage>
                {
                    new GuidePage("구매", null, "상점에서는 소모품이나 무기, 제스처 등을 구매할 수 있습니다."),
                    new GuidePage("판매", null, "구매하거나 획득한 아이템을 구매가의 70%의 가격으로 판매할 수도 있습니다."),
                }
            },
            {
                "힐링존", new List<GuidePage>
                {
                    new GuidePage("휴식", null, "힐링존에서는 현재 보유 중인 모든 몬스터를\n회복시키거나, 엔트리에 등록된 몬스터들을\n회복시킬 수 있습니다."),
                    new GuidePage("가격", null, "힐링존을 이용하기 위해서는 골드가 필요하며,\n모든 몬스터를 회복시키려면 조금 더 많은 골드가\n필요합니다."),
                    new GuidePage("효과", null, "해당 효과를 받은 모든 몬스터들은 Hp가 전부 회복됩니다. 단, 궁극기 게이지도 전부 초기화됩니다.")
                }
            },
            {
                "아이템", new List<GuidePage>
                {
                    new GuidePage("소모품", null, "물약 등 소모품을 사용할 수 있습니다.\n전투 중이나 필드 어디서든 전부 사용이 가능합니다."),
                    new GuidePage("장착템", null, "아이템 장착 시, 해당 효과가 모든 몬스터에게 적용됩니다.\n필드에서만 사용 가능합니다."),
                    new GuidePage("제스처", null, "배틀 중 포섭 진행 시 사용되는 아이템입니다.\n전투에서만 사용 가능합니다.")
                }
            },

        };
    }

    void SetupCategoryButtons()
    {
        for (int i = 0; i < categoryButtons.Count; i++)
        {
            int index = i;
            categoryButtons[i].onClick.AddListener(() =>
            {
                currentCategoryIndex = index;
                currentPageIndex = 0;

                string selectedCategory = categoryButtons[index].GetComponentInChildren<TextMeshProUGUI>().text;

                RefreshSubTabs(selectedCategory);
                RefreshUI();
            });
        }

        // 초기 진입시 기본 탭 설정
        string defaultCategory = categoryButtons[0].GetComponentInChildren<TextMeshProUGUI>().text;
        RefreshSubTabs(defaultCategory);
    }


    void RefreshSubTabs(string category)
    {
        // 기존 버튼 제거
        foreach (Transform child in subTabButtonParent)
        {
            Destroy(child.gameObject);
        }
        currentSubTabButtons.Clear();

        // 현재 카테고리의 페이지들 가져오기
        var pages = guideData[category];

        for (int i = 0; i < pages.Count; i++)
        {
            int index = i;

            // 프리팹 복제 및 활성화
            GameObject buttonObj = Instantiate(subTabButtonPrefab, subTabButtonParent);
            buttonObj.SetActive(true);

            // 텍스트 설정
            TextMeshProUGUI text = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            text.text = pages[i].title;

            // 버튼 클릭 이벤트 설정
            Button btn = buttonObj.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                currentPageIndex = index;
                RefreshUI();
            });

            currentSubTabButtons.Add(btn);
        }
    }


    void RefreshUI()
    {
        string category = categoryButtons[currentCategoryIndex].GetComponentInChildren<TextMeshProUGUI>().text;
        var pages = guideData[category];

        GuidePage page = pages[currentPageIndex];
        guideTitle.text = category;
        guideImage.sprite = page.image;
        guideDescription.text = page.description;

        prevButton.interactable = currentPageIndex > 0;
        nextButton.interactable = currentPageIndex < pages.Count - 1;

        for (int i = 0; i < currentSubTabButtons.Count; i++)
        {
            currentSubTabButtons[i].interactable = (i != currentPageIndex);
        }

    }

    public void OnPrevClicked()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            RefreshUI();
        }
    }

    public void OnNextClicked()
    {
        if (currentPageIndex < guideData[categoryButtons[currentCategoryIndex].GetComponentInChildren<TextMeshProUGUI>().text].Count - 1)
        {
            currentPageIndex++;
            RefreshUI();
        }
    }

    public void ShowCategory(string categoryName)
    {
        // 카테고리 인덱스 찾기
        int index = categoryButtons.FindIndex(btn =>
            btn.GetComponentInChildren<TextMeshProUGUI>().text == categoryName);

        if (index < 0)
        {
            Debug.LogWarning($"카테고리 '{categoryName}'를 찾을 수 없습니다.");
            return;
        }

        currentCategoryIndex = index;
        currentPageIndex = 0;

        string selectedCategory = categoryButtons[currentCategoryIndex].GetComponentInChildren<TextMeshProUGUI>().text;

        RefreshSubTabs(selectedCategory);
        RefreshUI(); // 이 두 줄이면 충분
    }
}

[System.Serializable]
public class GuidePage
{
    public string title;
    public Sprite image;
    public string description;

    public GuidePage(string title, Sprite image, string description)
    {
        this.title = title;
        this.image = image;
        this.description = description;
    }
}

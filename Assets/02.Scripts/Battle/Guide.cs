using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guide : MonoBehaviour
{
    [Header("GuideAttackImage 안의 페이지들")]
    public GameObject guideAttackImageParent;
    public GameObject guidaAttackBackGrounds;
    private List<GameObject> guideAttackPages = new();

    [Header("GuideInventory 안의 페이지들")]
    public GameObject guideInventoryImageParent;
    public GameObject guideInventoryBackGrounds;
    private List<GameObject> guideInventoryPages = new();

    [Header("GuideEmbrace 안의 페이지들")]
    public GameObject guideEmbraceImageParent;
    public GameObject guideEmbraceBackGrounds;
    private List<GameObject> guideEmbracePages = new();

    [Header("GuideEscape 안의 페이지들")]
    public GameObject guideEscapeImageParent;
    public GameObject guideEscapeBackGrounds;
    private List<GameObject> guideEscapePages = new();

    [Header("버튼들")]
    public Button prevButton;
    public Button nextButton;
    public Button exitButton;

    private List<GameObject> currentGuidePages;
    private GameObject currentBackground;
    private int currentIndex = 0;

    private void Awake()
    {
        guideAttackPages = GetChildren(guideAttackImageParent);
        guideInventoryPages = GetChildren(guideInventoryImageParent);
        guideEmbracePages = GetChildren(guideEmbraceImageParent);
        guideEscapePages = GetChildren(guideEscapeImageParent);

        HideAllGuides();
        SetButtonsActive(false);

        prevButton.onClick.AddListener(ShowPrevPage);
        nextButton.onClick.AddListener(ShowNextPage);
        exitButton.onClick.AddListener(ExitGuide);
    }

    private List<GameObject> GetChildren(GameObject parent)
    {
        var list = new List<GameObject>();
        foreach (Transform child in parent.transform)
        {
            list.Add(child.gameObject);
        }
        return list;
    }

    public void StartGuide(GuideType guideType)
    {
        HideAllGuides();

        switch (guideType)
        {
            case GuideType.Attack:
                currentGuidePages = guideAttackPages;
                currentBackground = guidaAttackBackGrounds;
                break;
            case GuideType.Inventory:
                currentGuidePages = guideInventoryPages;
                currentBackground = guideInventoryBackGrounds;
                break;
            case GuideType.Embrace:
                currentGuidePages = guideEmbracePages;
                currentBackground = guideEmbraceBackGrounds;
                break;
            case GuideType.Escape:
                currentGuidePages = guideEscapePages;
                currentBackground = guideEscapeBackGrounds;
                break;
        }

        if (currentGuidePages.Count == 0) return;

        currentIndex = 0;
        currentGuidePages[currentIndex].SetActive(true);
        currentBackground.SetActive(true);
        SetButtonsActive(true);
    }

    private void ShowPrevPage()
    {
        if (currentGuidePages == null || currentGuidePages.Count == 0) return;

        currentGuidePages[currentIndex].SetActive(false);
        currentIndex = (currentIndex - 1 + currentGuidePages.Count) % currentGuidePages.Count;
        currentGuidePages[currentIndex].SetActive(true);
    }

    private void ShowNextPage()
    {
        if (currentGuidePages == null || currentGuidePages.Count == 0) return;

        currentGuidePages[currentIndex].SetActive(false);
        currentIndex = (currentIndex + 1) % currentGuidePages.Count;
        currentGuidePages[currentIndex].SetActive(true);
    }

    private void ExitGuide()
    {
        HideAllGuides();
        SetButtonsActive(false);
    }

    private void HideAllGuides()
    {
        foreach (var page in guideAttackPages) page.SetActive(false);
        foreach (var page in guideInventoryPages) page.SetActive(false);
        foreach (var page in guideEmbracePages) page.SetActive(false);
        foreach (var page in guideEscapePages) page.SetActive(false);

        guidaAttackBackGrounds.SetActive(false);
        guideInventoryBackGrounds.SetActive(false);
        guideEmbraceBackGrounds.SetActive(false);
        guideEscapeBackGrounds.SetActive(false);
    }

    private void SetButtonsActive(bool isActive)
    {
        prevButton.gameObject.SetActive(isActive);
        nextButton.gameObject.SetActive(isActive);
        exitButton.gameObject.SetActive(isActive);
    }
}

public enum GuideType
{
    Attack,
    Inventory,
    Embrace,
    Escape
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapLogicGuideUI : MonoBehaviour
{
    [Header("UI Components")]
    public Image guideImage;
    public TextMeshProUGUI guideText;
    public Button nextButton;
    public Button prevButton;
    public Button closeButton;

    [Header("Guide Data")]
    public Sprite[] guideImages;
    [TextArea]
    public string[] guideTexts;

    private int currentIndex = 0;

    private void Start()
    {
        nextButton.onClick.AddListener(ShowNext);
        prevButton.onClick.AddListener(ShowPrev);
        closeButton.onClick.AddListener(CloseGuide);

        UpdateGuide();
    }

    private void ShowNext()
    {
        if (currentIndex < guideImages.Length - 1)
        {
            currentIndex++;
            UpdateGuide();
        }
    }

    private void ShowPrev()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateGuide();
        }
    }

    private void CloseGuide()
    {
        gameObject.SetActive(false);
    }

    private void UpdateGuide()
    {
        guideImage.sprite = guideImages[currentIndex];
        guideText.text = guideTexts[currentIndex];

        prevButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < guideImages.Length - 1;
    }
}

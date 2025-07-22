using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [Header("이미지")]
    public Image playerImage;
    public Image npcImage;
    public Image dialogueWindowImage;
    public Image speakerImage;
    public Image skipImage;

    [Header("텍스트")]
    public TMP_Text speakerNameText;
    public TMP_Text dialogueText;
    public TMP_Text skipText;

    [Header("선택지")]
    public GameObject choicePanel;
    public Button continueButton;
    public Button choiceButton1;
    public TMP_Text choice1Text;
    public Button choiceButton2;
    public TMP_Text choice2Text;
    public Button choiceButton3;
    public TMP_Text choice3Text;
    public Button skipButton;

    /// <summary>
    /// 대화 노드와 이미지 및 이름 정보를 받아 UI를 보여줌
    /// </summary>
    public void Show(DialogueNode node, Sprite npcSprite, string speakerName)
    {
        gameObject.SetActive(true);

        UpdateImages(npcSprite, speakerName);
        UpdateText(node, speakerName);
        UpdateChoices(node);
        UpdateSpeakerPosition(speakerName);
    }

    /// <summary>
    /// 대화 이미지 관련 요소 설정
    /// </summary>
    private void UpdateImages(Sprite npcSprite, string speaker)
    {
        // 대화창 이미지 설정
        if (playerImage != null) playerImage.sprite = PlayerManager.Instance.playerImage; // 플레이어 이미지 설정
        if (npcImage != null) npcImage.sprite = npcSprite;

        // 밝기 조절용 색상
        Color bright = Color.white;                     // (1,1,1,1)
        Color dim = new Color(0.25f, 0.25f, 0.25f, 1f);     // 어둡게

        bool isPlayerSpeaking = speaker == "나";

        if (npcImage != null)
            npcImage.color = isPlayerSpeaking ? dim : bright;

        if (playerImage != null)
            playerImage.color = isPlayerSpeaking ? bright : dim;
    }

    /// <summary>
    /// 대화 텍스트 및 이름 설정
    /// </summary>
    private void UpdateText(DialogueNode node, string speakerName)
    {
        if (speakerNameText != null) speakerNameText.text = speakerName;
        if (dialogueText != null) dialogueText.text = node.Text;
    }

    private void UpdateSpeakerPosition(string speaker)
    {
        bool isPlayer = speaker == "나";

        Vector2 anchor = isPlayer ? new Vector2(1, 1) : new Vector2(0, 1);
        Vector2 pivot = anchor;
        Vector2 anchoredPos = new Vector2(0, 100);

        RectTransform imageRect = speakerImage.rectTransform;
        imageRect.anchorMin = anchor;
        imageRect.anchorMax = anchor;
        imageRect.pivot = pivot;
        imageRect.anchoredPosition = anchoredPos;

        // speakerNameText는 자식이므로 자동 따라감
    }

    /// <summary>
    /// 선택지 버튼 세팅
    /// </summary>
    private void UpdateChoices(DialogueNode node)
    {
        choicePanel.SetActive(true);

        bool hasChoice1 = !string.IsNullOrEmpty(node.Choice1);
        bool hasChoice2 = !string.IsNullOrEmpty(node.Choice2);
        bool hasChoice3 = !string.IsNullOrEmpty(node.Choice3);
        bool hasAnyChoice = hasChoice1 || hasChoice2 || hasChoice3;

        // 버튼 텍스트 및 활성화
        choiceButton1.gameObject.SetActive(hasChoice1);
        choice1Text.text = hasChoice1 ? node.Choice1 : "";

        choiceButton2.gameObject.SetActive(hasChoice2);
        choice2Text.text = hasChoice2 ? node.Choice2 : "";

        choiceButton3.gameObject.SetActive(hasChoice3);
        choice3Text.text = hasChoice3 ? node.Choice3 : "";

        int rectX = 230; // 버튼 위치 X 좌표

        // 버튼 위치 조정
        if (hasChoice1 && hasChoice2 && !hasChoice3)
        {
            // 선택지 2개일 때: 중앙 정렬
            SetButtonPosition(choiceButton1.GetComponent<RectTransform>(), new Vector2(rectX, 70));
            SetButtonPosition(choiceButton2.GetComponent<RectTransform>(), new Vector2(rectX, -70));
        }
        else if (hasChoice1 && hasChoice2 && hasChoice3)
        {
            // 선택지 3개일 때: 세로 정렬
            SetButtonPosition(choiceButton1.GetComponent<RectTransform>(), new Vector2(rectX, 100));
            SetButtonPosition(choiceButton2.GetComponent<RectTransform>(), new Vector2(rectX, 0));
            SetButtonPosition(choiceButton3.GetComponent<RectTransform>(), new Vector2(rectX, -100));
        }

        skipButton.gameObject.SetActive(!hasAnyChoice);
        continueButton.gameObject.SetActive(!hasAnyChoice);
    }

    private void SetButtonPosition(RectTransform rect, Vector2 anchoredPos)
    {
        rect.anchoredPosition = anchoredPos;
    }

    /// <summary>
    /// 대화창 숨김
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // 버튼 이벤트 → DialogueManager에게 선택 전달
    public void OnClickChoice1() => DialogueManager.Instance.OnSelectChoice(1);
    public void OnClickChoice2() => DialogueManager.Instance.OnSelectChoice(2);

    public void OnClickChoice3() => DialogueManager.Instance.OnSelectChoice(3);
    public void OnClickContinue() => DialogueManager.Instance.OnSelectChoice(0);
    public void OnClickSkip() => DialogueManager.Instance.OnClickSkip();
}

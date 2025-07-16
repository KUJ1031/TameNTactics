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
    public Button skipButton;

    /// <summary>
    /// 대화 노드와 이미지 및 이름 정보를 받아 UI를 보여줌
    /// </summary>
    public void Show(DialogueNode node, Sprite npcSprite, string speakerName)
    {
        gameObject.SetActive(true);

        UpdateImages(npcSprite);
        UpdateText(node, speakerName);
        UpdateChoices(node);
    }

    /// <summary>
    /// 대화 이미지 관련 요소 설정
    /// </summary>
    private void UpdateImages(Sprite npcSprite)
    {
        if (npcImage != null) npcImage.sprite = npcSprite;

        // playerImage나 dialogueWindowImage는 고정일 경우 생략 가능
    }

    /// <summary>
    /// 대화 텍스트 및 이름 설정
    /// </summary>
    private void UpdateText(DialogueNode node, string speakerName)
    {
        if (speakerNameText != null) speakerNameText.text = speakerName;
        if (dialogueText != null) dialogueText.text = node.Text;
    }

    /// <summary>
    /// 선택지 버튼 세팅
    /// </summary>
    private void UpdateChoices(DialogueNode node)
    {
        choicePanel.SetActive(true); // 패널은 항상 켜두고

        bool hasChoice1 = !string.IsNullOrEmpty(node.Choice1);
        bool hasChoice2 = !string.IsNullOrEmpty(node.Choice2);
        bool hasAnyChoice = hasChoice1 || hasChoice2;

        // 선택지 버튼 보이기/숨기기
        choiceButton1.gameObject.SetActive(hasChoice1);
        choice1Text.text = hasChoice1 ? node.Choice1 : "";

        choiceButton2.gameObject.SetActive(hasChoice2);
        choice2Text.text = hasChoice2 ? node.Choice2 : "";

        // 스킵 버튼은 선택지가 없을 때만 보이게
        skipButton.gameObject.SetActive(!hasChoice1 && !hasChoice2);

        // continue 버튼은 선택지가 없을 때만 보이게
        continueButton.gameObject.SetActive(!hasAnyChoice);
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
    public void OnClickContinue() => DialogueManager.Instance.OnSelectChoice(0);
    public void OnClickSkip() => DialogueManager.Instance.OnClickSkip();
}

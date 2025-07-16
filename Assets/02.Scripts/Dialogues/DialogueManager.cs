using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("어드레서블")]
    public string csvAddress = "dialogue"; // 어드레서블 이름 (e.g. Assets/AddressableAssetsData/Dialogs/dialogue.csv)

    [Header("UI 연결")]
    public DialogueUI dialogueUI;

    private Dictionary<string, Dictionary<int, DialogueNode>> dialogueTrees = new();
    private Dictionary<int, DialogueNode> currentTree;
    private DialogueNode currentNode;

    private Sprite currentNPCImage;
    private string currentSpeakerName;

    private bool isSkipping = false;
    public bool isCommunicationEneded = false; // 대화 종료 여부

    private Coroutine skipBlinkCoroutine;


    void Start()
    {
        // CSV 데이터 로딩
        Addressables.LoadAssetAsync<TextAsset>(csvAddress).Completed += OnCSVLoaded;
    }

    void OnCSVLoaded(AsyncOperationHandle<TextAsset> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            dialogueTrees = DialogueCSVParser.ParseByTreeID(handle.Result);
            Debug.Log("모든 NPC 대사 로드 완료");
        }
        else
        {
            Debug.LogError("CSV 어드레서블 로드 실패");
        }
    }

    /// <summary>
    /// 대화 시작: 해당 NPC의 대화 ID로부터 시작
    /// </summary>
    public void StartDialogue(string dialogueTreeId, Sprite npcImage, int startID)
    {
        currentNPCImage = npcImage;

        if (!dialogueTrees.ContainsKey(dialogueTreeId))
        {
            Debug.LogError($"대화 트리 '{dialogueTreeId}'가 없습니다.");
            return;
        }

        currentTree = dialogueTrees[dialogueTreeId];  // 트리 할당 필수!

        if (!currentTree.ContainsKey(startID))
        {
            Debug.LogError($"시작 ID {startID}가 대화 트리 '{dialogueTreeId}'에 없습니다.");
            return;
        }

        ShowNode(currentTree[startID]);
    }

    /// <summary>
    /// 현재 노드 표시
    /// </summary>
    void ShowNode(DialogueNode node)
    {
        Debug.Log($"노드 표시: ID {node.ID}, 화자 {node.Speaker}, 텍스트 '{node.Text}'");
        currentNode = node;

        // 스피커 이름으로 이미지 가져오기
        Sprite speakerImage = GetSpeakerSprite(node.Speaker);

        dialogueUI.Show(
            node,
            speakerImage,    // 현재 노드의 화자에 해당하는 이미지
            node.Speaker     // 현재 노드의 화자 이름
        );
    }

    private Dictionary<string, Sprite> speakerSprites = new(); // 이름 → 스프라이트

    private Sprite GetSpeakerSprite(string speakerName)
    {
        if (speakerSprites.TryGetValue(speakerName, out var sprite))
            return sprite;

        return currentNPCImage; // 기본 이미지 (예: null or "???")
    }

    /// <summary>
    /// 선택지 클릭 시 호출
    /// </summary>
    public void OnSelectChoice(int choiceIndex)
    {
        int nextID = -1;

        if (choiceIndex == 1) nextID = currentNode.Choice1Next;
        else if (choiceIndex == 2) nextID = currentNode.Choice2Next;
        else if (choiceIndex == 3) nextID = currentNode.Choice3Next;
        else nextID = currentNode.Next;

        if (nextID == -1)
        {
            dialogueUI.Hide();
            isCommunicationEneded = true; // 대화 종료 상태로 설정

            return;
        }

        if (currentTree.TryGetValue(nextID, out var nextNode))
        {
            ShowNode(nextNode);
        }
        else
        {
            Debug.LogWarning($" ID {nextID} 없음");
            dialogueUI.Hide();
        }
    }

    /// <summary>
    /// 스킵 버튼 클릭 시 호출
    /// </summary>
    public void OnClickSkip()
    {
        if (!isSkipping)
            StartCoroutine(SkipToNextChoiceOrEnd());
    }

    /// <summary>
    /// 선택지 등장 전까지 자동으로 다음 노드로 이동
    /// </summary>
    private IEnumerator SkipToNextChoiceOrEnd()
    {
        isSkipping = true;

        Image skipImage = dialogueUI.skipImage;
        skipImage.gameObject.SetActive(true);
        if (skipImage != null && skipBlinkCoroutine == null)
            skipBlinkCoroutine = StartCoroutine(BlinkSkipImage(skipImage));

        while (currentNode != null && isSkipping)
        {
            // 마지막 노드 도달 시 처리
            if (currentNode.Next == -1)
            {
                // 마지막 노드 보여준 상태 유지 (Hide 안 함)
                StopSkipBlink();
                isSkipping = false;
                yield break;
            }

            int nextID = currentNode.Next;

            if (currentTree.TryGetValue(nextID, out var nextNode))
            {
                ShowNode(nextNode);
                yield return new WaitForSeconds(0.2f);

                bool hasChoice = !string.IsNullOrEmpty(nextNode.Choice1) || !string.IsNullOrEmpty(nextNode.Choice2);
                if (hasChoice)
                {
                    StopSkipBlink();
                    isSkipping = false;
                    yield break;
                }
            }
            else
            {
                Debug.LogWarning($"스킵 중: ID {nextID} 없음");
                dialogueUI.Hide();
                StopSkipBlink();
                isSkipping = false;
                yield break;
            }
        }

        StopSkipBlink();
        isSkipping = false;
    }




    private IEnumerator BlinkSkipImage(Image image)
    {
        Color originalColor = image.color;
        Text skipText = image.GetComponentInChildren<Text>();
        while (true)
        {
            // 점점 나타남
            for (float a = 0.1f; a <= 1f; a += Time.deltaTime * 1.5f)
            {
                SetAlpha(image, a);
                yield return null;
            }

            // 점점 사라짐
            for (float a = 1f; a >= 0.1f; a -= Time.deltaTime * 1.5f)
            {
                SetAlpha(image, a);
                yield return null;
            }
        }
    }

    private void StopSkipBlink()
    {
        if (skipBlinkCoroutine != null)
        {
            StopCoroutine(skipBlinkCoroutine);
            skipBlinkCoroutine = null;
        }

        if (dialogueUI.skipImage != null)
        {
            var image = dialogueUI.skipImage;
            var text = image.GetComponentInChildren<Text>();

            SetAlpha(image, 0f); // 알파 0으로 설정
            image.gameObject.SetActive(false);
        }
    }

    private void SetAlpha(Image image, float alpha)
    {
        if (image != null)
        {
            var c = image.color;
            c.a = alpha;
            image.color = c;
        }
    }

}

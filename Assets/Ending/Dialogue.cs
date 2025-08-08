using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public DialogueData[] dialogueDatas; //대화 데이터
    public Text dialogueText; // 대화를 표시하는 텍스트
    public Text dialogueText1; // 
    public float typingSpeed = 0.05f; // 타이핑 속도

    public float delayBetweenLines = 1.0f; // 다음 대사까지의 지연시간
    private int currentDataIndex = 0; // 현재 대화 데이터 인덱스
    private int currentLine = 0; // 현재 대사의 줄 인덱스
    private Coroutine typingCoroutine; // 타이핑 코루틴

    public EndingManager endingManager;
    public bool printiong = false;
    public GameObject DialogueObject;

    private void Start()
    {
        ShowDialogue(0); // 
    }

    private void ShowDialogue(int index)
    {
        if (index < 0 || index >= dialogueDatas.Length)
        {
            return;
        }
        if(index == 1)
        {
            dialogueText.fontSize = 100;
            dialogueText.color = Color.white;
        }
        currentDataIndex = index;
        currentLine = 0;
        ShowNextLine(); // 
    }

    private void ShowNextLine()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        DialogueData data = dialogueDatas[currentDataIndex];

        if (currentLine < data.lines.Length)
        {
            typingCoroutine = StartCoroutine(TypeLine(data.lines[currentLine]));
            currentLine++;
        }
        else
        {
            if(printiong == false)
            {
                dialogueText.text = "";
                Invoke("display", 5f);
            }
            else
            {
                Invoke("displaytext", 2f);
            }
                //endingManager.output();
                //currentDataIndex = 1;
        }
    }

    void display()
    {
        if (printiong == false)
        {
            printiong = true;
            ShowDialogue(1);
        }
    }

    void displaytext()
    {
        dialogueText.text = "";
        DialogueObject.SetActive(true);
    }
    // 
    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(delayBetweenLines);

        ShowNextLine();
    }
}

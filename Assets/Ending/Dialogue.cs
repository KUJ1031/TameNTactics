using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public DialogueData[] dialogueDatas; //대화 데이터
    public Text dialogueText; //대화를 반영하는 Text
    public float typingSpeed = 0.05f; //타이핑 속도

    public float delayBetweenLines = 1.0f; //다음 대사
    private int currentDetalndex = 0; //현재 대화 줄
    private int currentLine = 0; //
    private Coroutine typingCoroutine; //타이핑 코루틴

    private void Start()
    {
        ShowDialogue(0);
    }

    private void ShowDialogue(int index)
    {
        if (index < 0 || index >= dialogueDatas.Length)
        {
            return;
        }

        currentDetalndex = index;
        currentLine = 0;
        ShowNextLine();
    }

    private void ShowNextLine()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        DialogueData data = dialogueDatas[currentDetalndex];

        if (currentLine < data.lines.Length)
        {
            typingCoroutine = StartCoroutine(TypeLine(data.lines[currentLine]));
            currentLine++;
        }
        else
        {
            dialogueText.text = "";
            Debug.Log("대화 종료!");
        }
    }

    //한글자씩 나오는 효과
    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        //한글자씩 빼온다
        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(delayBetweenLines);

        ShowNextLine();
    }

}

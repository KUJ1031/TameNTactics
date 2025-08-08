using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    [TextArea(3, 10)]
    public string[] lines; // 대사 문장들을 저장하는 배열 (한 줄씩 입력)
}

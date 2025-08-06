using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/DialogueData")]

public class DialogueData : ScriptableObject
{
    [TextArea(3, 10)]
    public string[] lines;

}

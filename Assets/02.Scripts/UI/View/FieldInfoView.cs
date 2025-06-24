using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FieldInfoView : MonoBehaviour
{
    public TextMeshProUGUI mapNameText;
    public TextMeshProUGUI playTimeText;

    public void SetMapName(string name)
    {
        mapNameText.text = name;
    }

    public void SetPlayTime(float seconds)
    {
        int minute = (int)(seconds / 60);
        int second = (int)(seconds % 60);

        playTimeText.text = $"{minute:D2}:{second:D2}";
    }
}

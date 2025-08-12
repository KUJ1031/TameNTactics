using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBottm : MonoBehaviour
{
    public ScrollRect scrollRect;

    [Range(-1f, 1f)]
    public float minScrollY = -0.3f; // 아래로 내려갈 수 있는 최대값
    [Range(0f, 4f)]
    public float maxScrolly = 0.8f; // 위로 올라갈 수 있는 최대값


   

    void Update()
    {
        float pos = scrollRect.verticalNormalizedPosition;

        if (pos < minScrollY)
            scrollRect.verticalNormalizedPosition = minScrollY;
        else if (pos > maxScrolly)
            scrollRect.verticalNormalizedPosition = maxScrolly;
    }
}

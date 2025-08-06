using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FlyText : MonoBehaviour
{
    public float floatdistance = 50f;
    public float testtime = 1f;
    public Text text;
    private RectTransform RectTransform;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        play();
    }

    private void play()
    {
        gameObject.SetActive(true);
        StartCoroutine(FlyCoroutine());
    }

    private IEnumerator FlyCoroutine()
    {
        Vector2 Start = RectTransform.anchoredPosition;
        Vector2 end = Start + Vector2.up * floatdistance;
        float timer = 0f;
        while (timer <= testtime)
        {
            timer += Time.deltaTime;
            float t = timer / testtime;
            RectTransform.anchoredPosition = Vector2.Lerp(Start, end, t);
            yield return null;
        }
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyText : MonoBehaviour
{
    public float floatDistance = 50f;           
    public float floatDuration = 1f;            
    public Text flyText;                        

    private RectTransform rectTransform;        
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Play();
    }

    private void Play()
    {
        gameObject.SetActive(true);
        StartCoroutine(FlyUpCoroutine());
    }

    private IEnumerator FlyUpCoroutine()
    {
        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 endPosition = startPosition + Vector2.up * floatDistance;
        float timer = 0f;

        while (timer <= floatDuration)
        {
            timer += Time.deltaTime;
            float t = timer / floatDuration;
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}

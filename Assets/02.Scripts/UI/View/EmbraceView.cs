using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmbraceView : MonoBehaviour
{
    [SerializeField] private GameObject successMessage;
    [SerializeField] private GameObject failMessage;
    [SerializeField] private TextMeshProUGUI guideText;

    public void ShowGuide(string message)
    {
        if (guideText != null)
        {
            guideText.text = message;
            guideText.gameObject.SetActive(true);
        }

        StartCoroutine(HideAfterDelay(2f));
    }

    public void ShowSuccessMessage()
    {
        if (successMessage != null)
        {
            successMessage.SetActive(true);
        }

        if (failMessage != null)
        {
            failMessage.SetActive(false);
        }

        StartCoroutine(HideAfterDelay(2f));
    }

    public void ShowFailMessage()
    {
        if (failMessage != null)
        {
            failMessage.SetActive(true);
        }

        if (successMessage != null)
        {
            successMessage.SetActive(false);
        }

        StartCoroutine(HideAfterDelay(2f));
    }

    public void HideMessage()
    {
        if (successMessage != null)
        {
            successMessage.SetActive(false);
        }

        if (failMessage != null)
        {
            failMessage.SetActive(false);
        }
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //HideMessage();

        if (guideText != null)
        {
            guideText.gameObject.SetActive(false);
        }

        if (successMessage != null)
        {
            successMessage.SetActive(false);
        }

        if (failMessage != null)
        {
            failMessage.SetActive(false);
        }
    }
}

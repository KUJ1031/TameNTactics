using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmbraceView : MonoBehaviour
{
    [SerializeField] private GameObject successMessage;
    [SerializeField] private GameObject failMessage;
    [SerializeField] private TextMeshProUGUI guideText;

    private void Awake()
    {
        HideMessage();
    }

    public void ShowGuide(string message)
    {
        if (guideText != null)
        {
            guideText.text = message;
        }
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
}

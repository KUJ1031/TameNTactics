using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmbraceView : MonoBehaviour
{
    [SerializeField] private GameObject successMessage;
    [SerializeField] private GameObject failMessage;
    [SerializeField] private TextMeshProUGUI guideText;

    private Coroutine currentCoroutine;

    public void ShowGuide(string message)
    {
        StopCurrentCoroutine();

        if (guideText != null)
        {
            guideText.text = message;
            guideText.gameObject.SetActive(true);
        }

        currentCoroutine = StartCoroutine(HideAfterDelay(2f));
    }

    public void ShowSuccessMessage()
    {
        StopCurrentCoroutine();

        if (successMessage != null)
        {
            successMessage.SetActive(true);
        }

        if (failMessage != null)
        {
            failMessage.SetActive(false);
        }

        currentCoroutine = StartCoroutine(HideAfterDelay(2f));
    }

    public void ShowFailMessage()
    {
        StopCurrentCoroutine();

        if (failMessage != null)
        {
            failMessage.SetActive(true);
        }

        if (successMessage != null)
        {
            successMessage.SetActive(false);
        }

        currentCoroutine = StartCoroutine(HideAfterDelay(2f));
    }

    public void HideMessage()
    {
        StopCurrentCoroutine();

        if (successMessage != null)
        {
            successMessage.SetActive(false);
        }

        if (failMessage != null)
        {
            failMessage.SetActive(false);
        }

        if (guideText != null)
        {
            guideText.gameObject.SetActive(false);
        }
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

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

        currentCoroutine = null;
    }

    private void StopCurrentCoroutine()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        if (successMessage != null)
            successMessage.SetActive(false);

        if (failMessage != null)
            failMessage.SetActive(false);

        if (guideText != null)
            guideText.gameObject.SetActive(false);
    }
}

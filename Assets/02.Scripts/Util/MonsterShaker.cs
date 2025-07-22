using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterShaker : MonoBehaviour
{
    private float shakeDuration = 0.2f;
    private float shakeMagnitude = 0.1f;

    private Vector3 originalPos;

    private void Awake()
    {
        originalPos = transform.localPosition;
    }

    public void TriggerShake()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector2 randomOffset = Random.insideUnitCircle * shakeMagnitude;
            transform.localPosition = originalPos + new Vector3(randomOffset.x, randomOffset.y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}

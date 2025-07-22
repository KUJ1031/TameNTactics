using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterShaker : MonoBehaviour
{
    private float shakeDuration = 0.2f;
    private float shakeStrength = 0.3f;
    private int vibrato = 10;
    private float randomness = 180f;

    private Vector3 originalPos;

    private void Awake()
    {
        originalPos = transform.localPosition;
    }

    public void TriggerShake()
    {
        transform.localPosition = originalPos;
        transform.DOShakePosition(
            duration: shakeDuration,
            strength: shakeStrength,
            vibrato: vibrato,
            randomness: randomness,
            snapping: false,
            fadeOut: true
            ).OnComplete(() => transform.localPosition = originalPos);
    }
}

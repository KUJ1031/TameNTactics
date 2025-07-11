using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public TextMeshProUGUI damageText;

    public float popupTime = 2f;
    public float fadeTime = 0.5f;
    public float moveSpeed = 1f;

    public void SetUp(int damage)
    {
        damageText.text = damage.ToString();
        StartCoroutine(ShowDamage());
    }

    private IEnumerator ShowDamage()
    {
        float time = 0f;

        while (time < popupTime)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;

            if (time > popupTime - fadeTime)
            {
                float opacity = (time - (popupTime - fadeTime)) / fadeTime;
                damageText.color = new Color(
                    damageText.color.r, damageText.color.g, damageText.color.b, 1f - opacity);
            }
            
            time += Time.deltaTime;
            yield return null;
        }
        
        Destroy(gameObject);
    }
}

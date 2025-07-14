using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public void SetUp()
    {
        StartCoroutine(ShowDamageAnimation());
    }

    private IEnumerator ShowDamageAnimation()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}

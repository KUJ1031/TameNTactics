using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public void SetUp()
    {
        Debug.Log("SetUp 호출됨");
        StartCoroutine(ShowDamageAnimation());
    }

    private IEnumerator ShowDamageAnimation()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}

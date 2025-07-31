using UnityEngine;

public class WanderingShopNPC : MonoBehaviour
{
    public bool IsInteracting { get; private set; } = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsInteracting = true;
            Debug.Log($"[WanderingShopNPC] 충돌 감지: {IsInteracting}");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsInteracting = false;
            Debug.Log($"[WanderingShopNPC] 충돌 종료: {IsInteracting}");
        }
    }
}

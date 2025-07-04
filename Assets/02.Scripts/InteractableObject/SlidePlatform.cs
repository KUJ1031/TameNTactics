using UnityEngine;

public class SlidePlatform : MonoBehaviour
{
    public float slideForce = 5f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = other.GetComponent<PlayerController>();
            var rb = other.GetComponent<Rigidbody2D>();

            if (controller != null && rb != null)
            {
                controller.isInputBlocked = true;

                // 마지막 입력 방향을 기준으로 밀기
                Vector2 slideDir = controller.lastMoveInput;
                if (slideDir == Vector2.zero)
                    slideDir = Vector2.down; // 기본 방향 (예: 아래로 미끄러지게)

                rb.velocity = slideDir * slideForce;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = other.GetComponent<PlayerController>();
            if (controller != null)
                controller.isInputBlocked = false;

            var rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = Vector2.zero;
        }
    }
}

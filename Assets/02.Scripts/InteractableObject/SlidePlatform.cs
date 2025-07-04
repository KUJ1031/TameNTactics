using UnityEngine;

public class SlidePlatform : MonoBehaviour
{
    [Header("플레이어를 미끄러뜨리는 힘")]
    public float slideForce = 5f;

    // 플레이어가 슬라이드 플랫폼 위에 있는 동안 계속 실행
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = other.GetComponent<PlayerController>();
            var rb = other.GetComponent<Rigidbody2D>();

            if (controller != null && rb != null)
            {
                // 플레이어의 입력을 잠금
                controller.isInputBlocked = true;

                // 마지막 이동 입력 방향을 기준으로 미끄러짐
                Vector2 slideDir = controller.lastMoveInput;

                if (slideDir == Vector2.zero)
                    slideDir = Vector2.down; // 아무 입력도 없을 경우 아래로 기본 밀기

                // Rigidbody에 슬라이드 힘 적용
                rb.velocity = slideDir * slideForce;
            }
        }
    }

    // 플레이어가 슬라이드 플랫폼을 벗어났을 때 실행
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = other.GetComponent<PlayerController>();
            if (controller != null)
            {
                // 입력 잠금 해제
                controller.isInputBlocked = false;
            }

            var rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // 남아 있는 슬라이드 속도 제거
                rb.velocity = Vector2.zero;
            }
        }
    }
}

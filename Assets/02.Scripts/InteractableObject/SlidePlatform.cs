using UnityEngine;

public class SlidePlatform : MonoBehaviour
{
    [Header("슬라이드 속도")]
    public float slideForce = 5f;

    [Header("박스 등은 이 방향으로 밀림 (상하좌우만!)")]
    public Vector2 slideDirection = Vector2.down;

    private void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        Vector2 dir = Vector2.zero;

        // 플레이어인 경우: 입력 방향
        var controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.isInputBlocked = true;
            dir = GetCardinalDirection(controller.lastMoveInput);

            if (dir == Vector2.zero)
                dir = Vector2.down; // 기본값
        }
        // 박스 등 Pushable 객체인 경우
        else if (other.GetComponent<PushableBox>() != null)
        {
            dir = GetCardinalDirection(slideDirection);
        }

        // 슬라이드 힘 적용
        rb.velocity = dir.normalized * slideForce;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = Vector2.zero;

        var controller = other.GetComponent<PlayerController>();
        if (controller != null)
            controller.isInputBlocked = false;
    }

    /// <summary>
    /// 대각선 입력 방지 - 상하좌우로만 변환
    /// </summary>
    private Vector2 GetCardinalDirection(Vector2 input)
    {
        if (input == Vector2.zero) return Vector2.zero;

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            return input.x > 0 ? Vector2.right : Vector2.left;
        else
            return input.y > 0 ? Vector2.up : Vector2.down;
    }
}

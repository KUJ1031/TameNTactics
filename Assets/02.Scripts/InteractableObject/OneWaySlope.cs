using UnityEngine;

public class OneWaySlope : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    [Header("통과 가능한 방향")]
    public Direction allowedDirection = Direction.Down;

    [Header("슬라이드 속도")]
    public float slideForce = 5f;

    private Vector2 GetAllowedVector()
    {
        return allowedDirection switch
        {
            Direction.Up => Vector2.up,
            Direction.Down => Vector2.down,
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            _ => Vector2.zero
        };
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var rb = other.attachedRigidbody;
        var controller = other.GetComponent<PlayerController>();
        var playerCollider = other.GetComponent<Collider2D>();
        var slopeCollider = GetComponent<Collider2D>();

        if (rb == null || controller == null || playerCollider == null || slopeCollider == null) return;

        Vector2 allowedVec = GetAllowedVector();

        Vector2 moveDir = rb.velocity.normalized;
        if (moveDir == Vector2.zero)
        {
            Vector2 input = controller.lastMoveInput.normalized;
            moveDir = input != Vector2.zero ? input : allowedVec;
        }

        float dot = Vector2.Dot(moveDir, allowedVec);

        if (dot < 0.7f)
        {
            controller.isInputBlocked = false;
            rb.velocity = Vector2.zero;

            // 발판과 플레이어 간 거리와 방향 구하기
            ColliderDistance2D dist = playerCollider.Distance(slopeCollider);

            if (dist.isOverlapped)
            {
                // 겹친 상태면 바깥 방향(발판->플레이어 방향)으로 밀어내기
                rb.position += dist.normal * dist.distance;
            }
        }
        else
        {
            controller.isInputBlocked = true;
            rb.velocity = allowedVec * slideForce;
        }
    }




    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var controller = other.GetComponent<PlayerController>();
        if (controller != null)
            controller.isInputBlocked = false;

        var rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = Vector2.zero;
    }
}

// OneWaySlope.cs
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OneWaySlope : MonoBehaviour
{
    public enum Direction { Up, Down, Left, Right }
    public Direction allowedDirection = Direction.Down;

    private Collider2D slopeCollider;

    private void Awake()
    {
        slopeCollider = GetComponent<Collider2D>();
        // 반드시 Trigger 체크 필요
        if (!slopeCollider.isTrigger)
        {
            Debug.LogWarning("OneWaySlope Collider must be set as Trigger!");
            slopeCollider.isTrigger = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var rb = other.attachedRigidbody;
        var controller = other.GetComponent<PlayerController>();
        var playerCollider = other.GetComponent<Collider2D>();
        var slidePlatform = other.GetComponentInParent<SlidePlatform>();

        if (rb == null || controller == null || playerCollider == null) return;

        Vector2 allowedVec = DirectionToVector(allowedDirection);
        Vector2 moveDir = controller.lastMoveInput.normalized;
        if (moveDir == Vector2.zero)
            moveDir = rb.velocity.normalized;

        float dot = Vector2.Dot(moveDir, allowedVec);

        ColliderDistance2D dist = playerCollider.Distance(slopeCollider);

        if (dot < 0.7f)
        {
            // 허용 방향 아닐 때
            if (dist.isOverlapped)
            {
                // 위치 겹침 시 밀어내기
                rb.position += dist.normal * dist.distance;

                // 슬라이드 플랫폼 있다면 슬라이드 강제 중단 요청
                if (slidePlatform != null)
                    slidePlatform.CancelSlideDueToSlope(other.gameObject);

                controller.isInputBlocked = false;
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            // 허용 방향일 때, 슬라이드는 OneWaySlope가 관여하지 않음
            // 필요시 별도 처리 가능
        }
    }

    private Vector2 DirectionToVector(Direction dir)
    {
        return dir switch
        {
            Direction.Up => Vector2.up,
            Direction.Down => Vector2.down,
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            _ => Vector2.zero,
        };
    }
}

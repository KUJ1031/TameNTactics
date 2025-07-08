using UnityEngine;
using System.Collections.Generic;

public class SlidePlatform : MonoBehaviour
{
    public float slideForce = 5f;

    private class SlideInfo
    {
        public Vector2 direction;
        public Vector2 previousPosition;
        public float stuckTimer;
        public bool isSliding;
        public bool isWaitingForInput;
    }

    private Dictionary<GameObject, SlideInfo> slideInfos = new();
    private float stuckCheckTime = 0.1f;

    // 플레이어-박스 충돌 무시 관리를 위한 딕셔너리
    private Dictionary<Collider2D, Collider2D> ignoredCollisions = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (input == Vector2.zero) input = Vector2.down;

        slideInfos[other.gameObject] = new SlideInfo
        {
            direction = input.normalized,
            previousPosition = other.attachedRigidbody.position,
            stuckTimer = 0f,
            isSliding = true,
            isWaitingForInput = false
        };

        var controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.isInputBlocked = true;
            controller.isSliding = true;
            controller.slideDirection = input.normalized;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var rb = other.attachedRigidbody;
        var controller = other.GetComponent<PlayerController>();
        if (rb == null || controller == null) return;

        if (!slideInfos.TryGetValue(other.gameObject, out var info)) return;

        if (info.isSliding)
        {
            // 박스 밀기 체크
            bool isPushingBox = CheckIfPushingBox(rb, info.direction);

            if (isPushingBox)
            {
                // 박스 밀 때는 플레이어가 움직이지 않도록 velocity 0
                rb.velocity = Vector2.zero;
                controller.isInputBlocked = true;

                // 플레이어-박스 충돌 무시 시작
                var playerCol = controller.GetComponent<Collider2D>();
                var boxCol = GetBoxColliderInDirection(rb.position, info.direction);
                if (playerCol != null && boxCol != null && !ignoredCollisions.ContainsKey(playerCol))
                {
                    Physics2D.IgnoreCollision(playerCol, boxCol, true);
                    ignoredCollisions[playerCol] = boxCol;
                }
            }
            else
            {
                // 박스 안 밀 때는 슬라이드 방향으로 이동 유지
                rb.velocity = info.direction * slideForce;

                // 충돌 다시 활성화 (박스와 충돌 무시 해제)
                var playerCol = controller.GetComponent<Collider2D>();
                if (playerCol != null && ignoredCollisions.TryGetValue(playerCol, out var boxCol))
                {
                    Physics2D.IgnoreCollision(playerCol, boxCol, false);
                    ignoredCollisions.Remove(playerCol);
                }
            }

            info.stuckTimer += Time.deltaTime;
            if (info.stuckTimer >= stuckCheckTime)
            {
                info.stuckTimer = 0f;

                Vector2 currentPosition = rb.position;
                float movedDistance = Vector2.Distance(currentPosition, info.previousPosition);

                if (movedDistance < 0.05f)
                {
                    info.isSliding = false;
                    info.isWaitingForInput = true;
                    rb.velocity = Vector2.zero;
                    controller.isInputBlocked = false;

                    // 충돌 무시 해제
                    var playerCol = controller.GetComponent<Collider2D>();
                    if (playerCol != null && ignoredCollisions.TryGetValue(playerCol, out var boxCol))
                    {
                        Physics2D.IgnoreCollision(playerCol, boxCol, false);
                        ignoredCollisions.Remove(playerCol);
                    }
                }
                info.previousPosition = currentPosition;
            }
        }
        else if (info.isWaitingForInput)
        {
            controller.isInputBlocked = false;

            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (input != Vector2.zero)
            {
                info.direction = input.normalized;
                info.isSliding = true;
                info.isWaitingForInput = false;
                controller.isInputBlocked = true;
            }
        }
    }

    private bool CheckIfPushingBox(Rigidbody2D rb, Vector2 dir)
    {
        float boxDetectDistance = 0.6f;
        RaycastHit2D hit = Physics2D.Raycast(rb.position, dir, boxDetectDistance);
        if (hit.collider != null && hit.collider.CompareTag("Box"))
        {
            return true;
        }
        return false;
    }

    private Collider2D GetBoxColliderInDirection(Vector2 position, Vector2 dir)
    {
        float boxDetectDistance = 0.6f;
        RaycastHit2D hit = Physics2D.Raycast(position, dir, boxDetectDistance);
        if (hit.collider != null && hit.collider.CompareTag("Box"))
        {
            return hit.collider;
        }
        return null;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        slideInfos.Remove(other.gameObject);

        var controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.isInputBlocked = false;
            controller.isSliding = false;
            controller.slideDirection = Vector2.zero;
        }

        var rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = Vector2.zero;

        // 충돌 무시 해제
        var playerCol = other.GetComponent<Collider2D>();
        if (playerCol != null && ignoredCollisions.TryGetValue(playerCol, out var boxCol))
        {
            Physics2D.IgnoreCollision(playerCol, boxCol, false);
            ignoredCollisions.Remove(playerCol);
        }
    }

    public void CancelSlideDueToSlope(GameObject player)
    {
        if (!slideInfos.ContainsKey(player)) return;

        var info = slideInfos[player];
        info.isSliding = false;
        info.isWaitingForInput = true;

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = Vector2.zero;

        var controller = player.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.isInputBlocked = false;
            controller.isSliding = false;
            controller.slideDirection = Vector2.zero;
        }

        var playerCol = player.GetComponent<Collider2D>();
        if (playerCol != null && ignoredCollisions.TryGetValue(playerCol, out var boxCol))
        {
            Physics2D.IgnoreCollision(playerCol, boxCol, false);
            ignoredCollisions.Remove(playerCol);
        }

        Debug.Log("슬라이드 강제 중단됨: OneWaySlope 요청");
    }
}

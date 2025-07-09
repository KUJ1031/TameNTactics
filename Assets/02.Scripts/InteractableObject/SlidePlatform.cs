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
        if (input == Vector2.zero) return;



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
        // 충돌한 객체가 "Player" 태그가 아니면 무시
        if (!other.CompareTag("Player")) return;

        var rb = other.attachedRigidbody;
        var controller = other.GetComponent<PlayerController>();

        // Rigidbody 또는 PlayerController가 없으면 무시
        if (rb == null || controller == null) return;

        // 해당 플레이어에 대한 슬라이드 정보가 없으면 무시
        if (!slideInfos.TryGetValue(other.gameObject, out var info)) return;

        // 슬라이드 중일 때의 처리
        if (info.isSliding)
        {
            // 1. 현재 슬라이드 방향으로 박스를 밀고 있는지 체크
            bool isPushingBox = CheckIfPushingBox(rb, info.direction);

            if (isPushingBox)
            {
                // → 박스를 밀고 있을 경우
                // 플레이어의 움직임 중단
                rb.velocity = Vector2.zero;
                controller.isInputBlocked = true;

                // 플레이어와 박스의 충돌 무시 (겹쳐도 튕기지 않도록)
                var playerCol = controller.GetComponent<Collider2D>();
                var boxCol = GetBoxColliderInDirection(rb.position, info.direction);

                if (playerCol != null && boxCol != null && !ignoredCollisions.ContainsKey(playerCol))
                {
                    Physics2D.IgnoreCollision(playerCol, boxCol, true); // 충돌 무시
                    ignoredCollisions[playerCol] = boxCol;              // 나중에 복원할 수 있도록 저장
                }
            }
            else
            {
                // → 박스를 밀고 있지 않으면 슬라이드 계속 진행
                rb.velocity = info.direction * slideForce;

                // 이전에 무시했던 충돌 복원 (다시 충돌하게 설정)
                var playerCol = controller.GetComponent<Collider2D>();
                if (playerCol != null && ignoredCollisions.TryGetValue(playerCol, out var boxCol))
                {
                    Physics2D.IgnoreCollision(playerCol, boxCol, false); // 충돌 복원
                    ignoredCollisions.Remove(playerCol);
                }
            }

            // 2. 일정 시간마다 이동 정체 여부를 검사
            info.stuckTimer += Time.deltaTime;
            if (info.stuckTimer >= stuckCheckTime)
            {
                info.stuckTimer = 0f;

                // 이동 거리를 측정하여 거의 안 움직였으면 슬라이드 중단
                Vector2 currentPosition = rb.position;
                float movedDistance = Vector2.Distance(currentPosition, info.previousPosition);

                if (movedDistance < 0.05f)
                {
                    // 슬라이드 중단 및 입력 대기 상태로 전환
                    info.isSliding = false;
                    info.isWaitingForInput = true;
                    rb.velocity = Vector2.zero;
                    controller.isInputBlocked = false;
                    // 충돌 무시했던 것도 복원
                    var playerCol = controller.GetComponent<Collider2D>();
                    if (playerCol != null && ignoredCollisions.TryGetValue(playerCol, out var boxCol))
                    {
                        Physics2D.IgnoreCollision(playerCol, boxCol, false);
                        ignoredCollisions.Remove(playerCol);
                    }
                }

                // 다음 비교를 위한 위치 저장
                info.previousPosition = currentPosition;
            }
        }
        // 슬라이드가 끝났고, 입력을 기다리는 상태일 때
        else if (info.isWaitingForInput)
        {
            // 입력 차단 해제
           // controller.isInputBlocked = false;

            // 플레이어가 다시 방향키 입력 시 슬라이드 재시작
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (input != Vector2.zero)
            {
                // 새 방향으로 슬라이드 시작
                info.direction = input.normalized;
                info.isSliding = true;
                info.isWaitingForInput = false;

                // 입력 차단 (슬라이드 중엔 수동 입력 금지)
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
        if (!slideInfos.TryGetValue(player, out var info)) return;

        info.isSliding = false;
        info.isWaitingForInput = true;

        var controller = player.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.isInputBlocked = false;
            controller.isSliding = false;
            controller.slideDirection = Vector2.zero;
        }

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = Vector2.zero;

        var playerCol = player.GetComponent<Collider2D>();
        if (playerCol != null && ignoredCollisions.TryGetValue(playerCol, out var boxCol))
        {
            Physics2D.IgnoreCollision(playerCol, boxCol, false);
            ignoredCollisions.Remove(playerCol);
        }
    }
}

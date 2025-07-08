// SlidePlatform.cs
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
            controller.isInputBlocked = true;
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
            rb.velocity = info.direction * slideForce;

            info.stuckTimer += Time.deltaTime;
            if (info.stuckTimer >= stuckCheckTime)
            {
                info.stuckTimer = 0f;

                Vector2 currentPosition = rb.position;
                float movedDistance = Vector2.Distance(currentPosition, info.previousPosition);

                if (movedDistance < 0.05f)
                {
                    // 막힘 → 입력 대기 상태
                    info.isSliding = false;
                    info.isWaitingForInput = true;
                    rb.velocity = Vector2.zero;
                    controller.isInputBlocked = false;
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

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        slideInfos.Remove(other.gameObject);

        var controller = other.GetComponent<PlayerController>();
        if (controller != null)
            controller.isInputBlocked = false;

        var rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = Vector2.zero;
    }

    // OneWaySlope에서 슬라이드 강제 중단 시 호출
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
            controller.isInputBlocked = false;

        Debug.Log("슬라이드 강제 중단됨: OneWaySlope 요청");
    }
}

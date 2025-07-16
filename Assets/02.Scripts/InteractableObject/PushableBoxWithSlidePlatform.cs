using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBoxWithSlidePlatform : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed;

    private bool isMoving = false;
    private bool isStopped = false;

    private Vector3 currentDirection;
    private bool isInTeleportCooldown = false;

    private PlayerController player;
    private Collider2D playerCol;
    private Collider2D boxCol;

    public CinemachineVirtualCamera virtualCamera;
    private void Start()
    {
        virtualCamera  = PlayerManager.Instance.virtualCamera;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var controller = collision.gameObject.GetComponent<PlayerController>();
        if (collision.gameObject.CompareTag("Collider"))
        {

            GlobalCamera.Instance.SetFollow(player.transform);

            //if (playerCol != null && boxCol != null)
            //    Physics2D.IgnoreCollision(playerCol, boxCol, false);

            isMoving = false;
            isStopped = true;

            StopAllCoroutines(); // MoveBox 중단
            return;
        }

        if (!isStopped && collision.gameObject.CompareTag("Player"))
        {

            if (controller == null) return;

            controller.isInputBlocked = true; // 플레이어 입력 차단

            Vector2 input = controller.GetMoveInput();
            Vector2 dir = Vector2.zero;

            if (input == Vector2.zero)
            {
                dir = Vector2.zero; // 입력 없을 때는 정지
            }
            else
            {
                // x, y 절댓값 비교해서 더 큰 쪽을 기준으로 방향 결정
                if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                {
                    dir = input.x > 0 ? Vector2.right : Vector2.left;
                }
                else
                {
                    dir = input.y > 0 ? Vector2.up : Vector2.down;
                }
            }
            Vector3 forcedDir = new Vector3(dir.x, dir.y, 0f);
            // Vector3 forcedDir = Vector3.right;

            Debug.Log("최종 방향 : " + forcedDir);

            GlobalCamera.Instance.SetFollow(transform);

            StartCoroutine(MoveBox(forcedDir, controller));
        }
    }
    public void TryPush(Vector3 direction, PlayerController player = null)
    {
        if (isMoving) return;
        StartCoroutine(MoveBox(direction.normalized, player));
    }

    private IEnumerator MoveBox(Vector3 direction, PlayerController player)
    {
        this.player = player;

        isMoving = true;
        currentDirection = direction;

        Collider2D playerCol = null, boxCol = GetComponent<Collider2D>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector3 originalPos = transform.position;

        if (player != null)
        {

            player.rb.velocity = Vector2.zero;

            playerCol = player.GetComponent<Collider2D>();
            //if (playerCol && boxCol)
            //    Physics2D.IgnoreCollision(playerCol, boxCol, true);

           // SlidePlatform.CancelPlayerSlide(player.gameObject);
        }

        Vector3 targetPos = originalPos + direction.normalized * moveDistance;

        try
        {
            Debug.Log("박스 이동 시작: " + originalPos + " -> " + targetPos);
            while ((rb.position - (Vector2)targetPos).sqrMagnitude > 0.01f)
            {

                Vector2 newPos = Vector2.MoveTowards(rb.position, (Vector2)targetPos, moveSpeed * Time.deltaTime);
                rb.MovePosition(newPos);

                if (player != null)
                    player.rb.velocity = Vector2.zero;

                yield return null;
            }

            rb.MovePosition(targetPos);
        }
        finally
        {

        }
    }
    public bool IsMoving()
    {
        return isMoving;
    }

    public void StartTeleportCooldown(float duration)
    {
        StartCoroutine(TeleportCooldownRoutine(duration));
    }

    private IEnumerator TeleportCooldownRoutine(float duration)
    {
        isInTeleportCooldown = true;
        yield return new WaitForSeconds(duration);
        isInTeleportCooldown = false;
    }

    public bool IsInTeleportCooldown()
    {
        return isInTeleportCooldown;
    }
}

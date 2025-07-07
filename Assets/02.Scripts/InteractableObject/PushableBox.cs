using UnityEngine;
using System.Collections;

public class PushableBox : MonoBehaviour
{
    [Header("오브젝트가 한 번 이동할 거리 (유닛 단위)")]
    public float moveDistance = 1f;

    [Header("오브젝트 이동 속도 (유닛/초)")]
    public float moveSpeed;

    [Header("오브젝트가 움직이는 중인지 체크하는 내부 플래그")]
    private bool isMoving = false;

    [Header("오브젝트 이동 시 플레이어도 함께 움직이도록 할지 여부")]
    public bool isPlayerMovingWithBox = true;  // 플레이어가 박스를 밀 때 같이 이동하게 함

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isMoving) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            var playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController == null) return;

            Vector2 input = playerController.GetMoveInput();
            Debug.Log("입력: " + input);

            if (input != Vector2.zero)
            {
                Vector3 dir = Vector3.zero;
                if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                    dir = input.x > 0 ? Vector3.right : Vector3.left;
                else
                    dir = input.y > 0 ? Vector3.up : Vector3.down;

                StartCoroutine(MoveBox(dir, playerController));
            }
        }
    }

    private IEnumerator MoveBox(Vector3 direction, PlayerController player)
    {
        isMoving = true;

        if (player != null)
        {
            // 플레이어 입력 잠금 및 현재 속도 초기화
            player.isInputBlocked = true;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        Vector3 target = transform.position + direction * moveDistance;
        Vector3 playerStartPos = player.transform.position;
        Vector3 playerTarget = playerStartPos + direction * moveDistance;

        while ((transform.position - target).sqrMagnitude > 0.01f)
        {
            // 박스를 목표 지점으로 부드럽게 이동
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            if (isPlayerMovingWithBox && player != null)
            {
                // 박스와 동일한 속도로 플레이어 위치도 함께 이동
                player.transform.position = Vector3.MoveTowards(player.transform.position, playerTarget, moveSpeed * Time.deltaTime);
            }

            yield return null;
        }

        // 정확한 위치 보정
        transform.position = target;

        if (isPlayerMovingWithBox && player != null)
        {
            player.transform.position = playerTarget;
        }

        if (player != null)
            player.isInputBlocked = false;  // 입력 잠금 해제

        isMoving = false;
    }
}

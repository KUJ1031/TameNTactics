using System.Collections;
using UnityEngine;

public class PushableBox : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed;
    public bool isPlayerMovingWithBox = false;

    private bool isMoving = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isMoving) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            var controller = collision.gameObject.GetComponent<PlayerController>();
            if (controller == null) return;

            Vector2 input = controller.isSliding ? controller.slideDirection : controller.GetMoveInput();

            if (input != Vector2.zero)
            {
                Vector3 dir = (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                    ? (input.x > 0 ? Vector3.right : Vector3.left)
                    : (input.y > 0 ? Vector3.up : Vector3.down);

                StartCoroutine(MoveBox(dir, controller));
            }
        }
    }

    public void TryPush(Vector3 direction, PlayerController player = null)
    {
        if (isMoving) return;
        StartCoroutine(MoveBox(direction.normalized, player));
    }

    private IEnumerator MoveBox(Vector3 direction, PlayerController player)
    {
        isMoving = true;

        if (player != null)
        {
            player.isInputBlocked = true;
            player.rb.velocity = Vector2.zero;  // 이동 멈춤
        }

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + direction * moveDistance;

        while ((transform.position - targetPos).sqrMagnitude > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (player != null)
            {
                player.rb.velocity = Vector2.zero;  // 계속 멈춰있게 강제
            }

            yield return null;
        }

        transform.position = targetPos;

        if (player != null)
        {
            player.isInputBlocked = false;
            player.rb.velocity = Vector2.zero; // 마지막에 한번 더 멈추게
        }

        isMoving = false;
    }

}

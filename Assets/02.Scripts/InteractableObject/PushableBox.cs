using System.Collections;
using UnityEngine;

public class PushableBox : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed;

    private bool isMoving = false;

    private void OnCollisionStay2D(Collision2D collision)
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
            player.isInputBlocked = true;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        Vector3 target = transform.position + direction * moveDistance;

        while ((transform.position - target).sqrMagnitude > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = target;

        if (player != null)
            player.isInputBlocked = false;

        isMoving = false;
    }
}
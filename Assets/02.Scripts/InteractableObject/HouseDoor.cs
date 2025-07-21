using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class HouseDoor : MonoBehaviour
{
    public GameObject outdoorGroup;
    public GameObject indoorGroup;
    public float fadeDuration = 1f;
    public Collider2D doorCollider;  // 문 콜라이더 참조 추가

    private bool isInside = false;
    private bool playerInRange = false;
    private Transform playerTransform;
    public float resetDistance = 5f;

    public TilemapRenderer doorSpriteRenderer;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            isInside = !isInside;
            StartCoroutine(FadeTiles());

            // 문 충돌 제거 및 outdoorGroup 콜라이더 끄기
            doorCollider.enabled = !isInside;
            SetGroupColliderEnabled(outdoorGroup, !isInside);
        }

        // 문에서 일정 거리 이상 멀어지면 다시 콜라이더 활성화
        if (isInside && playerTransform != null)
        {
            if (isInside && playerTransform != null)
            {
                Vector2 playerPos = playerTransform.position;
                Vector2 colliderPos = (Vector2)doorCollider.transform.position + doorCollider.offset;
                float distance = Vector2.Distance(playerPos, colliderPos);
                Debug.Log($"Distance to door: {distance}");

                if (distance > resetDistance)
                {
                    isInside = false;
                    SetAlphaForGroup(indoorGroup, 0f);
                    SetAlphaForGroup(outdoorGroup, 1f);

                    doorCollider.enabled = true;
                    SetGroupColliderEnabled(outdoorGroup, true);

                    if (doorSpriteRenderer != null)
                        doorSpriteRenderer.enabled = !isInside;
                }
            }
        }
    }

    IEnumerator FadeTiles()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            float alpha = timer / fadeDuration;
            float indoorAlpha = isInside ? alpha : 1 - alpha;
            float outdoorAlpha = 1 - indoorAlpha;

            SetAlphaForGroup(indoorGroup, indoorAlpha);
            SetAlphaForGroup(outdoorGroup, outdoorAlpha);

            timer += Time.deltaTime;
            yield return null;
        }

        SetAlphaForGroup(indoorGroup, isInside ? 1f : 0f);
        SetAlphaForGroup(outdoorGroup, isInside ? 0f : 1f);
    }

    void SetAlphaForGroup(GameObject group, float alpha)
    {
        foreach (var r in group.GetComponentsInChildren<TilemapRenderer>())
        {
            Color c = r.material.color;
            c.a = alpha;
            r.material.color = c;
        }
    }

    void SetGroupColliderEnabled(GameObject group, bool enabled)
    {
        foreach (var col in group.GetComponentsInChildren<Collider2D>())
        {
            col.enabled = enabled;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isInside && collision.collider.CompareTag("Player"))
        {
            playerInRange = true;
            playerTransform = collision.transform;
            Debug.Log("Player entered the door range. Press 'E' to toggle.");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!isInside && collision.collider.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player exited the door range.");
        }
    }
}
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 moveDir;
    public float lifeTime = 3f;

    private BulletPool pool;

    public void Init(Vector2 direction, BulletPool assignedPool)
    {
        moveDir = direction.normalized;
        pool = assignedPool;
        Invoke(nameof(Deactivate), lifeTime);
    }

    private void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime);
    }

    private void Deactivate()
    {
        if (pool != null)
            pool.ReturnBullet(gameObject);  // 여기서 꼭 호출되어야 함!
        else
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어와 충돌 시 추가 로직 (예: 데미지 처리 등)
            Debug.Log("으악 아파!");
        }
        // 다른 충돌 처리 로직이 필요하면 여기에 추가
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("총알과 충돌: 움직임 억제");

            // 박스 Rigidbody가 있으면 움직임 강제로 정지
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            // 총알 비활성화 처리
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.Deactivate();
            }
        }
    }
}

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어와 충돌 시 추가 로직 (예: 데미지 처리 등)
            Debug.Log("으악 아파!");
        }
        // 다른 충돌 처리 로직이 필요하면 여기에 추가
        else if (collision.gameObject.CompareTag("BulletDisappear"))
        {

            // 박스 Rigidbody2D 가져오기
            Rigidbody2D boxRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (boxRb != null)
            {
                // 박스가 밀리지 않도록 속도 강제로 0으로
                boxRb.velocity = Vector2.zero;
                boxRb.angularVelocity = 0f;
            }

            // 적과 충돌 시 추가 로직 (예: 데미지 처리 등)
            Deactivate();
        }
    }
}

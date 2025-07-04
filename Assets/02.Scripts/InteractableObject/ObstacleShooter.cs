using UnityEngine;

public class ObstacleShooter : MonoBehaviour
{
    public BulletPool bulletPool;
    public float fireRate = 1f;
    public Vector2 shootDirection = Vector2.right;

    private void Start()
    {
        InvokeRepeating(nameof(Shoot), 0f, fireRate);
    }

    private void Shoot()
    {
        GameObject bulletObj = bulletPool.GetBullet();
        if (bulletObj == null)
        {
            Debug.Log("총알을 얻지 못했습니다. 모든 총알이 사용 중입니다!");
            return;  // null일 때 더 이상 진행하지 않음
        }

        bulletObj.transform.position = transform.position;
        bulletObj.transform.rotation = Quaternion.identity;

        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.Init(shootDirection, bulletPool);
    }
}

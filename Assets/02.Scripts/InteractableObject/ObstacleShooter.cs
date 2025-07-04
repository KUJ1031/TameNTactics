using UnityEngine;

public class ObstacleShooter : MonoBehaviour
{
    [Header("총알 오브젝트를 관리하는 BulletPool 컴포넌트")]
    public BulletPool bulletPool;

    [Header("발사 간격 (초 단위)")]
    public float fireRate = 1f;

    [Header("총알이 발사될 방향")]
    public Vector2 shootDirection = Vector2.right;

    // 시작 시 일정 간격으로 총알 발사 시작
    private void Start()
    {
        // 지정된 fireRate 간격으로 Shoot 메서드 반복 호출
        InvokeRepeating(nameof(Shoot), 0f, fireRate);
    }

    // 총알 풀에서 총알을 가져와 설정된 방향으로 발사
    private void Shoot()
    {
        // 사용 가능한 총알 가져오기
        GameObject bulletObj = bulletPool.GetBullet();

        if (bulletObj == null)
        {
            Debug.Log("총알을 얻지 못했습니다. 모든 총알이 사용 중입니다!");
            return;  // 총알 풀에서 가져올 수 없을 경우 아무것도 하지 않음
        }

        // 총알 위치 및 회전 초기화
        bulletObj.transform.position = transform.position;
        bulletObj.transform.rotation = Quaternion.identity;

        // 총알 발사 방향 및 풀 정보 전달
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.Init(shootDirection, bulletPool);
    }
}

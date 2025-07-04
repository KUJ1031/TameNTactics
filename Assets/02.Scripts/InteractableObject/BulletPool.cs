using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int poolSize = 5;

    private Queue<GameObject> pool = new();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetBullet()
    {
        foreach (var bullet in pool)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }

        // 모두 사용 중이면, 필요 시 새로 생성 (풀 제한 있음)
        if (pool.Count >= poolSize)
        {
            Debug.LogWarning("모든 총알이 사용 중이고, 풀 사이즈를 초과할 수 없습니다!");
            return null;
        }

        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(true);
        pool.Enqueue(newBullet);
        return newBullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        pool.Enqueue(bullet);
    }
}

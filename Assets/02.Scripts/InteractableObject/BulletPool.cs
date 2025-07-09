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
        int loopCount = pool.Count;
        for (int i = 0; i < loopCount; i++)
        {
            GameObject bullet = pool.Dequeue();
            pool.Enqueue(bullet); // 다시 넣음

            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }

        if (pool.Count >= poolSize)
        {
            Debug.LogWarning("모든 총알이 사용 중이고, 풀 사이즈 초과");
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

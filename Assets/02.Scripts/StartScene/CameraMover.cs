using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Vector2 moveAreaMin;   // 이동 가능한 영역의 최소 좌표 (X, Y)
    public Vector2 moveAreaMax;   // 이동 가능한 영역의 최대 좌표 (X, Y)
    public float moveSpeed = 2f;  // 이동 속도
    public float waitTime = 1f;   // 도착 후 다음 이동까지 대기 시간

    private Vector3 targetPos;
    private bool isWaiting = false;

    void Start()
    {
        PickNewTarget();
    }

    void Update()
    {
        if (isWaiting) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            StartCoroutine(WaitAndMove());
        }
    }

    private System.Collections.IEnumerator WaitAndMove()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        PickNewTarget();
        isWaiting = false;
    }

    void PickNewTarget()
    {
        float randomX = Random.Range(moveAreaMin.x, moveAreaMax.x);
        float randomY = Random.Range(moveAreaMin.y, moveAreaMax.y);
        targetPos = new Vector3(randomX, randomY, transform.position.z); // Z는 고정
    }
}

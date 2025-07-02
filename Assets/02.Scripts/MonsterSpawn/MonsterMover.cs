using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MonsterMover : MonoBehaviour
{
    [Header("이동 속도 관련")]
    public float patrolSpeed = 1f;    // 순찰 속도
    public float chaseSpeed = 2f;     // 추격/도망 속도
    public float sightRadius = 4f;    // 시야 범위

    // 내부 참조 변수들
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 startPosition;
    private BoxCollider2D factoryBounds;
    private Transform player;
    private MonsterData monsterData;

    // 상태 변수
    private bool isPlayerInSight = false;
    private bool isEscaping = false;
    private Vector2 escapeTarget;

    [Header("정찰 - 멈춤 관련")]
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private float waitDuration = 2f;

    private float moveTimer = 0f;
    private float moveDuration = 2f; // 정찰 시 걷는 시간

    // 외부에서 이동 영역을 지정해줄 수 있음
    public void SetMoveArea(BoxCollider2D bounds)
    {
        factoryBounds = bounds;
    }

    private void Start()
    {
        // 초기화
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        startPosition = rb.position;

        // 몬스터 데이터 참조
        monsterData = GetComponent<MonsterCharacter>()?.monster.monsterData;
    }

    private void Update()
    {
        // 필수 요소들이 없으면 처리 안 함
        if (player == null || factoryBounds == null || monsterData == null) return;

        // 플레이어 감지 로직
        float distance = Vector2.Distance(rb.position, player.position);
        bool inFactory = factoryBounds.bounds.Contains(player.position);
        isPlayerInSight = distance < sightRadius && inFactory;

        // 도망 성격일 경우 도망 타겟 설정
        if (monsterData.personality == Personality.Timid)
        {
            if (isPlayerInSight && !isEscaping)
            {
                escapeTarget = GetEscapePoint(); // 도망 위치 계산
                isEscaping = true;
            }
            else if (!isPlayerInSight)
            {
                isEscaping = false;
            }
        }
    }

    private void FixedUpdate()
    {
        // 이동 처리 (프레임 간 일관성 보장)
        if (monsterData == null || factoryBounds == null) return;

        switch (monsterData.personality)
        {
            case Personality.Aggressive:
                if (isPlayerInSight)
                    MoveToward(player.position, chaseSpeed); // 추격
                else
                    Patrol(); // 순찰
                break;

            case Personality.Timid:
                if (isEscaping)
                    MoveToward(escapeTarget, chaseSpeed); // 도망
                else
                    Patrol(); // 순찰
                break;

            case Personality.Normal:
                Patrol(); // 평범한 몬스터는 무조건 순찰
                break;
        }
    }

    /// <summary>
    /// 순찰 행동 처리: 일정 시간 걷고 멈추는 방식
    /// </summary>
    private void Patrol()
    {
        if (isWaiting)
        {
            // 멈춰 있는 시간 처리
            waitTimer -= Time.fixedDeltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;

                // 걷는 시간과 방향 설정
                moveDuration = Random.Range(2f, 4f);
                moveTimer = moveDuration;
                moveDirection = Random.insideUnitCircle.normalized;
            }

            return;
        }

        // 걷고 있는 경우
        moveTimer -= Time.fixedDeltaTime;
        if (moveTimer <= 0f)
        {
            isWaiting = true;
            waitDuration = Random.Range(1.5f, 3.5f); // 다음 멈춤 시간 설정
            waitTimer = waitDuration;
            return;
        }

        // 이동 위치 계산
        Vector2 next = rb.position + moveDirection * patrolSpeed * Time.fixedDeltaTime;
        if (factoryBounds.bounds.Contains(next))
        {
            rb.MovePosition(next);
        }
        else
        {
            moveDirection = -moveDirection; // 경계에 닿으면 방향 반전
        }
    }

    /// <summary>
    /// 대상 위치로 이동하는 함수 (추격 or 도망)
    /// </summary>
    private void MoveToward(Vector2 target, float speed)
    {
        Vector2 dir = (target - rb.position).normalized;
        Vector2 next = rb.position + dir * speed * Time.fixedDeltaTime;

        if (factoryBounds.bounds.Contains(next))
            rb.MovePosition(next);
    }

    /// <summary>
    /// 도망 성격일 때 플레이어로부터 반대 방향으로 도망치는 위치 계산
    /// </summary>
    private Vector2 GetEscapePoint()
    {
        Vector2 awayDir = (rb.position - (Vector2)player.position).normalized;
        Vector2 candidate = rb.position + awayDir * 5f;

        // 영역 밖으로 벗어나지 않게 클램핑
        Bounds b = factoryBounds.bounds;
        candidate.x = Mathf.Clamp(candidate.x, b.min.x + 0.5f, b.max.x - 0.5f);
        candidate.y = Mathf.Clamp(candidate.y, b.min.y + 0.5f, b.max.y - 0.5f);

        return candidate;
    }
}

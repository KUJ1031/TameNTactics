using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MonsterMover : MonoBehaviour
{
    public float patrolSpeed = 1f;
    public float chaseSpeed = 2f;
    public float sightRadius = 4f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 startPosition;
    private BoxCollider2D factoryBounds;
    private Transform player;
    private Monster monsterData;

    private bool isPlayerInSight = false;
    private bool isEscaping = false;
    private Vector2 escapeTarget;

    public void SetMoveArea(BoxCollider2D bounds)
    {
        factoryBounds = bounds;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        startPosition = rb.position;
        monsterData = GetComponent<Monster>();

        PickRandomDirection();
        InvokeRepeating(nameof(PickRandomDirection), 0f, 2f);
    }

    private void Update()
    {
        if (player == null || factoryBounds == null || monsterData == null) return;

        float distance = Vector2.Distance(rb.position, player.position);
        bool inFactory = factoryBounds.bounds.Contains(player.position);
        isPlayerInSight = distance < sightRadius && inFactory;

        if (monsterData.personality == Personality.소심함)
        {
            if (isPlayerInSight && !isEscaping)
            {
                escapeTarget = GetEscapePoint();
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
        if (monsterData == null || factoryBounds == null) return;

        switch (monsterData.personality)
        {
            case Personality.호전적임:
                if (isPlayerInSight)
                    MoveToward(player.position, chaseSpeed);
                else
                    Patrol();
                break;

            case Personality.소심함:
                if (isEscaping)
                {
                    MoveToward(escapeTarget, chaseSpeed);
                }
                else
                {
                    Patrol();
                }
                break;

            case Personality.평범함:
                    Patrol();
                break;
        }
    }

    private void Patrol()
    {
        Vector2 next = rb.position + moveDirection * patrolSpeed * Time.fixedDeltaTime;
        if (factoryBounds.bounds.Contains(next))
        {
            rb.MovePosition(next);
        }
        else
        {
            moveDirection = -moveDirection;
        }
    }

    private void MoveToward(Vector2 target, float speed)
    {
        Vector2 dir = (target - rb.position).normalized;
        Vector2 next = rb.position + dir * speed * Time.fixedDeltaTime;
        if (factoryBounds.bounds.Contains(next))
            rb.MovePosition(next);
    }

    private Vector2 GetEscapePoint()
    {
        Vector2 awayDir = (rb.position - (Vector2)player.position).normalized;
        Vector2 candidate = rb.position + awayDir * 5f;

        Bounds b = factoryBounds.bounds;
        candidate.x = Mathf.Clamp(candidate.x, b.min.x + 0.5f, b.max.x - 0.5f);
        candidate.y = Mathf.Clamp(candidate.y, b.min.y + 0.5f, b.max.y - 0.5f);

        return candidate;
    }

    private void PickRandomDirection()
    {
        if (!isPlayerInSight)
            moveDirection = Random.insideUnitCircle.normalized;
    }
}

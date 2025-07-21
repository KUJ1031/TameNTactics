using UnityEngine;
using System.Collections;

public class RandomAnimationPlayer : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float changeDirectionInterval = 1.5f;

    [Header("멈춤 설정")]
    [Range(0f, 1f)] public float stopChance = 0.3f; // 멈출 확률
    public float stopDuration = 0.5f;               // 멈춰있는 시간

    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private SPUM_Prefabs spumPrefab;

    private PlayerState currentState = PlayerState.IDLE;

    private bool isPlayingSpecialAnimation = false;
    private bool isStoppedByRandom = false;
    private Vector3 lastPosition;
    private bool isJumping = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spumPrefab = GetComponent<SPUM_Prefabs>();

        if (spumPrefab._anim != null)
        {
            spumPrefab._anim.speed = 0.3f;
        }

        lastPosition = transform.position;

        StartCoroutine(ChangeDirectionRoutine());
        StartCoroutine(RandomAnimationLoop());

        lastPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            if (hit != null && hit.gameObject == gameObject)
            {
                StartCoroutine(FastJump());
            }
        }
    }


    private IEnumerator FastJump()
    {
        isJumping = true;
        Vector3 jumpStartPos = transform.position; // 현재 위치 저장

        float jumpHeight = 0.5f;
        float jumpTime = 0.15f;
        float elapsed = 0f;

        while (elapsed < jumpTime)
        {
            float t = elapsed / jumpTime;
            // 점프 중 위치 보간 (위로 이동 후 다시 내려옴)
            float height = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            transform.position = jumpStartPos + Vector3.up * height;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = jumpStartPos; // 원위치 복귀
        isJumping = false;
    }


    private void FixedUpdate()
    {
        if (isPlayingSpecialAnimation)
        {
            rb.velocity = Vector2.zero;
            if (currentState != PlayerState.OTHER)
            {
                spumPrefab.PlayAnimation(PlayerState.OTHER, 0);
                currentState = PlayerState.OTHER;
            }
            return;
        }

        if (isStoppedByRandom)
        {
            rb.velocity = Vector2.zero;
            if (currentState != PlayerState.IDLE)
            {
                spumPrefab.PlayAnimation(PlayerState.IDLE, 0);
                currentState = PlayerState.IDLE;
            }
        }
        else
        {
            rb.velocity = moveDirection * moveSpeed;

            float movedDistance = Vector3.Distance(transform.position, lastPosition);

            if (movedDistance > 0.0001f)
            {
                if (currentState != PlayerState.MOVE)
                {
                    spumPrefab.PlayAnimation(PlayerState.MOVE, 0);
                    currentState = PlayerState.MOVE;
                }
            }
            else
            {
                if (currentState != PlayerState.IDLE)
                {
                    spumPrefab.PlayAnimation(PlayerState.IDLE, 0);
                    currentState = PlayerState.IDLE;
                }
            }

            // 좌우 반전 처리
            if (moveDirection.x != 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x) * (moveDirection.x > 0 ? -1 : 1);
                transform.localScale = scale;
            }
        }

        lastPosition = transform.position;
    }



    private IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            if (isPlayingSpecialAnimation)
            {
                moveDirection = Vector2.zero;
                isStoppedByRandom = false;
            }
            else
            {
                if (Random.value < stopChance)
                {
                    // 멈춤 상태 진입
                    moveDirection = Vector2.zero;
                    isStoppedByRandom = true;
                    yield return new WaitForSeconds(stopDuration);
                    isStoppedByRandom = false;
                }
                else
                {
                    moveDirection = GetRandomDirection();
                }
            }

            yield return new WaitForSeconds(changeDirectionInterval);
        }
    }

    private Vector2 GetRandomDirection()
    {
        float angle = Random.Range(0f, 360f);
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    private IEnumerator RandomAnimationLoop()
    {
        PlayerState[] possibleStates = new PlayerState[] { PlayerState.ATTACK, PlayerState.DAMAGED, PlayerState.DEBUFF, PlayerState.OTHER };
        float playChance = 1f;
        float minInterval = 7.5f;
        float maxInterval = 9.0f;
        float slowDuration = 1.5f;

        if (spumPrefab == null)
            yield break;

        spumPrefab.OverrideControllerInit();

        while (true)
        {
            float interval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(interval);

            if (Random.value < playChance)
            {
                // 상태를 랜덤 선택
                PlayerState playState = possibleStates[Random.Range(0, possibleStates.Length)];

                if (spumPrefab.StateAnimationPairs.TryGetValue(playState.ToString(), out var animList))
                {
                    if (animList != null && animList.Count > 0)
                    {
                        int randIndex = Random.Range(0, animList.Count);

                        isPlayingSpecialAnimation = true;

                        spumPrefab.PlayAnimation(playState, randIndex);

                        yield return new WaitForSeconds(slowDuration);
                        isPlayingSpecialAnimation = false;
                    }
                }
            }
        }
    }
    private void OnMouseDown()
    {
        if (spumPrefab != null && spumPrefab.StateAnimationPairs.TryGetValue(PlayerState.DAMAGED.ToString(), out var animList))
        {
            if (animList != null && animList.Count > 0)
            {
                int randIndex = Random.Range(0, animList.Count);
                isPlayingSpecialAnimation = true;
                spumPrefab.PlayAnimation(PlayerState.ATTACK, randIndex);

                // 코루틴으로 일정 시간 뒤에 다시 이동 상태로 전환
                StartCoroutine(EndSpecialAnimationAfterDelay(1.5f));
            }
        }
    }

    private IEnumerator EndSpecialAnimationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isPlayingSpecialAnimation = false;
    }

}

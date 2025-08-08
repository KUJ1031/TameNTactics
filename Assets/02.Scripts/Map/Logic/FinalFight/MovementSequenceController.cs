using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovementStep
{
    public Vector2 direction; // 이동 방향
    public float duration;    // 해당 방향으로 이동할 시간
    public float speed = 1f;  // 이동 속도
}

public class MovementSequenceController : MonoBehaviour
{
    [Header("움직임 단계 리스트")]
    public List<MovementStep> movementSteps = new List<MovementStep>();

    [Header("느리게 실행할지 여부")]
    public bool isSlow = false;

    private int currentStepIndex = 0;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (movementSteps.Count > 0)
        {
            //StartCoroutine(PlayMovementSequence());
        }
    }

    public void StartSequence()
    {
        if (movementSteps.Count > 0)
        {
            currentStepIndex = 0;
            StartCoroutine(PlayMovementSequence());
        }
    }

    private IEnumerator PlayMovementSequence()
    {
        animator.speed = isSlow ? 0.2f : 1.0f;
        animator.SetBool("1_Move", true);

        while (currentStepIndex < movementSteps.Count)
        {
            MovementStep step = movementSteps[currentStepIndex];
            float elapsed = 0f;

            // 방향에 따라 X Flip 설정
            if (step.direction.x != 0 && spriteRenderer != null)
            {
                spriteRenderer.flipX = step.direction.x > 0;
            }

            while (elapsed < step.duration)
            {
                transform.Translate(step.direction.normalized * step.speed * Time.deltaTime);
                elapsed += Time.deltaTime;
                yield return null;
            }

            currentStepIndex++;
        }

        animator.SetBool("1_Move", false);
        animator.speed = 1.0f;
        PlayerManager.Instance.playerController.isInputBlocked = false; // 플레이어 입력 차단
    }
}

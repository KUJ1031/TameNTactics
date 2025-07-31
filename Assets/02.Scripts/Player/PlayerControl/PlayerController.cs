using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset lnputActions;

    public float movespeed = 5f;
    private IPlayerState currentState;
    public Rigidbody2D rb;
    public PlayerinputAction inputActions;

    public InputAction moveAction;

    private InputAction inputAction;

    internal bool isInputBlocked = false;
    public bool isSliding { get; set; } = false;
    public Vector2 slideDirection { get; set; } = Vector2.zero;
    public Vector2 lastMoveInput = Vector2.zero;

    private Animator animator;
    private Vector3 originalScale;

    private bool isAutoMoving = false;
    private Vector2 autoMoveDirection = Vector2.zero;
    private float autoMoveSpeed = 0f;

    private void Awake()
    {
        inputActions = new PlayerinputAction();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        originalScale = transform.localScale;
    }


    private void OnEnable()
    {
        inputActions.Player.Enable();
        ChangeState(new Idlestate());

        var map = lnputActions.FindActionMap("Player", true);
        moveAction = map.FindAction("Move", true);

        moveAction.Enable();
        Debug.Log(moveAction);
        ChangeState(new Idlestate());
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    //상태변환
    public void ChangeState(IPlayerState newState)
    {
        currentState?.OnExit(this);
        currentState = newState;
        currentState?.OnEnter(this);
    }


    void Update()
    {
        UpdateLastMoveInput();

        if (isInputBlocked && !isAutoMoving)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("1_Move", false);
            return;
        }

        if (isAutoMoving)
        {
            // 자동 이동 중 velocity 유지
            rb.velocity = autoMoveDirection.normalized * autoMoveSpeed;
            animator.SetBool("1_Move", true);
            if (Mathf.Abs(autoMoveDirection.x) > 0.01f)
                Flip(autoMoveDirection.x);
            return;  // 자동 이동 중엔 입력 무시하고 여기서 끝냄
        }

        currentState?.OnHandlelnput(this);
        currentState?.OnUpdate(this);

        Vector2 input = moveAction.ReadValue<Vector2>();
        bool isMoving = input.sqrMagnitude > 0.01f;
        animator.SetBool("1_Move", isMoving);

        if (Mathf.Abs(input.x) > 0.01f)
            Flip(input.x);

        // 실제 물리 이동 처리
        rb.velocity = input.normalized * movespeed;
    }

    public void UpdateLastMoveInput()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        if (input != Vector2.zero)
            lastMoveInput = input.normalized;
        else
            lastMoveInput = Vector2.zero;  // 입력 없으면 0으로 초기화
    }

    public Vector2 GetMoveInput()
    {
        // 1. 직접 키 입력이 있는 경우
        Vector2 input = moveAction.ReadValue<Vector2>();
        if (input != Vector2.zero)
        {
            lastMoveInput = input.normalized;
            return lastMoveInput;
        }

        // 2. 키 입력이 없지만 물리적으로 움직이는 경우
        if (rb.velocity.sqrMagnitude > 0.01f)
        {
            return rb.velocity.normalized;
        }

        // 3. 완전히 멈춰있는 경우
        return Vector2.zero;
    }

    void Flip(float moveX)
    {
        Vector3 scale = originalScale;
        scale.x = Mathf.Abs(originalScale.x) * (moveX > 0 ? -1 : 1);
        transform.localScale = scale;
    }

    public void BlockInput(bool shouldBlock)
    {
        isInputBlocked = shouldBlock;

        if (shouldBlock)
        {
            rb.velocity = Vector2.zero; // 이동 멈추기
            animator.SetBool("1_Move", false);
            lastMoveInput = Vector2.zero;
        }
    }

    public void AutoMove(Vector2 direction, float duration, float customSpeed, bool canMove)
    {
        StartCoroutine(AutoMoveCoroutine(direction, duration, customSpeed, canMove));
    }

    private IEnumerator AutoMoveCoroutine(Vector2 direction, float duration, float customSpeed, bool canMove)
    {
        isAutoMoving = true;
        autoMoveDirection = direction;
        autoMoveSpeed = customSpeed;

        BlockInput(true);  // 입력 차단

        yield return new WaitForSeconds(duration);

        isAutoMoving = false;
        autoMoveDirection = Vector2.zero;
        autoMoveSpeed = 0f;

        BlockInput(!canMove); // 입력 허용
    }

}

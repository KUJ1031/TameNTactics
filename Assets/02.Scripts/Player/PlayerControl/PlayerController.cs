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
    public Vector2 lastMoveInput { get; private set; } = Vector2.zero;


    private void Awake()
    {
        inputActions = new PlayerinputAction();
    }


    private void OnEnable()
    {
        inputActions.Player.Enable();
        ChanageState(new Idlestate());

        var map = lnputActions.FindActionMap("Player", true);
        moveAction = map.FindAction("Move", true);

        moveAction.Enable();
        Debug.Log(moveAction);
        ChanageState(new Idlestate());
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    //상태변환
    public void ChanageState(IPlayerState newState)
    {
        currentState?.OnExit(this);
        currentState = newState;
        currentState?.OnEnter(this);
    }


    // Update is called once per frame
    void Update()
    {
        // 항상 입력 갱신
        UpdateLastMoveInput();

        if (isInputBlocked) return;

        currentState?.OnHandlelnput(this);
        currentState?.OnUpdate(this);
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
}

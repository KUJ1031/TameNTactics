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
        currentState?.OnHandlelnput(this);
        currentState?.OnUpdate(this);
    }
}

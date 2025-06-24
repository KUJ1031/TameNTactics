using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movespeed = 5f;
    private IPlayerState currentState;
    public Rigidbody2D rb;
    public PlayerinputAction inputActions;



    private void Awake()
    {
        inputActions = new PlayerinputAction();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
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
        currentState?.OnHandlelnput(this, inputActions);
        currentState?.OnUpdate(this);
    }
}

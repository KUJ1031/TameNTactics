using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Idlestate : IPlayerState
{
    public void OnEnter(PlayerController player)
    {
        player.rb.velocity = Vector2.zero;
    }

    public void OnExit(PlayerController player)
    {

    }


    public void OnHandlelnput(PlayerController player)
    {
        Vector2 moves = player.moveAction.ReadValue<Vector2>();
        if (moves != Vector2.zero)
        {
            player.ChangeState(new MoveState()); //이동 상태로 전환
        }
    }

    public void OnUpdate(PlayerController player)
    {
        //.......
    }
}

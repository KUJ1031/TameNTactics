using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IPlayerState
{
    public void OnEnter(PlayerController player)
    {
       // Debug.Log("MoveState 진입");
    }

    public void OnExit(PlayerController player)
    {

    }

    public void OnHandlelnput(PlayerController player, PlayerinputAction input)
    {
        Vector2 inputdir = input.Player.Move.ReadValue<Vector2>();

        if (inputdir.sqrMagnitude<0.01f)
        {
            player.ChanageState(new Idlestate());//IDIe 상태로 전환
        }
    }

    public void OnUpdate(PlayerController player)
    {
        Vector2 move = player.inputActions.Player.Move.ReadValue<Vector2>();
        player.rb.velocity = move * player.movespeed; //이동 속도 적용

    }
}

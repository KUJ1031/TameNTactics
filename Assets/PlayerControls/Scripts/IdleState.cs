using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idlestate : IPlayerState
{
    public void OnEnter(PlayerController player)
    {
        player.rb.velocity = Vector2.zero;
    }

    public void OnExit(PlayerController player)
    {

    }

    public void OnHandlelnput(PlayerController player, PlayerinputAction inputs)
    {
        //가만히 있기 때문에 입력을 처리하지 않음
        Vector2 move = inputs.Player.Move.ReadValue<Vector2>();
        if(move!=Vector2.zero)
        {
            player.ChanageState(new MoveState()); //이동 상태로 전환
        }
    }

    public void OnUpdate(PlayerController player)
    {
        //.......
    }
}

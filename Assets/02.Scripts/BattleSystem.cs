using System;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public enum BattleState
    {
        Start,
        WaitingAction,
        ExecuteMove,
        ProcessDamage,
        CheckFaint,
        BattleEnd
    }
    
    public BattleState currentState;

    private void Start()
    {
        currentState = BattleState.Start;
    }

    void SetupBattle()
    {
        //배틀 화면 UI 전체 세팅
    }

    void SelectedMove()
    {
        
    }
    
}

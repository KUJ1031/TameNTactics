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

    private void Update()
    {
        throw new NotImplementedException();
    }

    void SetupBattle()
    {
        //배틀 화면 UI 전체 세팅
    }

    void SelectedMove()
    {
        
    }
    
}

using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    private BaseBattleState currentState;

    private void Start()
    {
        ChangeState(new PlayerMenuState(this));
    }

    private void Update()
    {
        currentState?.Execute();
    }

    public void ChangeState(BaseBattleState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
}

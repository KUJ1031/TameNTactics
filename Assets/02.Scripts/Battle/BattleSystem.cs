using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    private BaseBattleState currentState;
    public BattleManager battleManager;

    private void Start()
    {
        battleManager = BattleManager.Instance;
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

using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    private BaseBattleState currentState;
    public BattleManager BattleManager;

    private void Start()
    {
        BattleManager = BattleManager.Instance;
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

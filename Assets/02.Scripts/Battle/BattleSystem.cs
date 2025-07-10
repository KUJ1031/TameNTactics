using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem Instance { get; private set; }

    private BaseBattleState currentState;
    public BaseBattleState CurrentState => currentState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

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

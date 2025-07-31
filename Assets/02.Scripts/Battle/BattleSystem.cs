using UnityEngine;

public class BattleSystem : Singleton<BattleSystem>
{
    private BaseBattleState currentState;
    public BaseBattleState CurrentState => currentState;

    // 포섭하기 중 선택된 아이템을 저장할 변수
    public ItemInstance selectedGestureItem;

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

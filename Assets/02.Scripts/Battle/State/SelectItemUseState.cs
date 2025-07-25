using UnityEngine;
using System.Collections;

public class SelectItemUseState : BaseBattleState
{
    private ItemInstance selectedItem;

    public SelectItemUseState(BattleSystem system, ItemInstance item) : base(system)
    {
        selectedItem = item;
    }

    public override void Enter()
    {
        Debug.Log($"아이템 사용 대상 선택 상태로 진입했습니다. {selectedItem.data.itemName}을(를) 사용할 대상을 선택하세요.");

        // 사용 가능한 대상 목록 설정 (아군 몬스터만 예시)
        BattleManager.Instance.possibleTargets = BattleManager.Instance.BattleEntryTeam.FindAll(m => m.CurHp > 0);
    }

    public override void Execute()
    {
        // 마우스 또는 키보드 입력으로 대상 선택 대기 (실제로는 UI 이벤트로 처리)
    }

    public void OnTargetSelected(Monster target)
    {
        if (target == null || target.CurHp <= 0)
        {
            Debug.LogWarning("유효하지 않은 대상입니다.");
            return;
        }

        // 아이템 효과 적용 예시: 체력 회복
        foreach (var effect in selectedItem.data.itemEffects)
        {
            if (effect.type == ItemEffectType.curHp)
            {
                target.Heal_Potion(effect.value);
                Debug.Log($"{target.monsterName}에게 {selectedItem.data.itemName} 효과 적용: 체력 {effect.value} 회복");
            }
            else
            {
                Debug.LogWarning($"지원하지 않는 아이템 효과: {effect.type}");
            }
        }

        // 아이템 수량 감소
        selectedItem.quantity--;
        if (selectedItem.quantity <= 0)
        {
            // 인벤토리에서 제거 로직 필요 시 호출
            PlayerManager.Instance.player.RemoveItem(selectedItem, 1);
        }

        UIManager.Instance.battleUIManager.InventoryView.HideInventory();
        UIManager.Instance.battleUIManager.HidePossibleTargets();
        battleSystem.StartCoroutine(EnemyAttackAfterDelay(0.5f));
        // 다음 상태로 전환 (적 공격)
    }

    public void OnCancel()
    {
        UIManager.Instance.battleUIManager.InventoryView.HideInventory();
        UIManager.Instance.battleUIManager.HidePossibleTargets();
        battleSystem.ChangeState(new PlayerMenuState(battleSystem));
    }

    public override void Exit()
    {
        UIManager.Instance.battleUIManager.HidePossibleTargets();
        Debug.Log("SelectItemUseState 종료");
    }

    private IEnumerator EnemyAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        BattleManager.Instance.EnemyAttackAfterPlayerTurn();
    }
}

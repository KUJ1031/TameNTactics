using UnityEngine;
public class InventoryState : BaseBattleState
{
    public InventoryState(BattleSystem system) : base(system) { }

    public override void Enter()
    {
        BattleTutorialManager.Instance.InitItemSelected();
        var inventoryView = UIManager.Instance.battleUIManager.InventoryView;
        inventoryView.ShowInventory();
        inventoryView.OnItemUseConfirmed += OnSelectedItem;
        inventoryView.OnInventoryCancelled += OnCancelInventory;

        UIManager.Instance.battleUIManager.BattleSelectView.HideSkillPanel();
        UIManager.Instance.battleUIManager.BattleSelectView.HideSelectPanel();
        UIManager.Instance.battleUIManager.SkillView.HideActiveSkillTooltip();

        Debug.Log("인벤토리 상태로 진입했습니다. 아이템을 선택하세요.");
    }

    public override void Execute() { }

    public void OnSelectedItem(ItemInstance item)
    {
        // 아이템 선택 후 대상 선택 상태로 변경
        battleSystem.ChangeState(new SelectItemUseState(battleSystem, item));
    }


    public void OnCancelInventory()
    {
        UIManager.Instance.battleUIManager.InventoryView.HideInventory();
        battleSystem.ChangeState(new PlayerMenuState(battleSystem));
    }

    public override void Exit()
    {
        UIManager.Instance.battleUIManager.InventoryView.OnItemUseConfirmed -= OnSelectedItem;
        UIManager.Instance.battleUIManager.InventoryView.OnInventoryCancelled -= OnCancelInventory;
        UIManager.Instance.battleUIManager.InventoryView.HideInventory();
    }
}


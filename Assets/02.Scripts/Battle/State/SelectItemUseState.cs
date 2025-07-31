using UnityEngine;
using System.Collections;
using System.Linq;

public class SelectItemUseState : BaseBattleState
{
    private ItemInstance selectedItem;

    public SelectItemUseState(BattleSystem system, ItemInstance item) : base(system)
    {
        selectedItem = item;
    }

    public override void Enter()
    {
        Debug.Log($"아이템 사용 상태 진입: {selectedItem.data.itemName}");
        BattleTutorialManager.Instance.InitMonsterItemSelected();
        UIManager.Instance.battleUIManager.BattleSelectView.ShowBehaviorPanel("아이템을 사용할 몬스터를 선택하세요.");
        // 선택 대상이 필요 없는 경우 (예: 아군 전체 회복)
        if (selectedItem.data.itemEffects.Any(e => e.type == ItemEffectType.allMonsterCurHp))
        {
            UseItemWithoutTarget();
        }
        else
        {
            // 대상 선택 UI 활성화
            Debug.Log("아이템 대상 선택 UI 활성화");

            // 조건: 생존 중 + 체력이 가득 차지 않은 몬스터만
            BattleManager.Instance.possibleTargets = BattleManager.Instance.BattleEntryTeam
                .Where(m => m.CurHp > 0 && m.CurHp < m.MaxHp)
                .ToList();

            UIManager.Instance.battleUIManager.EnableHoverSelect(BattleManager.Instance.possibleTargets);
        }
    }

    public override void Execute()
    {
        // 대상 선택은 UI 클릭으로 처리됨
    }

    public override void Exit()
    {
        UIManager.Instance.battleUIManager.BattleSelectView.HideBeHaviorPanel();
        Debug.Log("SelectItemUseState 종료");
    }
    public void OnCancel()
    {
        UIManager.Instance.battleUIManager.HidePossibleTargets();
        battleSystem.ChangeState(new PlayerMenuState(battleSystem));
    }

    private void UseItemWithoutTarget()
    {
        // 몬스터 하나씩 순차적 회복 후 적 공격 시작
        battleSystem.StartCoroutine(HealAllAndProceed());
    }

    public void OnTargetSelected(Monster target)
    {
        UIManager.Instance.battleUIManager.DisableHoverSelect(BattleManager.Instance.possibleTargets);
        UIManager.Instance.battleUIManager.BattleSelectView.HideBeHaviorPanel();
        BattleTutorialManager.Instance.EndInventoryTutorial();
        if (target == null || target.CurHp <= 0)
        {
            Debug.LogWarning("죽었거나 유효한 몬스터가 아닙니다.");
            return;
        }

        UIManager.Instance.battleUIManager.DeselectMonster(target);

        // 회복 코루틴 끝난 후 적 공격 시작하도록 처리
        battleSystem.StartCoroutine(ApplyEffectAndProceed(target));
    }
    private IEnumerator ApplyEffectAndProceed(Monster target)
    {
        foreach (var effect in selectedItem.data.itemEffects)
        {
            if (effect.type == ItemEffectType.curHp)
            {
                yield return battleSystem.StartCoroutine(HealOverTime(target, effect.value, 0.5f));
                Debug.Log($"{target.monsterName} → {selectedItem.data.itemName} 사용: 체력 {effect.value} 서서히 회복");
            }
            else
            {
                Debug.LogWarning($"지원하지 않는 대상 지정 아이템 효과: {effect.type}");
            }
        }

        AfterItemUse();

        // 적 공격 호출는 여기서 한 번만 수행
        yield return new WaitForSeconds(0.5f);
        BattleManager.Instance.EnemyAttackAfterPlayerTurn();
    }

    private IEnumerator HealAllAndProceed()
    {
        foreach (var effect in selectedItem.data.itemEffects)
        {
            if (effect.type == ItemEffectType.allMonsterCurHp)
            {
                foreach (var monster in BattleManager.Instance.BattleEntryTeam)
                {
                    if (monster.CurHp > 0 && monster.CurHp < monster.MaxHp)
                    {
                        yield return battleSystem.StartCoroutine(HealOverTime(monster, effect.value, 0.5f));
                        Debug.Log($"{monster.monsterName} → {selectedItem.data.itemName} 사용: 체력 {effect.value} 서서히 회복");
                    }
                }
            }
            else
            {
                Debug.LogWarning($"지원하지 않는 자동 적용 아이템 효과: {effect.type}");
            }
        }

        AfterItemUse();

        yield return new WaitForSeconds(0.5f);
        BattleManager.Instance.EnemyAttackAfterPlayerTurn();
    }

    // HealOverTime 에서 적 공격 호출 제거
    public IEnumerator HealOverTime(Monster target, int totalHealAmount, float duration)
    {
        int healedAmount = 0;
        int healStep = 1;
        float interval = duration / totalHealAmount;

        while (healedAmount < totalHealAmount)
        {
            if (target.CurHp >= target.MaxHp) yield break;

            target.Heal_Potion(healStep);
            healedAmount += healStep;

            yield return new WaitForSeconds(interval);
        }
    }

    private void AfterItemUse()
    {
        // 인벤토리에서 아이템 수량 차감
        if (selectedItem.quantity > 0)
        {
            PlayerManager.Instance.player.RemoveItem(selectedItem, 1);
        }
        else
        {
            Debug.LogWarning("아이템 수량이 부족합니다.");
        }

        // UI 정리 및 다음 턴으로
        UIManager.Instance.battleUIManager.InventoryView.HideInventory();
        UIManager.Instance.battleUIManager.HidePossibleTargets();
    }

}

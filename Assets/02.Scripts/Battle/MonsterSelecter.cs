using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSelecter : MonoBehaviour
{
    private Monster monster;

    public void Initialize(Monster monster)
    {
        this.monster = monster;
    }

    private void OnMouseDown()
    {
        if (BattleSystem.Instance.CurrentState is SelectPlayerMonsterState state)
        {
            state.OnMonsterSelected(monster);
        }

        if (BattleSystem.Instance.CurrentState is SelectTargetState enemyState)
        {
            UIManager.Instance.battleUIManager.BattleSelectView.HideBeHaviorPanel();
            enemyState.OnSelectTargetMonster(monster);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSelecter : MonoBehaviour
{
    private Monster monster;
    public static bool isClicked = false;

    public void Initialize(Monster monster)
    {
        this.monster = monster;
    }

    private void OnMouseDown()
    {
        if (isClicked) return;
        
        if (BattleSystem.Instance.CurrentState is SelectPlayerMonsterState state && !isClicked)
        {
            state.OnMonsterSelected(monster);
        }

        if (BattleSystem.Instance.CurrentState is SelectTargetState enemyState && !isClicked)
        {
            isClicked = true;
            enemyState.OnSelectTargetMonster(monster);
        }
    }
}

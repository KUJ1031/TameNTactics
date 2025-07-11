using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSelecter : MonoBehaviour
{
    private Monster monster;
    public static bool isClicked = false;
    public static float lockTime = 3f;
    public static float playerSelectTime = 0.5f;

    public void Initialize(Monster monster)
    {
        this.monster = monster;
    }

    private void OnMouseDown()
    {
        if (isClicked) return;
        
        isClicked = true;
        
        if (BattleSystem.Instance.CurrentState is SelectPlayerMonsterState state)
        {
            state.OnMonsterSelected(monster);
        }

        if (BattleSystem.Instance.CurrentState is SelectTargetState enemyState)
        {
            enemyState.OnSelectTargetMonster(monster);
        }
        
        StartCoroutine(UnLockMouseClick());
    }

    private IEnumerator UnLockMouseClick()
    {
        yield return new WaitForSeconds(lockTime);
        isClicked = false;
    }
}

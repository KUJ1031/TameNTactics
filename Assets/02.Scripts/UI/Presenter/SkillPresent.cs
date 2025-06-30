using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPresent : MonoBehaviour
{
    [SerializeField] private SkillView skillView;

    private bool isAttackMode = false;

    private void Update()
    {
        if (!isAttackMode) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Monster monster = hit.collider.GetComponent<Monster>();
                Debug.Log($"몬스터 클릭 {monster.GetData()}");
                if (monster != null)
                {
                    ShowMonsterSkills(monster.GetData());
                }
            }
        }
    }

    public void EnableAttackMode() => isAttackMode = true;

    public void DisableAttackMode()
    {
        isAttackMode = false;
        skillView.HideSkills();
    }

    private void ShowMonsterSkills(MonsterData monsterData)
    {
        if (monsterData == null || monsterData.skills == null) return;

        skillView.ShowSkillList(monsterData.skills);
    }
}

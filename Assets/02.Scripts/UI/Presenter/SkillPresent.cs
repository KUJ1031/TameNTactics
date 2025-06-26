using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPresent : MonoBehaviour
{
    [SerializeField] private SkillView skillView;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Monster monster = hit.collider.GetComponent<Monster>();
                if (monster != null)
                {
                    ShowMonsterSkills(monster.GetData());
                }
            }

            // 몬스터 외에 다른 부분을 클릭하면 패널 숨기기
            skillView.HideSkills();
        }
    }

    private void ShowMonsterSkills(MonsterData monsterData)
    {
        if (monsterData == null || monsterData.skills == null) return;

        skillView.ShowSkillList(monsterData.skills);
    }
}

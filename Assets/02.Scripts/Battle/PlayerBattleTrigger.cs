using UnityEngine;
using System.Collections.Generic;

public class PlayerBattleTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //충돌한 객체 저장
        MonsterFactory factory;
        Monster monster;

        factory = other.GetComponentInParent<MonsterFactory>();
        monster = other.GetComponent<Monster>();

        if (factory == null) return;
        if (monster == null) return;

        // 적 팀 구성
        List<Monster> enemyTeam = factory.GetRandomEnemyTeam(monster);

        //적 팀 배틀메니저로
        BattleManager.Instance.enemyTeam = enemyTeam;

        Destroy(other.gameObject); // 충돌한 적 제거
        
        //씬이동
        UnityEngine.SceneManagement.SceneManager.LoadScene("BattleUITest");
    }
}

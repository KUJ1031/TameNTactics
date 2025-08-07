using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleTestSetup : MonoBehaviour
{
    [Header("테스트용 몬스터 데이터")]
    public List<MonsterData> allyMonsters;
    public List<MonsterData> enemyMonsters;

    [Header("생성자 참조")]
    public SpawnBattleAllMonsters spawner;

    [Header("공격 위치 참조")]
    public Transform attackPosition;

    private void Start()
    {
        PlayerManager.Instance.player.playerEliteStartCheck.Clear();
        PlayerManager.Instance.player.playerEliteStartCheck.Add(0, false);
        PlayerManager.Instance.player.playerEliteStartCheck.Add(1, false);
        PlayerManager.Instance.player.playerEliteStartCheck.Add(2, false);

        if (PlayerManager.Instance == null || BattleManager.Instance == null)
        {
            Debug.LogError("PlayerManager 또는 BattleManager가 아직 초기화되지 않았습니다.");
            return;
        }

        if (attackPosition != null)
        {
            BattleManager.Instance.AttackPosition = attackPosition;
            Debug.Log($"AttackPosition 등록 완료: {attackPosition.name}");
        }
        else
        {
            Debug.LogError("AttackPosition이 설정되지 않았습니다. 인스펙터에서 할당해주세요.");
        }

        var player = PlayerManager.Instance.player;
        player.battleEntry.Clear();
        BattleManager.Instance.enemyTeam.Clear();

        foreach (var data in allyMonsters)
        {
            Monster m = new Monster();
            m.SetMonsterData(data);
            m.SetLevel(25);
            m.RecalculateStats();
            player.battleEntry.Add(m);
        }

        foreach (var data in enemyMonsters)
        {
            Monster m = new Monster();
            m.SetMonsterData(data);
            m.SetLevel(25);
            m.RecalculateStats();
            BattleManager.Instance.enemyTeam.Add(m);
        }

        Debug.Log("테스트용 몬스터 세팅 완료");

        BattleSystem.Instance.ChangeState(new PlayerMenuState(BattleSystem.Instance));

        // 선택 가능한 몬스터 중 첫 번째를 자동 선택
        var firstMonster = PlayerManager.Instance.player.battleEntry.FirstOrDefault();
        if (firstMonster != null)
        {
            BattleManager.Instance.SelectPlayerMonster(firstMonster);
            Debug.Log($"초기 선택 몬스터: {firstMonster.monsterName}");
        }
    }
}
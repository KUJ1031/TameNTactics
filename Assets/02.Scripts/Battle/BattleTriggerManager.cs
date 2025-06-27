using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전투 관련 정보를 저장하고 공유하는 매니저 (전투 씬 전환 전후에 사용)
/// - 충돌한 몬스터
/// - 적 팀
/// - 플레이어 출전 팀
/// </summary>
public class BattleTriggerManager : Singleton<BattleTriggerManager>
{
    // 충돌한 적 몬스터
    private MonsterData lastMonster;

    // 전투에 등장할 적 팀 (MonsterFactory에서 랜덤 구성)
    private List<MonsterData> enemyTeam;

    // 플레이어의 출전 팀 (PlayerManager에서 battleEntry를 참조)
    private List<MonsterData> playerTeam;

    // 벤치 몬스터 (PlayerManager에서 benchEntry를 참조)
    private List<MonsterData> benchMonsters;

    /// <summary>
    /// 충돌한 몬스터 저장
    /// </summary>
    public void SetLastMonster(MonsterData data)
    {
        lastMonster = data;
    }

    public MonsterData GetLastMonster() => lastMonster;

    /// <summary>
    /// 전투 적팀 저장
    /// </summary>
    public void SetEnemyTeam(List<MonsterData> team)
    {
        enemyTeam = team;
    }

    public List<MonsterData> GetEnemyTeam() => enemyTeam;

    /// <summary>
    /// 플레이어팀 저장
    /// </summary>
    public void SetPlayerTeam(List<MonsterData> team)
    {
        playerTeam = team;
    }

    public List<MonsterData> GetPlayerTeam() => playerTeam;

    public void SetBenchMonsters(List<MonsterData> bench) => benchMonsters = bench;

    public List<MonsterData> GetBenchMonsters() => benchMonsters;

    /// <summary>
    /// 플레이어 팀과 벤치 자동 동기화 (PlayerManager에서 가져옴)
    /// </summary>
    public void SyncPlayerTeamFromPlayer()
    {
        var player = PlayerManager.Instance?.player;
        if (player != null)
        {
            playerTeam = new List<MonsterData>(player.battleEntry);
            benchMonsters = new List<MonsterData>(player.benchEntry);
        }
        else
        {
            Debug.LogWarning("플레이어가 존재하지 않아 팀을 불러올 수 없습니다.");
            playerTeam = new List<MonsterData>();
            benchMonsters = new List<MonsterData>();
        }
    }

    /// <summary>
    /// 전투 데이터 초기화 (전투 종료 후 등)
    /// </summary>
    public void ResetBattleData()
    {
        lastMonster = null;
        enemyTeam = null;
        playerTeam = null;
        benchMonsters = null;
    }
}

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

    // 플레이어의 출전 팀 (EntryManager에서 선택된 몬스터 목록)
    private List<MonsterData> playerTeam;

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

    /// <summary>
    /// EntryManager에서 플레이어 팀 자동 불러오기
    /// </summary>
    public void SyncPlayerTeamFromEntry()
    {
        playerTeam = new List<MonsterData>(EntryManager.Instance.selectedEntries);
    }

    /// <summary>
    /// 전투 데이터 초기화 (전투 종료 후 등)
    /// </summary>
    public void ResetBattleData()
    {
        lastMonster = null;
        enemyTeam = null;
        playerTeam = null;
    }
}

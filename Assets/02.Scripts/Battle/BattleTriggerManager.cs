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
    // 충돌한 적 몬스터 (Monster 인스턴스)
    private Monster lastMonster;

    // 전투에 등장할 적 팀
    private List<Monster> enemyTeam;

    // 플레이어의 출전 팀
    private List<Monster> playerTeam;

    // 플레이어의 벤치 몬스터
    private List<Monster> benchMonsters;

    /// <summary>
    /// 충돌한 몬스터 저장
    /// </summary>
    public void SetLastMonster(Monster monster)
    {
        lastMonster = monster;
    }

    public Monster GetLastMonster() => lastMonster;

    /// <summary>
    /// 전투 적팀 저장
    /// </summary>
    public void SetEnemyTeam(List<Monster> team)
    {
        enemyTeam = team;
    }

    public List<Monster> GetEnemyTeam()
    {
        Debug.Log($"적 팀 정보: {enemyTeam?.Count}마리");
        //몬스터 레벨 디버그
        foreach (var m in enemyTeam)
        {
            if (m == null)
            {
                Debug.LogWarning("적 몬스터 리스트에 null이 포함되어 있음!");
                continue;
            }

            Debug.Log($"적 몬스터: {m.monsterData.monsterName}, 레벨: {m.Level}");
        }
        return enemyTeam;
    }
        
        

    /// <summary>
    /// 플레이어팀 저장
    /// </summary>
    public void SetPlayerTeam(List<Monster> team)
    {
        playerTeam = team;
    }

    public List<Monster> GetPlayerTeam() => playerTeam;

    public void SetBenchMonsters(List<Monster> bench) => benchMonsters = bench;

    public List<Monster> GetBenchMonsters() => benchMonsters;

    /// <summary>
    /// 플레이어 팀과 벤치 자동 동기화 (PlayerManager에서 가져옴)
    /// </summary>
    public void SyncPlayerTeamFromPlayer()
    {
        var player = PlayerManager.Instance?.player;
        if (player != null)
        {
            playerTeam = new List<Monster>(player.battleEntry);
            benchMonsters = new List<Monster>(player.benchEntry);
        }
        else
        {
            Debug.LogWarning("플레이어가 존재하지 않아 팀을 불러올 수 없습니다.");
            playerTeam = new List<Monster>();
            benchMonsters = new List<Monster>();
        }
    }
    //public void SetEnemyTeamData(List<Monster> list) => enemyTeamData = list;
    //public List<MonsterData> GetEnemyTeamData() => enemyTeamData;
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

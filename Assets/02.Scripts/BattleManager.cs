using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : Singleton<BattleManager>
{
    public void StartBattle(MonsterData enemy)
    {
        var playerTeam = EntryManager.Instance.selectedEntries;
        // 1. 출전 몬스터 불러오기
        if (playerTeam.Count == 0)
        {
            Debug.LogWarning("출전 몬스터가 없습니다! 전투를 시작할 수 없습니다.");
            return;
        }
        SceneManager.LoadScene("SampleBattleScene"); // 전투 씬으로 전환
        Debug.Log($"출전 몬스터 수: {playerTeam.Count}");
        foreach (var monster in playerTeam)
        {
            Debug.Log($"출전 몬스터: {monster.monsterName}");
        }
        Debug.Log($"적 몬스터: {enemy.monsterName}");
        // 출전 몬스터가 없다면 경고 메시지 출력
        

        // 2. 씬 전환 or 전투 UI 띄우기
        Time.timeScale = 0f; // 게임 일시 정지

        // 3. 데이터 전달 등등...
    }
}

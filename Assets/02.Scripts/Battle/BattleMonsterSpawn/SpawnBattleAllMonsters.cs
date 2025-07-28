using System.Collections.Generic;
using UnityEngine;

public class SpawnBattleAllMonsters : MonoBehaviour
{
    private List<Monster> playerTeam;   //플레이어 몬스터 리스트
    private List<Monster> enemyTeam;    //적 몬스터 리스트

    //public GameObject monsterPrefab;    //몬스터 기본 프리팹

    public Transform allySpawner;       //아군 생성 위치
    public Transform enemySpawner;      //적군 생성 위치

    private void Start()
    {
        playerTeam = PlayerManager.Instance.player.battleEntry;
        enemyTeam = BattleManager.Instance.enemyTeam;

        CreateMonster(playerTeam, allySpawner);
        CreateMonster(enemyTeam, enemySpawner);

        foreach (Monster monster in enemyTeam)
        {
            if (enemyTeam.Count == 0) break;

            monster.EncounterPlus();
        }

        BattleManager.Instance.FindSpawnMonsters();
        BattleManager.Instance.StartBattle();
        UIManager.Instance.battleUIManager.SettingMonsterInfo(allySpawner, enemySpawner);
        UIManager.Instance.battleUIManager.SettingMonsterPassive(playerTeam);
        UIManager.Instance.battleUIManager.SettingMonsterSelecter(allySpawner, enemySpawner);
    }

    //위치에 몬스터 생성
    private void CreateMonster(List<Monster> monsterList, Transform Spawner)
    {
        for (int i = 0; i < monsterList.Count; i++)
        {
            string spawnPointName = "SpawnPoint_" + (i + 1);
            Transform spawnPointTransform = Spawner.Find(spawnPointName);

            string prefabPath = $"Units/{monsterList[i].monsterData.monsterName}";
            GameObject loadedPrefab = Resources.Load<GameObject>(prefabPath);

            if (monsterList[i].CurHp > 0)
            {
                //스폰 위치에 객체 생성
                GameObject enemyMonster;

                if (playerTeam.Contains(monsterList[i]))
                {
                    enemyMonster = Instantiate(loadedPrefab, spawnPointTransform);

                    Vector3 newScale = enemyMonster.transform.localScale;
                    newScale.x *= -1;
                    enemyMonster.transform.localScale = newScale;
                }
                else
                {
                    enemyMonster = Instantiate(loadedPrefab, spawnPointTransform);
                }

                //객체 값 수정
                var monsterChar = enemyMonster.GetComponent<MonsterCharacter>();
                monsterChar.Init(monsterList[i]);

                var clickable = enemyMonster.GetComponent<MonsterSelecter>();
                clickable?.Initialize(monsterChar.monster); // Monster 데이터 넘기기

                monsterChar.monster.HpChange += UIManager.Instance.battleUIManager.UpdateHpGauge;
                monsterChar.monster.ultimateCostChange += UIManager.Instance.battleUIManager.UpdateUltimateGauge;
            }
        }
    }
}

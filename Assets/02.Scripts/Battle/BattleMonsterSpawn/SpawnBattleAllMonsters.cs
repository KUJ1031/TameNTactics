using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpawnBattleAllMonsters : MonoBehaviour
{
    private List<Monster> playerTeam;   //플레이어 몬스터 리스트
    private List<Monster> enemyTeam;    //적 몬스터 리스트

    public GameObject monsterPrefab;    //몬스터 기본 프리팹

    public Transform allySpawner;       //아군 생성 위치
    public Transform enemySpawner;      //적군 생성 위치

    private void Start()
    {
        playerTeam = PlayerManager.Instance.player.battleEntry;
        enemyTeam = BattleManager.Instance.enemyTeam;

        CreateMonster(playerTeam, allySpawner);
        CreateMonster(enemyTeam, enemySpawner);

        UIManager.Instance.battleUIManager.SettingMonsterGauge(allySpawner, enemySpawner);
    }

    //위치에 몬스터 생성
    private void CreateMonster(List<Monster> monsterList, Transform Spawner)
    {
        for (int i = 0; i < monsterList.Count; i++)
        {
            string spawnPointName = "SpawnPoint_" + (i + 1);
            Transform spawnPointTransform = Spawner.Find(spawnPointName);

            //스폰 위치에 객체 생성
            GameObject enemyMonster = Instantiate(monsterPrefab, spawnPointTransform);
            //객체 값 수정
            enemyMonster.GetComponent<MonsterCharacter>().Init(monsterList[i]);

            var monsterChar = enemyMonster.GetComponent<MonsterCharacter>();

            var clickable = enemyMonster.GetComponent<MonsterSelecter>();
            clickable?.Initialize(monsterChar.monster); // Monster 데이터 넘기기

            //스킬 보여주기
            Debug.Log($"[SpawnBattleAllMonsters] {monsterChar.monster.monsterName} 스킬 개수: {monsterChar.monster.skills.Count}");
            foreach (var skill in monsterChar.monster.skills)
            {
                Debug.Log($"[SpawnBattleAllMonsters] {monsterChar.monster.monsterName} 스킬: {skill.skillName}");
            }
        }
    }
}

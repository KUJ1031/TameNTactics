using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전투 트리거를 통해 마지막으로 충돌한 적 몬스터의 데이터를 저장/제공하는 매니저
/// 전투 씬 전환 또는 BattleManager에서 해당 데이터를 사용할 수 있도록 싱글톤으로 관리
/// </summary>
public class BattleTriggerManager : Singleton<BattleTriggerManager>
{
    // 마지막으로 저장된 적 몬스터 정보
    private MonsterData lastMonster;

    /// <summary>
    /// 외부에서 전투를 유발한 몬스터 데이터를 저장
    /// BattleTrigger 등에서 호출됨
    /// </summary>
    /// <param name="data">적 몬스터의 MonsterData</param>
    public void SetLastMonster(MonsterData data)
    {
        lastMonster = data;
    }

    /// <summary>
    /// 저장된 마지막 몬스터 정보를 반환
    /// BattleManager 등에서 호출하여 전투에 사용
    /// </summary>
    /// <returns>저장된 MonsterData 객체</returns>
    public MonsterData GetLastMonster()
    {
        return lastMonster;
    }
}

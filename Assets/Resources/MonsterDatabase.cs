using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DB/MonsterDatabase")]
public class MonsterDatabase : ScriptableObject
{
    public List<MonsterData> allMonsters = new();

    // 편의성: 싱글턴처럼 Resources에서 로드
    private static MonsterDatabase _instance;
    public static MonsterDatabase Instance
    {
        get
        {
            if (_instance == null) _instance = Resources.Load<MonsterDatabase>("MonsterDatabase");
            return _instance;
        }
    }

    public MonsterData GetByNumber(int number)
    {
        return allMonsters.Find(m => m.monsterNumber == number);
    }
}

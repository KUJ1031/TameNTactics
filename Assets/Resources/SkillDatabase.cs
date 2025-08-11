using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DB/SkillDatabase")]
public class SkillDatabase : ScriptableObject
{
    public List<SkillData> allSkills = new();

    // 편의성: 싱글턴처럼 Resources에서 로드
    private static SkillDatabase _instance;
    public static SkillDatabase Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<SkillDatabase>("SkillDatabase");
            return _instance;
        }
    }

    public SkillData GetByNumber(int number)
    {
        return allSkills.Find(s => s.skillID == number);
    }
}

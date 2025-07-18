using UnityEngine;

[CreateAssetMenu(fileName = "MonsterTypeIcon_", menuName = "Monster/MonsterTypeIcon")]
public class MonsterTypeIconSO : ScriptableObject
{
    public MonsterType type;
    public Sprite icon;
}

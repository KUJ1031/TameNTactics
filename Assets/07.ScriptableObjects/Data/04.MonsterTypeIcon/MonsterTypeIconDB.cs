using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterTypeIconDB", menuName = "Monster/MonsterTypeIconDB")]
public class MonsterTypeIconDB : ScriptableObject
{
    public List<MonsterTypeIconSO> iconList;

    private Dictionary<MonsterType, Sprite> typeToSprite;

    public void InitializeTypeIcon()
    {
        typeToSprite = new Dictionary<MonsterType, Sprite>();
        foreach (var entry in iconList)
        {
            if (!typeToSprite.ContainsKey(entry.type))
            {
                typeToSprite.Add(entry.type, entry.icon);
            }
        }
    }

    public Sprite GetTypeIcon(MonsterType type)
    {
        if (typeToSprite == null)
        {
            InitializeTypeIcon();
        }

        return typeToSprite.TryGetValue(type, out var sprite) ? sprite : null;
    }
}

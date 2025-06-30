[System.Serializable]
public class SerializableMonsterInfo
{
    public MonsterData monsterData;
    public int level;
    public int curHp;
    public int curExp;

    public SerializableMonsterInfo(Monster monster)
    {
        monsterData = monster.monsterData;
        level = monster.Level;
        curHp = monster.CurHp;
        curExp = monster.CurExp;
    }
}

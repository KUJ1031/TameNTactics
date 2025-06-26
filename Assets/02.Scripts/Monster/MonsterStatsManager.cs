public static class MonsterStatsManager
{
    public static void AddExp(MonsterData monster, int expAmount)
    {
        monster.curExp += expAmount;

        while (monster.curExp >= monster.maxExp && monster.level < 30)
        {
            monster.curExp -= monster.maxExp;
            monster.level++;
            RecalculateStats(monster);
            monster.curHp = monster.maxHp;
        }
    }

    public static void RecalculateStats(MonsterData monster)
    {
        monster.maxHp = monster.baseHp + 12 * (monster.level - 1);
        monster.attack = monster.baseAttack + 3 * (monster.level - 1);
        monster.defense = monster.baseDefense + 3 * (monster.level - 1);
        monster.speed = monster.baseSpeed + 3 * (monster.level - 1);
        monster.maxExp = monster.baseExp + 25 * (monster.level - 1);
        monster.expReward = monster.baseExpReward + 25 * (monster.level - 1);
        monster.goldReward = monster.baseGoldReward + 30 * (monster.level - 1);
    }
}

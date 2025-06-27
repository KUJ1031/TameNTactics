public static class MonsterStatsManager
{
    // 경험치 추가 & 레벨업 시 스텟 변화
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

    // 레벨에 맞는 스텟 초기화
    public static void RecalculateStats(MonsterData monster)
    {
        monster.maxHp = monster.maxHp + 12 * (monster.level - 1);
        monster.attack = monster.attack + 3 * (monster.level - 1);
        monster.defense = monster.defense + 3 * (monster.level - 1);
        monster.speed = monster.speed + 3 * (monster.level - 1);
        monster.maxExp = monster.maxExp + 25 * (monster.level - 1);
        monster.expReward = monster.expReward + 25 * (monster.level - 1);
        monster.goldReward = monster.goldReward + 30 * (monster.level - 1);
    }
}

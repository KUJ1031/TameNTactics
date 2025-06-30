public static class MonsterStatsManager
{
    // 경험치 추가 & 레벨업 시 스텟 변화
    public static void AddExp(Monster monster, int expAmount)
    {
        monster.curExp += expAmount;

        while (monster.curExp >= monster.maxExp && monster.level < 30)
        {
            monster.curExp -= monster.maxExp;
            monster.level++;
            RecalculateStats(monster);
            monster.curHp = monster.maxHp; // 레벨업 시 체력 회복
        }
    }

    // 레벨에 맞는 스탯 재계산
    public static void RecalculateStats(Monster monster)
    {
        int levelMinusOne = monster.level - 1;

        monster.maxHp = monster.monsterData.maxHp + 12 * levelMinusOne;
        monster.attack = monster.monsterData.attack + 3 * levelMinusOne;
        monster.defense = monster.monsterData.defense + 3 * levelMinusOne;
        monster.speed = monster.monsterData.speed + 3 * levelMinusOne;
        monster.maxExp = monster.monsterData.maxExp + 25 * levelMinusOne;
        monster.expReward = monster.monsterData.expReward + 25 * levelMinusOne;
        monster.goldReward = monster.monsterData.goldReward + 30 * levelMinusOne;

        // 만약 curHp가 maxHp보다 크다면 맞춰줌
        if (monster.curHp > monster.maxHp)
            monster.curHp = monster.maxHp;
    }
}

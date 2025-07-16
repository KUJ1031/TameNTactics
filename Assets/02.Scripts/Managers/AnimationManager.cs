using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationManager : Singleton<AnimationManager>
{
    [SerializeField] private AnimationController damagedPrefab;

    private List<MonsterCharacter> allMonsters = new();

    public void SubscribeEvents()
    {
        foreach (var m in allMonsters)
            m.monster.DamagedAnimation += OnMonsterDamagedAnimation;
    }

    private void OnDisable()
    {
        if (allMonsters == null) return;
        foreach (var m in allMonsters)
            m.monster.DamagedAnimation -= OnMonsterDamagedAnimation;
    }

    private void OnMonsterDamagedAnimation(Monster monster)
    {
        MonsterCharacter mc = null;
        
        foreach (var mon in allMonsters)
        {
            if (mon.monster == monster)
            {
                mc = mon;
                break;
            }
        }
        
        if (mc == null) return;

        Vector3 spawnPos = mc.transform.position;
        spawnPos += Vector3.up * 1f;

        AnimationController damagedAnimation = Instantiate(damagedPrefab, spawnPos, Quaternion.identity);

        damagedAnimation.SetUp();
    }

    public void AddAllMonsters(MonsterCharacter monster)
    {
        if (!allMonsters.Contains(monster))
        {
            allMonsters.Add(monster);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private AnimationController damagedPrefab;

    private List<MonsterCharacter> AllMonsters =>
        BattleManager.Instance.BattleEntryCharacters
            .Concat(BattleManager.Instance.BattleEnemyCharacters)
            .ToList();
    
    private List<MonsterCharacter> cachedMonsters;

    public void RegisterMonsters()
    {
        cachedMonsters = AllMonsters;
        foreach (var m in cachedMonsters)
            m.monster.DamagedAnimation += OnMonsterDamagedAnimation;
    }

    private void FindAllMonsters()
    {
        cachedMonsters = AllMonsters;
        foreach (var m in cachedMonsters)
            m.monster.DamagedAnimation += OnMonsterDamagedAnimation;
    }

    private void OnDisable()
    {
        if (cachedMonsters == null) return;
        foreach (var m in cachedMonsters)
            m.monster.DamagedAnimation -= OnMonsterDamagedAnimation;
    }

    private void OnMonsterDamagedAnimation(Monster monster)
    {
        MonsterCharacter mc = null;
        
        foreach (var mon in AllMonsters)
        {
            if (mon.monster == monster)
            {
                mc = mon;
            }
        }
        
        if (mc == null) return;

        Vector3 spawnPos = mc.transform.position;

        AnimationController damagedAnimation = Instantiate(damagedPrefab, spawnPos, Quaternion.identity);
        damagedAnimation.SetUp();
    }
}

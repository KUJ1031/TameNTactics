using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionUI : FieldMenuBaseUI
{

    [SerializeField] private Image MonsterImage;
    [SerializeField] private TextMeshProUGUI MonsterName;
    [SerializeField] private TextMeshProUGUI MonsterType;
    [SerializeField] private TextMeshProUGUI MonsterCatch;
    [SerializeField] private TextMeshProUGUI MonsterSpawnArea;
    [SerializeField] private TextMeshProUGUI MonsterSpawnTime;
    [SerializeField] private TextMeshProUGUI MonstereNcounterCount;
    [SerializeField] private TextMeshProUGUI MonsterCaptureCount;
    [SerializeField] private TextMeshProUGUI MonsterPersonality;

    [SerializeField] private TextMeshProUGUI MonsterSkill1;
    [SerializeField] private TextMeshProUGUI MonsterSkill2;
    [SerializeField] private TextMeshProUGUI MonsterSkill3;

    [SerializeField] private Image CollectionSlot;




    public void Setdata(MonsterData data)
    {
        MonsterImage.sprite = data.monsterImage;
        MonsterName.text = data.monsterName;
        MonsterType.text = data.type.ToString();
        MonsterSpawnArea.text = data.spawnArea.ToString();
        MonsterSpawnTime.text = data.spawnTime.ToString();
        MonstereNcounterCount.text = data.encounterCount.ToString();
        MonsterCaptureCount.text = data.captureCount.ToString();
        MonsterPersonality.text = data.personality.ToString();
    }

}

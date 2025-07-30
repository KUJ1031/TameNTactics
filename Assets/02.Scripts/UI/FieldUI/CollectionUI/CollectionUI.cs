using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionUI : FieldMenuBaseUI
{

    [SerializeField] private Image MonsterImage;
    [SerializeField] private TextMeshProUGUI MonsterNumber;
    [SerializeField] private TextMeshProUGUI MonsterName;
    [SerializeField] private TextMeshProUGUI MonsterType;
    [SerializeField] private TextMeshProUGUI MonsterSpawnArea;
    [SerializeField] private TextMeshProUGUI MonsterSpawnTime;
    [SerializeField] private TextMeshProUGUI MonsterEncounterCount;
    [SerializeField] private TextMeshProUGUI MonsterCaptureCount;
    [SerializeField] private TextMeshProUGUI MonsterPersonality;

    //[SerializeField] private TextMeshProUGUI MonsterSkill1;
    //[SerializeField] private TextMeshProUGUI MonsterSkill2;
    //[SerializeField] private TextMeshProUGUI MonsterSkill3;

    //[SerializeField] private Image CollectionSlot;

    [Header("해금에 필요한 포섭수")]
    [SerializeField] private int areaNuRockCount = 1;
    [SerializeField] private int timeNuRockCount = 3;
    [SerializeField] private int personalityNuRockCount = 5;


    public void SetData(MonsterData data)
    {
        MonsterImage.sprite = data.monsterImage;
        MonsterImage.color = data.encounterCount == 0 ? Color.black : Color.white;
        MonsterNumber.text = data.monsterNumber.ToString();
        MonsterName.text = data.encounterCount == 0 ? "???" : data.monsterName;
        MonsterType.text = data.encounterCount == 0 ? "???" : data.type.ToKorean();
        MonsterPersonality.text = data.captureCount < personalityNuRockCount ? "???" : data.personality.ToKorean();
        MonsterSpawnArea.text = data.captureCount < areaNuRockCount ? "???" : data.spawnArea .ToKorean();
        MonsterSpawnTime.text = data.captureCount < timeNuRockCount ? "???" : data.spawnTime.ToKorean();
        MonsterEncounterCount.text = data.encounterCount.ToString();
        MonsterCaptureCount.text = data.captureCount.ToString();
    }

}

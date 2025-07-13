using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    [SerializeField] private BattleSelectView battleSelectView;
    [SerializeField] private BattleInfoView battleInfoView;
    [SerializeField] private SkillView skillView;
    [SerializeField] private MenuView menuView;
    [SerializeField] private EmbraceView embraceView;

    public EmbraceView EmbraceView { get { return embraceView; } }
    public BattleSelectView BattleSelectView { get { return battleSelectView; } }

    [SerializeField] private BattleUIButtonHandler battleUIButtonHandler;

    [SerializeField] private DamagePopup damagePopupPrefab;

    [Header("포섭하기 미니게임")]
    [SerializeField] private GameObject miniGamePrefab;

    public GameObject MiniGamePrefab { get { return miniGamePrefab; } }

    private List<MonsterCharacter> allMonsterCharacters = new();

    private void OnEnable()
    {
        EventBus.OnMonsterDead -= RemoveGauge;
        EventBus.OnMonsterDead += RemoveGauge;
    }

    private void OnDisable()
    {
        EventBus.OnMonsterDead -= RemoveGauge;
        foreach (var mc in allMonsterCharacters)
        {
            mc.monster.DamagePopup -= OnMonsterDamaged;
        }
    }

    public void OnAttackButtonClick()
    {
        battleSelectView.HideSelectPanel();
    }

    public void IntoBattleMenuSelect()
    {
        battleSelectView.HideSkillPanel();
    }

    public void ShowMonsterSkills(MonsterData monsterData)
    {
        if (monsterData == null || monsterData.skills == null) return;

        skillView.ShowSkillList(monsterData.skills);
    }


    // 배틀씬 진입 시 몬스터 체력, 궁극기 게이지 세팅
    public void SettingMonsterGauge(Transform ally, Transform enemy)
    {
        allMonsterCharacters.Clear();

        MonsterCharacter[] allyChildren = ally.GetComponentsInChildren<MonsterCharacter>();
        MonsterCharacter[] enemyChildren = enemy.GetComponentsInChildren<MonsterCharacter>();

        allMonsterCharacters.AddRange(allyChildren);
        allMonsterCharacters.AddRange(enemyChildren);

        foreach (var mon in allMonsterCharacters)
        {
            mon.monster.DamagePopup += OnMonsterDamaged;

            Vector3 screenPos = Camera.main.WorldToScreenPoint(mon.transform.position);
            GameObject gauge = battleSelectView.InitiateGauge(screenPos);

            float hpRatio = (float)mon.monster.CurHp / mon.monster.CurMaxHp;

            battleSelectView.SetHpGauge(gauge, hpRatio);

            mon.gameObject.AddComponent<MonsterGaugeHolder>().InitGauge(gauge);
        }
    }

    public void SettingMonsterSelecter(Transform ally, Transform enemy)
    {
        foreach (var mon in allMonsterCharacters)
        {
            var selectImage = battleSelectView.InitiateSelectImage(mon.transform);

            var hoverHandler = mon.GetComponent<MonsterHoverHandler>();

            if (hoverHandler != null)
            {
                hoverHandler.SetUp(selectImage);
            }
        }
    }

    public void UpdateHpGauge(Monster monster)
    {
        MonsterCharacter mc = FindMonsterCharacter(monster);

        if (mc == null) return;

        var gaugeHolder = mc.GetComponent<MonsterGaugeHolder>();

        if (gaugeHolder == null || gaugeHolder.gauge == null) return;

        float hpRatio = (float)monster.CurHp / monster.CurMaxHp;
        battleSelectView.SetHpGauge(gaugeHolder.gauge, hpRatio);
    }

    public void UpdateUltimateGauge(Monster monster)
    {
        MonsterCharacter mc = FindMonsterCharacter(monster);

        if (mc == null) return;

        var gaugeHolder = mc.GetComponent<MonsterGaugeHolder>();

        if (gaugeHolder == null || gaugeHolder.gauge == null) return;

        float ultimateRatio = (float)monster.CurUltimateCost / monster.MaxUltimateCost;
        battleSelectView.SetUltimateGauge(gaugeHolder.gauge, ultimateRatio);
    }

    public void RemoveGauge(Monster monster)
    {
        MonsterCharacter mc = FindMonsterCharacter(monster);

        if (mc == null) return;

        var gaugeHolder = mc.GetComponent<MonsterGaugeHolder>();

        if (gaugeHolder != null && gaugeHolder.gauge != null)
        {
            Destroy(gaugeHolder.gauge);
            gaugeHolder.gauge = null;
        }
    }

    private MonsterCharacter FindMonsterCharacter(Monster monster)
    {
        foreach (var mc in allMonsterCharacters)
        {
            if (mc.monster == monster)
            {
                return mc;
            }
        }
        return null;
    }

    public void OffSelectMonsterUI()
    {
        battleSelectView.OffSelectMonster();
    }

    public void BattleEndMessage(bool isWin)
    {
        battleInfoView.ShowEndBattleMessage(isWin);
    }

    private void OnMonsterDamaged(Monster monster, int damage)
    {
        MonsterCharacter mc = FindMonsterCharacter(monster);
        if (mc == null) return;

        Vector3 spawnPos = mc.transform.position + Vector3.up * 1.5f;

        DamagePopup popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);
        popup.SetUp(damage);
    }
}
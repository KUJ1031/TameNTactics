using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleSelectView : MonoBehaviour
{
    public Button attackButton;
    public Button attackInventoryButton;
    public Button embraceButton;
    public Button runButton;
    public Button cancelButton;

    [SerializeField] private GameObject selectPanel;
    [SerializeField] private GameObject skillPanel;
    [SerializeField] private GameObject monsterInfoPanel;
    [SerializeField] private GameObject behaviorPanel;
    [SerializeField] private GameObject selectMonsterImage;

    [SerializeField] private Canvas gaugeCanvas;
    [SerializeField] private Canvas battleSelectCanvas;

    [SerializeField] private GameObject allyHighLight;
    [SerializeField] private GameObject enemyHighLight;

    [SerializeField] private MonsterTypeIconDB monsterTypeIconDB;

    public Canvas GaugeCanvas { get { return gaugeCanvas; } }

    public void ShowCancelButton()
    {
        if (PlayerManager.Instance.player.playerBattleTutorialCheck) cancelButton.gameObject.SetActive(true);

        cancelButton.onClick.AddListener(() => BattleSystem.Instance.ChangeState(new PlayerMenuState(BattleSystem.Instance)));
    }

    public void HideCancelButton()
    {
        cancelButton.gameObject.SetActive(false);
    }

    public void ShowSelectPanel()
    {
        selectPanel.SetActive(true);
    }

    public void HideSelectPanel()
    {
        selectPanel.SetActive(false);
    }

    public void ShowBehaviorPanel(string message)
    {
        behaviorPanel.GetComponentInChildren<TextMeshProUGUI>().text = message;
        if (behaviorPanel.activeSelf == false)
        {
            behaviorPanel.SetActive(true);
        }
    }

    public void HideBeHaviorPanel()
    {
        if (behaviorPanel.activeSelf == true)
        {
            behaviorPanel.SetActive(false);
        }
    }

    // 배틀 중의 선택지 Panel 숨김
    public void HideSkillPanel()
    {
        if (skillPanel != null)
        {
            skillPanel.SetActive(false);
        }

        if (selectPanel != null)
        {
            selectPanel.SetActive(true);
        }
    }

    public GameObject InitiateInfo(Vector3 screenPos)
    {
        GameObject infoPanel = Instantiate(monsterInfoPanel, gaugeCanvas.transform);

        infoPanel.transform.position = screenPos;
        infoPanel.SetActive(true);

        return infoPanel;
    }

    public GameObject InitiateSelectImage(Transform tr)
    {
        GameObject selectImage = Instantiate(selectMonsterImage, battleSelectCanvas.transform);

        // 몬스터의 Sprite Renderer 참조
        BoxCollider2D collider = tr.GetComponent<BoxCollider2D>();

        // 몬스터 sprite의 중앙
        Vector3 monsterCenterPos = collider.bounds.center;

        // UI 좌표를 몬스터의 좌표로 옮기기 위한 변수
        Vector3 screenPos = Camera.main.WorldToScreenPoint(monsterCenterPos);

        Canvas canvas = selectImage.GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out Vector2 localPoint);

        selectImage.GetComponent<RectTransform>().localPosition = localPoint;
        selectImage.SetActive(false);

        return selectImage;
    }

    public void SetHpGauge(GameObject panel, float hpRatio)
    {
        Image hpBar = panel.transform.GetChild(0).GetChild(0).GetComponent<Image>();

        hpBar.fillAmount = hpRatio;
    }

    public void SetUltimateGauge(GameObject panel, float ultimateRatio)
    {
        Image ultimateBar = panel.transform.GetChild(1).GetChild(0).GetComponent<Image>();

        ultimateBar.fillAmount = ultimateRatio;
    }

    public void SetMonsterInfo(GameObject panel, Monster monster)
    {
        TextMeshProUGUI hpText = panel.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI ultimateText = panel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();

        Image monsterType = panel.transform.GetChild(2).GetComponent<Image>();

        TextMeshProUGUI nameText = panel.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI levelText = panel.transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>();

        if (monsterTypeIconDB == null)
        {
            Debug.LogError("MonsterTypeIconDB가 존재하지 않습니다. 생성 후 다시 시도하세요.");
        }

        Sprite icon = monsterTypeIconDB.GetTypeIcon(monster.type);

        if (icon != null)
        {
            hpText.text = $"{monster.CurHp} / {monster.CurMaxHp}";
            ultimateText.text = $"{monster.CurUltimateCost} / {monster.MaxUltimateCost}";
            monsterType.sprite = icon;
            nameText.text = $"{monster.monsterName}";
            levelText.text = $"Lv {monster.Level}";
        }
    }

    public void OffSelectMonster()
    {
        selectMonsterImage.SetActive(false);
    }

    public void InteractableEmbraceButton_false()
    {
        embraceButton.interactable = false;
    }

    public void InteractableEmbraceButton_true()
    {
        embraceButton.interactable = true;
    }

    public void InteractableRunButton_false()
    {
        runButton.interactable = false;
    }

    public void InteractableRunButton_true()
    {
        runButton.interactable = true;
    }

    public void OnAllyHighLight()
    {
        allyHighLight.SetActive(true);
        enemyHighLight.SetActive(false);
    }

    public void OnEnemyHighLight()
    {
        enemyHighLight.SetActive(true);
        allyHighLight.SetActive(false);
    }

    public void OffAllHighLight()
    {
        allyHighLight.SetActive(false);
        enemyHighLight.SetActive(false);
    }
}

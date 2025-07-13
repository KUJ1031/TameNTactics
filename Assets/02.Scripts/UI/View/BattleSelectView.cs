using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private GameObject selectPanel;
    [SerializeField] private GameObject skillPanel;
    [SerializeField] private GameObject gaugePanel;
    [SerializeField] private GameObject behaviorPanel;
    [SerializeField] private GameObject selectMonsterImage;

    [SerializeField] private Canvas gaugeCanvas;
    [SerializeField] private Canvas battleSelectCanvas;

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
        behaviorPanel.SetActive(false);
    }

    // 배틀 중의 선택지 Panel 나타냄
    public void ShowSkillPanel()
    {
        skillPanel.SetActive(true);
        selectPanel.SetActive(false);
    }

    // 배틀 중의 선택지 Panel 숨김
    public void HideSkillPanel()
    {
        skillPanel.SetActive(false);
        selectPanel.SetActive(true);
    }

    public GameObject InitiateGauge(Vector3 screenPos)
    {
        GameObject gauge = Instantiate(gaugePanel, gaugeCanvas.transform);

        gauge.transform.position = screenPos;
        gauge.SetActive(true);

        return gauge;
    }

    public GameObject InitiateSelectImage(Transform tr)
    {
        GameObject selectImage = Instantiate(selectMonsterImage, battleSelectCanvas.transform);

        // 몬스터의 Sprite Renderer 참조
        SpriteRenderer sr = tr.GetComponentInChildren<SpriteRenderer>();

        // 몬스터 sprite의 중앙
        Vector3 monsterCenterPos = sr.bounds.center;

        // UI 좌표를 몬스터의 좌표로 옮기기 위한 변수
        Vector3 screenPos = Camera.main.WorldToScreenPoint(monsterCenterPos);

        Canvas canvas = selectImage.GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out Vector2 localPoint);

        selectImage.GetComponent<RectTransform>().localPosition = localPoint;
        selectImage.SetActive(false);

        return selectImage;
    }

    public void SetupMonsterUI(MonsterCharacter character)
    {
        var selectImage = InitiateSelectImage(character.transform);

        var hoverHandler = character.GetComponent<MonsterHoverHandler>();

        if (hoverHandler != null)
        {
            hoverHandler.SetUp(selectImage);
        }
    }

    public void SetHpGauge(GameObject gauge, float hpRatio)
    {
        Image hpBar = gauge.transform.GetChild(0).GetComponent<Image>();

        hpBar.fillAmount = hpRatio;
    }

    public void SetUltimateGauge(GameObject gauge, float ultimateRatio)
    {
        Image ultimateBar = gauge.transform.GetChild(1).GetComponent<Image>();

        ultimateBar.fillAmount = ultimateRatio;
    }

    public void MoveSelectMonster(Transform tr)
    {
        // 몬스터의 Sprite Renderer 참조
        SpriteRenderer sr = tr.GetComponentInChildren<SpriteRenderer>();

        // 몬스터 sprite의 중앙
        Vector3 monsterCenterPos = sr.bounds.center;

        // UI 좌표를 몬스터의 좌표로 옮기기 위한 변수
        Vector3 screenPos = Camera.main.WorldToScreenPoint(monsterCenterPos);

        Canvas canvas = selectMonsterImage.GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out localPoint);

        selectMonsterImage.transform.position = localPoint;
        selectMonsterImage.gameObject.SetActive(true);
    }

    public void OffSelectMonster()
    {
        selectMonsterImage.gameObject.SetActive(false);
    }
}

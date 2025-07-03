using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private RectTransform selectMonsterImage;
    [SerializeField] private Canvas gaugeCanvas;

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

    public void SetGauge(GameObject gauge, float hpRatio, float ultimateRatio)
    {
        Image hpBar = gauge.transform.GetChild(0).GetComponent<Image>();
        Image ultimateBar = gauge.transform.GetChild(1).GetComponent<Image>();

        hpBar.fillAmount = hpRatio;
        ultimateBar.fillAmount = ultimateRatio;
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
        Vector3 screenPos = Camera.main.WorldToScreenPoint(tr.position);

        Canvas canvas = selectMonsterImage.GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out localPoint);

        selectMonsterImage.localPosition = localPoint;
        selectMonsterImage.gameObject.SetActive(true);
    }
}

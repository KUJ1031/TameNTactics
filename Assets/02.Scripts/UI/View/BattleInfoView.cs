using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleInfoView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI battleDialogue;
    [SerializeField] private RectTransform passivePanel;
    [SerializeField] private GameObject passiveImage;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;

    [SerializeField] private TextMeshProUGUI gainExp;
    [SerializeField] private TextMeshProUGUI gainGold;

    private StringBuilder logBuilder = new();

    // 배틀 중 Dialogue를 출력해줄 메서드
    public void BattleDialogue(string battleLog)
    {
        logBuilder.AppendLine(battleLog);
        battleDialogue.text = logBuilder.ToString();
    }

    public void ClearBattleEndPanel()
    {
        if (victoryPanel.activeSelf)
        {
            victoryPanel.SetActive(false);
        }
        else if (defeatPanel.activeSelf)
        {
            defeatPanel.SetActive(false);
        }
    }

    public void ShowVictoryPanel(int exp, int gold)
    {
        victoryPanel.SetActive(true);
        gainExp.text = exp.ToString();
        gainGold.text = gold.ToString();
    }

    public void ShowDefeatPanel()
    {
        defeatPanel.SetActive(true);
    }

    public GameObject InitializePassiveIcon(Sprite skillIcon)
    {
        GameObject passiveIcon = Instantiate(passiveImage, passivePanel);

        Image passiveSkillImage = passiveIcon.transform.Find("PassiveIcon").GetComponent<Image>();
        passiveSkillImage.sprite = skillIcon;

        return passiveIcon;
    }
}

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
    [SerializeField] private RectTransform selectMonsterImage;


    // 배틀씬 진입 시 메뉴 진입
    public void OnBattleSelectMenu()
    {
        if (selectPanel.activeSelf == true)
        {
            selectPanel.SetActive(false);
        }
        else
        {
            selectPanel.SetActive(true);
        }
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

    public void MoveSelectMonster(Transform tr)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(tr.position);
        selectMonsterImage.position = screenPos;
        selectMonsterImage.gameObject.SetActive(true);
    }
}

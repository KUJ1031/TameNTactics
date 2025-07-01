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

    [SerializeField] private RectTransform selectMonsterImage;

    public GameObject skillPanel;

    // 배틀 중의 선택지 Panel 나타냄
    public void ShowSkillPanel()
    {
        skillPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    // 배틀 중의 선택지 Panel 숨김
    public void HideSkillPanel()
    {
        skillPanel.SetActive(false);
        gameObject.SetActive(true);
    }

    public void MoveSelectMonster(Transform tr)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(tr.position);
        selectMonsterImage.position = screenPos;
        selectMonsterImage.gameObject.SetActive(true);
    }
}

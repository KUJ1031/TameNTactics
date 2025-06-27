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

    public GameObject skillPanel;

    public void ShowSkillPanel()
    {
        skillPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void HideSkillPanel()
    {
        skillPanel.SetActive(false);
        gameObject.SetActive(true);
    }
}

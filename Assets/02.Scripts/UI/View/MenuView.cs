using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
    public Button menuButton;
    public GameObject menuPanel;

    public void ShowPanel()
    {
        menuPanel.SetActive(true);
        menuButton.gameObject.SetActive(false);
    }

    public void HidePanel()
    {
        menuPanel.SetActive(false);
        menuButton.gameObject.SetActive(true);
    }
}

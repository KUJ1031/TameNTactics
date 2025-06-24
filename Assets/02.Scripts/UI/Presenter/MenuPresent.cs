using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuPresent : MonoBehaviour
{
    [SerializeField] private MenuView menuView;

    private bool isPanelOpen = false;

    void Start()
    {
        menuView.menuButton.onClick.AddListener(OnMenuButtonClick);
    }

    void Update()
    {
        if (isPanelOpen && Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIObject())
            {
                ClosePanel();
            }
        }
    }

    private void OnMenuButtonClick()
    {
        isPanelOpen = true;
        menuView.ShowPanel();
    }

    private void ClosePanel()
    {
        isPanelOpen = false;
        menuView.HidePanel();
    }

    private bool IsPointerOverUIObject()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}

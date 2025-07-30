using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum PopupType
{
    EntrySwap,  //엔트리 교체
    Confirm,    //예/아니오
    Warning     //예
}

public class FieldUIManager : MonoBehaviour
{
    public static FieldUIManager Instance { get; private set; }

    [SerializeField] private FieldMenuBaseUI[] uiList;
    [SerializeField] private GameObject LeftMenuUI;
    [SerializeField] private GameObject BaseUI;
    [SerializeField] private GameObject fieldBaseUI;

    //[SerializeField] private GameObject swapPopupPrefab;
    //[SerializeField] private GameObject confirmPopupPrefab;
    //[SerializeField] private Transform uiCanvas;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    //메뉴열기
    public void OpenUI<T>() where T : FieldMenuBaseUI
    {
        BaseUI.SetActive(false);
        LeftMenuUI.SetActive(true);
        foreach (FieldMenuBaseUI ui in uiList)
        {
            if (ui is T) ui.Open();
            else ui.Close();
        }
    }

    //메뉴닫기
    public void CloseAllUI()
    {
        BaseUI.SetActive(true);
        LeftMenuUI.SetActive(false);
        foreach (var ui in uiList)
        {
            ui.Close();
        }
        if (fieldBaseUI != null) fieldBaseUI.GetComponent<FieldBaseUI>().RefreshEntrySlots();
    }

    //Confirm 팝업
    public void OpenConfirmPopup(PopupType type, string message, Action<bool> onConfirmed)
    {
        PopupUIManager.Instance.ShowPanel<ConfirmPopup>("SimplePopup", popup =>
        {
            popup.Open(type,message, onConfirmed);
        });
    }

    //EntrySwap 팝업
    public void OpenEntrySwapPopup(Action<Monster> onSwapped)
    {
        PopupUIManager.Instance.ShowPanel<PlayerEntrySwapPopup>("PlayerEntrySwapPopup", popup =>
        {
            popup.Open(onSwapped);
        });
    }

}

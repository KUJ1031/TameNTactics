using System;
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

    [SerializeField] private GameObject swapPopupPrefab;
    [SerializeField] private GameObject confirmPopupPrefab;
    [SerializeField] private Transform uiCanvas;
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
    }

    public void OpenPopupUI(PopupType type, params object[] args)
    {
        switch (type)
        {
            case PopupType.EntrySwap:
                if (args.Length >= 1 && args[0] is Action<Monster> onSwapped)
                {
                    SwapEntryMonster(onSwapped);
                }
                else { Debug.LogWarning("EntrySwap Popup에 필요한 매개변수가 부족합니다."); }
                break;
            case PopupType.Confirm:
                if (args.Length >= 2 && args[0] is string message && args[1] is Action<bool> onConfirm)
                {
                    Confirm(message, onConfirm);
                }
                else { Debug.LogWarning("Confirm Popup에 필요한 매개변수가 부족합니다."); }
                break;

            default:
                Debug.LogWarning($"지원하지 않는 PopupType: {type}");
                break;
        }
    }

    //엔트리스왑 팝업
    private void SwapEntryMonster(Action<Monster> onSwapped)
    {
        GameObject go = Instantiate(swapPopupPrefab, uiCanvas);
        var popup = go.GetComponent<PlayerEntrySwapPopup>();
        popup.Open(onSwapped);
    }

    //컨펌 팝업
    private void Confirm(string massage, Action<bool> isOK)
    {
        GameObject go = Instantiate(confirmPopupPrefab, uiCanvas);
        var popup = go.GetComponent<ConfirmPopup>();
        popup.Open(massage, isOK);
    }
}

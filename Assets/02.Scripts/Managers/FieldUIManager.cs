using UnityEngine;

public class FieldUIManager : MonoBehaviour
{
    public static FieldUIManager Instance { get; private set; }

    [SerializeField] private FieldMenuBaseUI[] uiList;
    [SerializeField] private GameObject LeftMenuUI;
    [SerializeField] private GameObject BaseUI;

    [SerializeField] private GameObject swapPopupPrefab;
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

    //팝업만들어 띄우기
    public void SwapEntryMonster(System.Action<Monster> onSwapped)
    {
        GameObject go = Instantiate(swapPopupPrefab, uiCanvas);
        var popup = go.GetComponent<PlayerEntrySwapPopup>();
        popup.Open(onSwapped);
    }
}

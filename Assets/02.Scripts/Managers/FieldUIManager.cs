using UnityEngine;

public class FieldUIManager : MonoBehaviour
{
    public static FieldUIManager Instance { get; private set; }

    [SerializeField] private FieldMenuBaseUI[] uiList;
    [SerializeField] private GameObject LeftMenuUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OpenUI<T>() where T : FieldMenuBaseUI
    {
        LeftMenuUI.SetActive(true);
        foreach (FieldMenuBaseUI ui in uiList)
        {
            if (ui is T) ui.Open();
            else ui.Close();
        }
    }

    public void CloseAllUI()
    {
        LeftMenuUI.SetActive(false);
        foreach (var ui in uiList)
        {
            ui.Close();
        }
    }
}

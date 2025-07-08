using UnityEngine;
using static FieldMenuBaseUI;


public class FieldUIManager : MonoBehaviour
{
    public static FieldUIManager Instance { get; private set; }

    [SerializeField] private FieldMenuBaseUI[] uiList;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OpenUI<T>() where T : FieldMenuBaseUI
    {
        foreach (FieldMenuBaseUI ui in uiList)
        {
            if (ui is T) ui.Open();
            else ui.Close();
        }
    }

    public void CloseAllUI()
    {
        foreach (var ui in uiList)
        {
            ui.Close();
        }
    }
}

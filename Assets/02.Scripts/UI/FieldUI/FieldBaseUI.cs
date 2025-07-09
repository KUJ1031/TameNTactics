using UnityEngine;
using UnityEngine.UI;

public class FieldBaseUI : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    
    void Start()
    {
        menuButton.onClick.AddListener(() => SetFieldBaseUI());
    }

    public void SetFieldBaseUI() 
    {
        FieldUIManager.Instance.OpenUI<PlayerInfoUI>();
    }
}

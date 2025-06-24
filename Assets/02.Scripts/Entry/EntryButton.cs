using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntryButton : MonoBehaviour
{
    [Header("버튼 연결 요소들")]
    public MonsterData monsterData; // Fireman 등
    public EntryManager entryManager;
    public TextMeshProUGUI buttonText; // UI 버튼 텍스트

    private void Start()
    {
        UpdateButtonUI();
    }

    public void OnClick()
    {
        entryManager.ToggleEntry(monsterData);
        UpdateButtonUI();
    }

    public void UpdateButtonUI()
    {
        if (entryManager.IsInEntry(monsterData))
            buttonText.text = "UnEquip";
        else
            buttonText.text = "Equip";
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealingResultPopup : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform contentPanel;
    public Button closeButton;

    public void Show(List<HealedMonsterInfo> healedMonsters)
    {
        foreach (Transform child in contentPanel)
            Destroy(child.gameObject);

        foreach (var info in healedMonsters)
        {
            var item = Instantiate(itemPrefab, contentPanel);
            var ui = item.GetComponent<HealingResultItemUI>();
            ui.Setup(info);
        }
        FieldUIManager.Instance.CloseAllUI();

        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

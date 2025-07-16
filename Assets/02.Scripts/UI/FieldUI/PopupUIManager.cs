using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupUIManager : Singleton<PopupUIManager>
{
    [SerializeField] private GameObject blockerPanel; // 클릭 막는 반투명 배경
    [SerializeField] private List<GameObject> panelObjects; // 미리 배치된 모든 팝업들 (SetActive(false) 상태)

    private Dictionary<string, GameObject> panelDict = new();
    private Stack<GameObject> openedPanels = new(); // 열려 있는 순서대로 관리

    private void Start()
    {
        foreach (var panel in panelObjects)
        {
            if (panel != null)
                panelDict[panel.name] = panel;
        }

        blockerPanel?.SetActive(false);
    }

    // 이름으로 팝업 열기
    public T ShowPanel<T>(string panelName, Action<T> onOpened = null) where T : Component
    {
        if (!panelDict.TryGetValue(panelName, out var panel))
        {
            Debug.LogWarning($"[PanelManager] 패널 '{panelName}' 없음.");
            return null;
        }

        // 이미 열려있으면 무시
        if (panel.activeSelf)
        {
            Debug.Log($"[PanelManager] 패널 '{panelName}' 이미 열림.");
            return panel.GetComponent<T>();
        }

        // 열기
        panel.SetActive(true);
        panel.transform.SetAsLastSibling(); // 제일 앞에 위치

        openedPanels.Push(panel);
        UpdateBlockerVisibility();

        var component = panel.GetComponent<T>();
        onOpened?.Invoke(component);
        return component;
    }

    // 특정 패널 닫기
    public void ClosePanel(GameObject panel)
    {
        if (panel == null || !panel.activeSelf) return;

        panel.SetActive(false);

        if (openedPanels.Count > 0 && openedPanels.Peek() == panel)
            openedPanels.Pop();

        UpdateBlockerVisibility();
    }

    // 가장 위에 있는 패널 닫기
    public void CloseTopPanel()
    {
        if (openedPanels.Count == 0) return;

        GameObject top = openedPanels.Pop();
        top.SetActive(false);

        UpdateBlockerVisibility();
    }

    // 모두 닫기
    public void CloseAllPanels()
    {
        while (openedPanels.Count > 0)
        {
            GameObject panel = openedPanels.Pop();
            panel.SetActive(false);
        }

        UpdateBlockerVisibility();
    }

    // Blocker 활성화/비활성화
    private void UpdateBlockerVisibility()
    {
        if (blockerPanel != null)
            blockerPanel.SetActive(openedPanels.Count > 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyRebinderManager : MonoBehaviour
{
    public static KeyRebinderManager Instance;
    public InputActionAsset inputActions;
    private KeyBindableField currentField;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        LoadAllSavedBindings();
    }

    //사용지기 입력할 준비가 된 필드 지정
    public void SetActiveField(KeyBindableField field)
    {
        currentField = field;
    }

    //Player
    public void LoadAllSavedBindings()
    {
        foreach (var map in inputActions.actionMaps)
        {
            foreach (var action in map.actions)
            {
                for (int i = 0; i < action.bindings.Count; i++)
                {
                    string key = $"{map.name}.{action.name}.{i}";
                    if (PlayerPrefs.HasKey(key))
                    {
                        string overridePath = PlayerPrefs.GetString(key);
                        action.ApplyBindingOverride(i, overridePath);
                        Debug.Log($"[로딩됨] {key} → {overridePath}");
                    }
                }
            }
        }
    }

    // 키 값을 저장한다.
    private void SaveBinding(string actionMap, string actionName, int bindingIndex, string overridePath)
    {
        string key = $"{actionMap}.{actionName}.{bindingIndex}";
        PlayerPrefs.SetString(key, overridePath);
        PlayerPrefs.Save();
    }
    private void Update()
    {
        if (currentField == null) return;
        foreach (var key in Keyboard.current.allKeys)
        {
            if (key.wasPressedThisFrame)
            {
                var actionMap = inputActions.FindActionMap(currentField.actionMapName, true);
                var action = actionMap.FindAction(currentField.actionName, true);
                if (actionMap == null)
                {
                    Debug.LogError($"Action Map '{currentField.actionMapName}' not found!");
                    return;
                }
                if (action == null)
                {
                    Debug.LogError($"Action '{currentField.actionName}' not found in Action Map '{currentField.actionMapName}'!");
                    return;
                }
                try
                {
                    string newPath = $"<Keyboard>/{key.name}";
                    action.ApplyBindingOverride(currentField.bindingIndex, newPath);
                    SaveBinding(currentField.actionMapName, currentField.actionName, currentField.bindingIndex, newPath);
                    currentField.SetKey(key.name); // UI 텍스트 반영
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"바인딩 중 오류: {ex.Message}");
                }
                currentField = null;
                break;
            }
        }
    }
}

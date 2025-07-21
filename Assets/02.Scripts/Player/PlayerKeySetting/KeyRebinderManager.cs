using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyRebinderManager : Singleton<KeyRebinderManager>
{
    public InputActionAsset inputActions;
    private KeyBindableField currentField;
    public GameObject CheckUI;
    public GameObject CompleteUI;

    [Header("플레이어 키 설정 UI")]
    public GameObject PlayerKeySettingUI;

    [Header("UI 버튼 연결")]
    public Button playerKeySettingButton;
    public Button exitButton; // UI에서 버튼을 연결

    protected override void Awake()
    {
        base.Awake();
        LoadAllSavedBindings(); // 저장된 키 로드
    }

    private void Start()
    {
        RefreshAllKeyFields();
    }

    // 플레이어 키 설정 UI를 열기 위한 버튼 클릭 이벤트
    public void OnPlayerKeySettingButton()
    {
        PlayerKeySettingUI.gameObject.SetActive(true);
    }

    // 플레이어 키 설정 UI를 닫기 위한 버튼 클릭 이벤트
    public void OnExitButton()
    {
        PlayerKeySettingUI.gameObject.SetActive(false);
    }
    //사용지기 입력할 준비가 된 필드 지정
    public void SetActiveField(KeyBindableField field)
    {
        currentField = field;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ResetAllBindings();
        }
        if (currentField == null) return;
        foreach (var key in Keyboard.current.allKeys)
        {
            if (key.wasPressedThisFrame)
            {
                var actionMap = inputActions.FindActionMap(currentField.actionMapName, true);
                var action = actionMap.FindAction(currentField.actionName, true);
                if (actionMap == null)
                {
                    return;
                }
                if (action == null)
                {
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
                    Debug.LogWarning($"Key bind 실패: {ex.Message}");
                }
                currentField = null;
                break;
            }
        }
    }
    public void SaveBinding(string actionMap,string actionName, int bindingIndex, string overridePath)
    {
        string key = $"{actionMap}.{actionName}.{bindingIndex}";
        PlayerPrefs.SetString(key, overridePath);
        PlayerPrefs.Save();
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
                    }
                }
            }
        }
    }

    public void CheckButton()
    {
        bool check = false;
        foreach (var Fields in KeyBindableField.allFields)
        {
            if (Fields.inputField.text == "Press a key..." || string.IsNullOrEmpty(Fields.inputField.text))
            {
                check = true;
            }
        }
        Debug.Log(check);
        if (check)
        {
            CheckUI.SetActive(true);
        }
        else
        {
            CompleteUI.SetActive(true);
        }
    }

    public void ResetAllBindings()
    {
        foreach (var map in inputActions.actionMaps)
        {
            foreach (var action in map.actions)
            {
                action.RemoveAllBindingOverrides();
            }
        }
        PlayerPrefs.DeleteAll(); // 또는 바인딩 관련 키만 삭제하고 싶다면 조건 추가
        PlayerPrefs.Save();
        RefreshAllKeyFields();
    }
    public void RefreshAllKeyFields()
    {
        KeyBindableField[] fields = FindObjectsOfType<KeyBindableField>();
        foreach (var field in fields)
        {
            field.UpdateKeyDisplay();
        }
    }


    // 플레이어의 현재 키 바인딩을 Player 인스턴스에 저장
    public void SaveCurrentBindingsToPlayer(Player player)
    {
        if (player == null || PlayerManager.Instance.player.playerKeySetting == null) return;

        foreach (var map in inputActions.actionMaps)
        {
            foreach (var action in map.actions)
            {
                for (int i = 0; i < action.bindings.Count; i++)
                {
                    var binding = action.bindings[i];
                    if (!binding.isPartOfComposite) // 조합 키 제외
                    {
                        string key = $"{map.name}.{action.name}.{i}";
                        string value = binding.overridePath ?? binding.effectivePath;

                        if (!string.IsNullOrEmpty(value))
                        {
                            player.playerKeySetting[key] = value;
                        }
                    }
                }
            }
        }
    }

}

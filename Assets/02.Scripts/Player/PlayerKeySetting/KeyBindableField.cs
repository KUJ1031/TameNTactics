using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyBindableField : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    public string actionName;       // 예: "Move"
    public int bindingIndex;        // 예: Left는 3, Right는 4
    public TMP_InputField inputField;   // 연결된 InputField
    public string actionMapName;  // 예: "Player"
    public string compositePartName;  // 예: "left", "up", or "" (버튼용)
    private Outline outline;
    //Inputfiled -> 안에 키값
    public string defaultBindingPath;
    //키 중복 추가
    public static List<KeyBindableField> allFields = new List<KeyBindableField>();
    private void Awake()
    {
        if (inputField == null)
            inputField = GetComponent<TMP_InputField>();
            outline = this.GetComponent<Outline>();
        if (!allFields.Contains(this))
            allFields.Add(this);
    }
    private void Start()
    {
        UpdateKeyDisplay(); // 최초 텍스트 표시
    }

    private void OnDestroy()
    {
        allFields.Remove(this);
    }

    public void CheckForConvlicts()
    {
        string currentKey = inputField.text;
        bool isConflict = false;
        foreach (var other in allFields)
        {
            if (other == this) continue;
            if (!string.IsNullOrEmpty(currentKey) &&
                other.inputField.text.ToLower() == currentKey.ToLower())
            {
                isConflict = true;
                break;
            }
        }
        // 중복된 키가 있는지 확인 후 색상 변경
        inputField.textComponent.color = isConflict ? Color.red : Color.black;
        inputField.ForceLabelUpdate();
    }

    public static void RefeshAllConflicts()
    {
        foreach (var field in allFields)
        {
            field.CheckForConvlicts();
        }
    }

    //마우스 클릭할때
    public void OnPointerClick(PointerEventData eventData)
    {
        outline.enabled = true;
        KeyRebinderManager.Instance.SetActiveField(this);
        inputField.text = "Press a key...";
    }
    public void SetKey(string keyName)
    {
        inputField.text = keyName;
        RefeshAllConflicts();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        outline.enabled = false;
    }
    //키 반영
    public void UpdateKeyDisplay()
    {
        var actionMap = KeyRebinderManager.Instance.inputActions.FindActionMap(actionMapName, true);
        if (actionMap == null) return;
        var action = actionMap.FindAction(actionName, true);
        if (action == null) return;
        var binding = action.bindings[bindingIndex];
        string pathToUse = string.IsNullOrEmpty(binding.overridePath) ? binding.effectivePath : binding.overridePath;
        string readable = InputControlPath.ToHumanReadableString(
            pathToUse,
            InputControlPath.HumanReadableStringOptions.OmitDevice
        );
        SetKey(readable);
    }
    //초기화 버튼
    public void ResetToDefault()
    {
        var actionMap = KeyRebinderManager.Instance.inputActions.FindActionMap(actionMapName, true);
        if (actionMap == null)
        {
            Debug.LogError($"ActionMap '{actionMapName}' 없음");
            return;
        }
        var action = actionMap.FindAction(actionName, true);
        if (action == null)
        {
            Debug.LogError($"Action '{actionName}' 없음");
            return;
        }
        int indexToReset = -1;
        if (string.IsNullOrEmpty(compositePartName))
        {
            // 단일 바인딩일 경우: bindingIndex 사용
            indexToReset = bindingIndex;
        }
        else
        {
            // Composite 바인딩일 경우: 파트 이름으로 인덱스 찾기
            for (int i = 0; i < action.bindings.Count; i++)
            {
                var b = action.bindings[i];
                if (b.isPartOfComposite && b.name == compositePartName.ToLower())
                {
                    indexToReset = i;
                    break;
                }
            }
            if (indexToReset == -1)
            {
                Debug.LogError($"Composite part '{compositePartName}' not found in '{actionName}'");
                return;
            }
        }
        // 바인딩 초기화
        action.ApplyBindingOverride(indexToReset, defaultBindingPath);
        // PlayerPrefs 제거
        string key = $"{actionMapName}.{actionName}.{indexToReset}";
        PlayerPrefs.DeleteKey(key);
        PlayerPrefs.Save();
        // 액션 리활성화 (특히 Move 같은 Composite일 때 중요)
        action.Disable();
        action.Enable();
        // UI 반영
        UpdateKeyDisplay();
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyBindableField : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    public string actionName; // 예: "Move"
    public int bindingIndex;  // 예: Left는 3, Righ는 4
    public InputField inputField; // 연결된 InputField

    public string actionMapName; // 예: "Player"
    public string compositePartName; // 예: "Ieft"

    public Outline Outline;

    private void Awake()
    {
        if (inputField == null) 
            inputField = GetComponent<InputField>();

        Outline = this.GetComponent<Outline>();
    }

    //마우스 클릭할때
    public void OnPointerClick(PointerEventData eventData)
    {
        Outline.enabled = true;
        KeyRebinderManager.Instance.SetActiveField(this);
        inputField.text = "Press a key...";
    }

    public void SetKey(string keyName)
    {
        inputField.text = keyName;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        Outline.enabled = false;
    }


}

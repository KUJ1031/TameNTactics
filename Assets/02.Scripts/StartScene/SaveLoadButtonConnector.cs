using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadButtonConnector : MonoBehaviour
{
    public Button saveButton;

    private void Start()
    {
        // 버튼이 존재하는 시점에서 연결
        saveButton.onClick.AddListener(PlayerSaveManager.Instance.OnSaveButtonPressed);
    }
}

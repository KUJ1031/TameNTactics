using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadButtonConnector : MonoBehaviour
{
    public Button saveButton;
    public TextMeshProUGUI saveButtonText; // 버튼에 표시할 텍스트
    public float disableDuration = 4f; // 비활성화할 시간 (초)

    private void Start()
    {
        // 버튼 클릭 이벤트 연결
        saveButton.onClick.AddListener(OnSaveButtonClicked);
    }

    private void OnSaveButtonClicked()
    {
        // 저장 기능 실행
        PlayerSaveManager.Instance.OnSaveButtonPressed();

        // 일정 시간 버튼 비활성화
        StartCoroutine(DisableButtonTemporarily());
    }

    private IEnumerator DisableButtonTemporarily()
    {
        saveButton.interactable = false;
        saveButtonText.text = "저장 중..."; // 버튼 텍스트 변경
        yield return new WaitForSeconds(disableDuration);
        saveButtonText.text = "저장"; // 버튼 텍스트 원래대로 변경
        saveButton.interactable = true;
    }
}

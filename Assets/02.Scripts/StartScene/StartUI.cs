using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class StartUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputNicknameField; // 유저가 입력할 필드
    [SerializeField] private Button submitNicknameButton;       // 클릭할 버튼
    [SerializeField] private GameObject StartPhase_1;         // 닉네임 입력 패널
    [SerializeField] private GameObject StartPhase_2;         // 캐릭터 선택 패널

    [SerializeField] private Button selectCharacterButton;
    [SerializeField] private List<CharacterImageSelector> characterSelectors;

    private CharacterImageSelector selected;

    private void Start()
    {
        if (submitNicknameButton != null)
            submitNicknameButton.onClick.AddListener(OnSubmitClicked);

        selectCharacterButton.onClick.AddListener(OnClickSelectButton);
        selectCharacterButton.interactable = false;
    }

    public void SelectCharacter(CharacterImageSelector chosen)
    {
        if (selected == chosen)
        {
            // 이미 선택된 걸 다시 누른 경우: 선택 해제
            selected.SetSelected(false);
            selected = null;
            selectCharacterButton.interactable = false;
        }
        else
        {
            // 이전 선택 해제
            foreach (var c in characterSelectors)
                c.SetSelected(c == chosen);

            selected = chosen;
            selectCharacterButton.interactable = true;
        }
    }

    private void OnSubmitClicked()
    {
        PlayerManager.Instance.player.playerName = inputNicknameField.text;
        Debug.Log("닉네임이 설정되었습니다: " + PlayerManager.Instance.player.playerName);
        StartPhase_1.SetActive(false); // 닉네임 입력 패널 비활성화
        StartPhase_2.SetActive(true);  // 캐릭터 선택 패널 활성화
    }

    private void OnClickSelectButton()
    {
        PlayerManager.Instance.player.playerGender = selected.buttonIndex; // 선택된 캐릭터의 인덱스를 플레이어 데이터에 저장
        PlayerManager.Instance.SetSelectedPrefabIndex(selected.buttonIndex);
    }
}
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class StartUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputNicknameField; // 유저가 입력할 필드
    [SerializeField] private Button submitNicknameButton;       // 클릭할 버튼
    [SerializeField] private GameObject startPhase_1;         // 닉네임 입력 패널
    [SerializeField] private GameObject startPhase_2;         // 캐릭터 선택 패널

    [SerializeField] private Button selectCharacterButton;
    [SerializeField] private List<CharacterImageSelector> characterSelectors;

    [SerializeField] private TextMeshProUGUI nickNameWarringText;

    private CharacterImageSelector selected;

    private void OnEnable()
    {
        startPhase_1.SetActive(true);
        startPhase_2.SetActive(false);
    }

    private void OnDisable()
    {
        selected.SetSelected(false);
        selected = null;
    }

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
        string nickname = inputNicknameField.text;

        // 1. 길이 제한

        if (nickname.Length <= 0)
        {
            nickNameWarringText.gameObject.SetActive(true);
            nickNameWarringText.text = "닉네임을 입력해주세요.";
            return;
        }
        if (nickname.Length > 8)
        {
            nickNameWarringText.gameObject.SetActive(true);
            nickNameWarringText.text = "닉네임은 띄워쓰기 포함 8자 이하로 입력해주세요.";
            inputNicknameField.text = "";
            return;
        }

        // 2. 특수 문자 제한 (예시 단어 리스트)
        string[] forbiddenSymbols = { "!", "@", "#", "$", "%", "^", "&", "*", "(", ")" };
        foreach (string symbol in forbiddenSymbols)
        {
            if (nickname.Contains(symbol))
            {
                nickNameWarringText.gameObject.SetActive(true);
                nickNameWarringText.text = "닉네임에 특수 문자는 사용할 수 없습니다.";
                inputNicknameField.text = "";
                return;
            }
        }
        // 통과 시 저장
        PlayerManager.Instance.player.playerName = nickname;
        Debug.Log("닉네임이 설정되었습니다: " + nickname);
        inputNicknameField.text = "";
        startPhase_1.SetActive(false);
        startPhase_2.SetActive(true);
    }


    private void OnClickSelectButton()
    {
        int gender = selected.buttonIndex;

        PlayerManager.Instance.player.playerGender = gender;
        PlayerManager.Instance.SetSelectedPrefabIndex(gender);

        // 선택과 동시에 이미지 업데이트
        GameObject.FindObjectOfType<GameStart>().ShowStartWarringPopup();
    }
}
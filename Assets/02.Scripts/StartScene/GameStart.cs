using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameStart : MonoBehaviour
{
    public Button startButton; // 시작 버튼
    public Button loadButton; // 로드 버튼
    public Button startPanelOnButton; // 시작 패널 열기 버튼
    public Button startPanelOffButton; // 시작 패널 닫기 버튼
    public Button loadPanelOnButton;
    public Button loadPanelOffButton; // 로드 패널 닫기 버튼

    public Button lisenceButton;
    public Button closeLisenceButton; // 라이센스 닫기 버튼

    public Button configureButton;  // 설정 패널 열기 버튼
    public Button closeConfigureButton; // 설정 패널 닫기 버튼

    public Image lisenceBackGroundImage;
    public GameObject gameStartPanel; // 시작 패널 오브젝트
    public GameObject gameLoadPanel;
    public GameObject configurePanel; // 설정 패널 오브젝트

    public LoadUI loadUI; // 로드 UI 스크립트

    [SerializeField] private GameObject saveWarringPopup;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;


    [SerializeField] private GameObject startWarringPopup;
    [SerializeField] private Image playerImage;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Button showStartWarringPopupButton;
    [SerializeField] private Button hideStartWarringPopupButton;

    private void Start()
    {
        // 시작 버튼 클릭 이벤트 등록
        startButton.onClick.AddListener(OnStartButtonClicked);
        loadButton.onClick.AddListener(OnLoadButtonClicked);
        startPanelOnButton.onClick.AddListener(ShowWarringPopup);
        startPanelOffButton.onClick.AddListener(CloseStartPanel);
        loadPanelOnButton.onClick.AddListener(OpenLoadPanel);
        loadPanelOffButton.onClick.AddListener(CloseLoadPanel);
        configureButton.onClick.AddListener(ShowConfigureButton);
        closeConfigureButton.onClick.AddListener(HideConfigureButton);
        lisenceButton.onClick.AddListener(OnLisenceButtonClicked);
        closeLisenceButton.onClick.AddListener(CloseLisenceeButtonClicked);

        nextButton.onClick.AddListener(OpenStartPanel);
        prevButton.onClick.AddListener(HideWarringPopup);

        showStartWarringPopupButton.onClick.AddListener(ShowStartWarringPopup);
        hideStartWarringPopupButton.onClick.AddListener(HideStartWarringPopup);

    }

    private void OnStartButtonClicked()
    {
        Debug.Log("게임을 시작합니다.");

        // 저장된 playerData 삭제
        string playerDataPath = Application.persistentDataPath + "/playerData.json";
        if (File.Exists(playerDataPath))
        {
            File.Delete(playerDataPath);
            Debug.Log("기존 저장된 playerData 데이터를 삭제했습니다.");
        }

        // 저장된 rayTrigger 데이터 삭제
        string rayTriggerPath = Application.persistentDataPath + "/ray_trigger_save.json";
        if (File.Exists(rayTriggerPath))
        {
            File.Delete(rayTriggerPath);
            Debug.Log("기존 저장된 ray_trigger 데이터를 삭제했습니다.");
        }

        // 씬 전환
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMapPuzzleTestScene");
    }

    private void OnLoadButtonClicked()
    {
        // 게임 로드 로직
        Debug.Log("게임을 불러옵니다.");
        // 예: 저장된 플레이어 데이터 불러오기, 씬 전환 등
        Player loadedPlayer = PlayerSaveManager.Instance.LoadPlayerData();

        if (loadedPlayer != null)
        {
            PlayerManager.Instance.player = loadedPlayer;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMapPuzzleTestScene");
            Debug.Log("저장된 플레이어 데이터를 불러왔습니다.");
        }
        else
        {
            Debug.Log("저장된 데이터가 없습니다.");
        }
    }

    private void OnLisenceButtonClicked()
    {
        // 라이센스 정보 표시 로직
        lisenceBackGroundImage.gameObject.SetActive(true);
    }

    public void CloseLisenceeButtonClicked()
    {
        // 라이센스 정보 닫기
        lisenceBackGroundImage.gameObject.SetActive(false);
    }

    public void OpenStartPanel()
    {
        // 시작 패널 열기
        HideWarringPopup();
        gameStartPanel.SetActive(true);
    }
    public void CloseStartPanel()
    {
        // 시작 패널 닫기
        gameStartPanel.SetActive(false);
    }

    public void OpenLoadPanel()
    {
        Player loadedPlayer = PlayerSaveManager.Instance.LoadPlayerData();
        if (loadedPlayer == null)
        {
            Debug.Log("저장된 데이터가 없습니다. 로드 패널을 열 수 없습니다.");
            return;
        }
        // 로드 패널 열기
        gameLoadPanel.SetActive(true);
        loadUI.TakeLoadDataUI(); // 로드 UI 갱신
    }
    public void CloseLoadPanel()
    {
        // 로드 패널 닫기
        gameLoadPanel.SetActive(false);
    }

    public void ShowWarringPopup()
    {
        Player loadedPlayer = PlayerSaveManager.Instance.LoadPlayerData();
        if (loadedPlayer == null)
        {
            HideWarringPopup();
            OpenStartPanel();
            return;
        }
        saveWarringPopup.SetActive(true);
    }
    public void HideWarringPopup()
    {
        saveWarringPopup.SetActive(false);
    }

    public void ShowStartWarringPopup()
    {
        playerNameText.text = PlayerManager.Instance.player.playerName;
        playerImage.sprite = PlayerManager.Instance.playerImage[PlayerManager.Instance.player.playerGender];
        startWarringPopup.SetActive(true);
    }

    public void HideStartWarringPopup()
    {
        startWarringPopup.SetActive(false);
    }

    public void ShowConfigureButton()
    {
        configurePanel.SetActive(true);
    }

    public void HideConfigureButton()
    {
        configurePanel.SetActive(false);
    }
}


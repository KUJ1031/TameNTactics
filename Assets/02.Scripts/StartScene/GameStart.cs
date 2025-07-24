using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Image lisenceBackGroundImage;
    public GameObject gameStartPanel; // 시작 패널 오브젝트
    public GameObject gameLoadPanel;

    public LoadUI loadUI; // 로드 UI 스크립트

    private void Start()
    {
        // 시작 버튼 클릭 이벤트 등록
        startButton.onClick.AddListener(OnStartButtonClicked);
        loadButton.onClick.AddListener(OnLoadButtonClicked);
        startPanelOnButton.onClick.AddListener(OpenStartPanel);
        startPanelOffButton.onClick.AddListener(CloseStartPanel);
        loadPanelOnButton.onClick.AddListener(OpenLoadPanel);
        loadPanelOffButton.onClick.AddListener(CloseLoadPanel);
        lisenceButton.onClick.AddListener(OnLisenceButtonClicked);
        closeLisenceButton.onClick.AddListener(CloseLisenceeButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        // 게임 시작 로직
        Debug.Log("게임을 시작합니다.");
        // 저장한 데이터 전부 삭제
        string path = Application.persistentDataPath + "/playerData.json";
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
            Debug.Log("기존 저장된 데이터를 삭제했습니다.");
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMapScene");
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
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMapScene");
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
}


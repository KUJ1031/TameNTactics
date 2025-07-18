using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    public Button startButton; // 시작 버튼
    public Button LoadButton;
    public Button LisenceButton;
    public Button CloseLisenceButton; // 라이센스 닫기 버튼

    public Image LisenceBackGroundImage;

    private void Start()
    {
        // 시작 버튼 클릭 이벤트 등록
        startButton.onClick.AddListener(OnStartButtonClicked);
        LoadButton.onClick.AddListener(OnLoadButtonClicked);
        LisenceButton.onClick.AddListener(OnLisenceButtonClicked);
        CloseLisenceButton.onClick.AddListener(CloseLisenceeButtonClicked);
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
        LisenceBackGroundImage.gameObject.SetActive(true);
    }

    public void CloseLisenceeButtonClicked()
    {
        // 라이센스 정보 닫기
        LisenceBackGroundImage.gameObject.SetActive(false);
    }
}


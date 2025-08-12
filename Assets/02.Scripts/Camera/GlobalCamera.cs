using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalCamera : Singleton<GlobalCamera>
{
    public CinemachineVirtualCamera virtualCamera;

    protected override bool IsDontDestroy => true;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // BattleScene에서만 꺼라
        if (scene.name == "BattleScene")
        {
            virtualCamera?.gameObject.SetActive(false);
        }
        else
        {
            virtualCamera?.gameObject.SetActive(true);
        }
    }

    public void SetFollow(Transform target)
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = target;
            virtualCamera.LookAt = target;
        }
        else
        {
            Debug.LogWarning("GlobalCamera: VirtualCamera가 설정되지 않았습니다.");
        }
    }
}

using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalCamera : Singleton<GlobalCamera>
{
    private static GlobalCamera instance;

    public CinemachineVirtualCamera virtualCamera; // 인스펙터에서 할당

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
        // 특정 씬에서만 카메라 비활성화
        string[] scenesToDisableCamera = { "BattleScene", "MinigameScene" };

        if (System.Array.Exists(scenesToDisableCamera, name => name == scene.name))
        {
            if (virtualCamera != null)
            {
                virtualCamera.gameObject.SetActive(false);
                Debug.Log($"GlobalCamera: {scene.name}에서 VirtualCamera 비활성화");
            }
        }
        else
        {
            if (virtualCamera != null)
            {
                virtualCamera.gameObject.SetActive(true);
                Debug.Log($"GlobalCamera: {scene.name}에서 VirtualCamera 활성화");
            }
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

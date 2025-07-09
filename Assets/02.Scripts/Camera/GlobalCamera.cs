using Cinemachine;
using UnityEngine;

public class GlobalCamera : MonoBehaviour
{
    private static GlobalCamera instance;
    public static GlobalCamera Instance => instance;

    public CinemachineVirtualCamera virtualCamera; // 인스펙터에서 할당

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 중복 제거
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

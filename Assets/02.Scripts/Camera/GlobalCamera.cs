using Cinemachine;
using UnityEngine;

public class GlobalCamera : Singleton<GlobalCamera>
{
    private static GlobalCamera instance;

    public CinemachineVirtualCamera virtualCamera; // 인스펙터에서 할당

    protected override bool IsDontDestroy => true;

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

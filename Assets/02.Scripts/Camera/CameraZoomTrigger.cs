using UnityEngine;
using Cinemachine;

public class CameraZoomTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float zoomedOutSize = 8f;
    public float zoomSpeed = 2f;

    private float originalSize;
    private Coroutine zoomCoroutine;

    private void OnEnable()
    {
        // ✅ 자동으로 가상 카메라 할당 (없을 경우에만)
        if (virtualCamera == null)
        {
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        }

        if (virtualCamera != null)
            originalSize = virtualCamera.m_Lens.OrthographicSize;
        else
            Debug.LogWarning("Cinemachine Virtual Camera를 찾지 못했습니다.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartZoom(zoomedOutSize);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartZoom(originalSize);
        }
    }

    private void StartZoom(float targetSize)
    {
        if (virtualCamera == null || !gameObject.activeInHierarchy)
            return;

        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(ZoomTo(targetSize));
    }

    private System.Collections.IEnumerator ZoomTo(float targetSize)
    {
        while (!Mathf.Approximately(virtualCamera.m_Lens.OrthographicSize, targetSize))
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
                virtualCamera.m_Lens.OrthographicSize,
                targetSize,
                Time.deltaTime * zoomSpeed
            );
            yield return null;
        }

        virtualCamera.m_Lens.OrthographicSize = targetSize;
    }
}

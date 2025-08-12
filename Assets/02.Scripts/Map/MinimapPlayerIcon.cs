using UnityEngine;

public class MinimapPlayerIcon : MonoBehaviour
{
    [Header("대상 플레이어")]
    private Transform player;

    [Header("미니맵 카메라")]
    public Camera minimapCamera;

    [Header("미니맵 UI")]
    public RectTransform minimapRect;
    public RectTransform playerIconRect;

    private void Update()
    {
        player = PlayerManager.Instance?.playerController.transform;
        if (player == null || minimapCamera == null) return;

        Vector3 viewportPos = minimapCamera.WorldToViewportPoint(player.position);
        float x = (viewportPos.x - 0.5f) * minimapRect.rect.width;
        float y = (viewportPos.y - 0.5f) * minimapRect.rect.height;

        playerIconRect.anchoredPosition = new Vector2(x, y);
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapZoomHandler : MonoBehaviour, IScrollHandler
{
    public RectTransform mapImage; // MapImage RectTransform
    public ScrollRect scrollRect;
    public float zoomSpeed = 0.1f;
    public float minZoom = 1f;
    public float maxZoom = 3f;

    private float currentZoom = 1f;

    public void OnScroll(PointerEventData eventData)
    {
        float scrollDelta = eventData.scrollDelta.y;
        currentZoom = Mathf.Clamp(currentZoom + scrollDelta * zoomSpeed, minZoom, maxZoom);
        mapImage.localScale = Vector3.one * currentZoom;

        ClampMapPosition();
    }

    private void ClampMapPosition()
    {
        // 맵이 Viewport 밖으로 안 나가게 위치 보정
        var viewportRect = scrollRect.viewport.rect;
        var contentRect = mapImage.rect;
        var scaledSize = contentRect.size * currentZoom;

        Vector2 pos = mapImage.anchoredPosition;

        float xLimit = Mathf.Max(0, (scaledSize.x - viewportRect.width) / 2);
        float yLimit = Mathf.Max(0, (scaledSize.y - viewportRect.height) / 2);

        pos.x = Mathf.Clamp(pos.x, -xLimit, xLimit);
        pos.y = Mathf.Clamp(pos.y, -yLimit, yLimit);

        mapImage.anchoredPosition = pos;
    }
}

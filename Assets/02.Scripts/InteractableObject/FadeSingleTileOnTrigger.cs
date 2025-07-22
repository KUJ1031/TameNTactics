using UnityEngine;
using UnityEngine.Tilemaps;

public class FadeSingleTileOnTrigger : MonoBehaviour
{
    public float fadeAlpha = 0.3f;
    public float fadeDuration = 0.5f;

    private Tilemap tilemap;
    private Vector3Int tilePosition;
    private Color originalColor;
    private bool isFading = false;

    private void Start()
    {
        tilemap = GetComponentInParent<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogWarning("Tilemap not found in parent.");
            enabled = false;
            return;
        }

        // 타일 위치 계산 (Collider의 중심 기준)
        Vector3 worldPos = transform.position;
        tilePosition = tilemap.WorldToCell(worldPos);
        originalColor = tilemap.GetColor(tilePosition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFading)
            StartCoroutine(FadeTileAlpha(fadeAlpha));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFading)
            StartCoroutine(FadeTileAlpha(originalColor.a));
    }

    private System.Collections.IEnumerator FadeTileAlpha(float targetAlpha)
    {
        isFading = true;
        float startAlpha = tilemap.GetColor(tilePosition).a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            Color c = originalColor;
            c.a = newAlpha;
            tilemap.SetColor(tilePosition, c);
            yield return null;
        }

        Color finalColor = originalColor;
        finalColor.a = targetAlpha;
        tilemap.SetColor(tilePosition, finalColor);
        isFading = false;
    }
}

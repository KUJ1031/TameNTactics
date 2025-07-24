using UnityEngine;

public class PlayerZoneDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        MapZone zone = other.GetComponent<MapZone>();
        if (zone != null && MapNameDisplayManager.Instance != null)
        {
            MapNameDisplayManager.Instance.ShowMapName(zone.zoneName);
            PlayerManager.Instance.player.playerLastStage = zone.zoneName; // 플레이어의 마지막 위치 저장
        }
    }
}

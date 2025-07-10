using UnityEngine;

public class PlayerZoneDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        MapZone zone = other.GetComponent<MapZone>();
        if (zone != null && MapNameDisplayManager.Instance != null)
        {
            MapNameDisplayManager.Instance.ShowMapName(zone.zoneName);
        }
    }
}

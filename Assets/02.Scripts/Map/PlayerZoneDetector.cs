using UnityEngine;

public class PlayerZoneDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        MapZone zone = other.GetComponent<MapZone>();
        if (zone != null && MapNameDisplayManager.Instance != null)
        {
            MapNameDisplayManager.Instance.ShowMapName(zone.zoneName);

            string nextBGMName = "";
            switch (zone.zoneName)
            {
                case "시작의 땅":
                case "초보 사냥터":
                case "위험한 쉼터":
                case "잊혀진 공간":
                    nextBGMName = "Forest";
                    break;
                case "불길한 다리":
                case "파멸의 성 입구":
                case "파멸의 성":
                case "결전의 장소":
                    nextBGMName = "Castle";
                    break;
                case "한적한 마을":
                    nextBGMName = "Village";
                    break;
            }
            AudioManager.Instance.CrossFadeBGM(nextBGMName, 0.5f);
            PlayerManager.Instance.player.playerLastStage = zone.zoneName; // 플레이어의 마지막 위치 저장
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HealingZone : MonoBehaviour
{
    [SerializeField] private Transform healingZoneTransform;
    // Start is called before the first frame update
    void Start()
    {
        var battleEntry = PlayerManager.Instance.player.battleEntry;

        if (battleEntry.Count > 0 && battleEntry.All(monster => monster.CurHp <= 0))
        {
            Debug.Log("모든 몬스터의 체력이 0입니다.");
            PlayerManager.Instance.playerController.transform.position = healingZoneTransform.position;
        }

    }
}

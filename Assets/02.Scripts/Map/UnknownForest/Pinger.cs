using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinger : MonoBehaviour
{
    private void Start()
    {
        if (PlayerManager.Instance.player.playerQuestClearCheck[0])
        {
            Destroy(gameObject);
        }
    }
}

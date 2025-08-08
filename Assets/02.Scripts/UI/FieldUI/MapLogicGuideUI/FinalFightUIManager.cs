using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFightUIManager : Singleton<FinalFightUIManager>
{
    [SerializeField] private GameObject EliteUI;
    [SerializeField] private GameObject BossUI;


    private void Start()
    {
        if (PlayerManager.Instance.player.playerEliteStartCheck[0] && !PlayerManager.Instance.player.playerEliteClearCheck[0])
        {
            Debug.Log("FinalFightManager: 딘과의 전투 시작");
            ShowEliteUI();
        }
        else if (PlayerManager.Instance.player.playerBossStartCheck[0])
        {
            Debug.Log("FinalFightManager: 보스와의 전투 시작");
            ShowBossUI();
        }

    }
    public void ShowEliteUI()
    {
        if (EliteUI != null)
        {
            EliteUI.gameObject.SetActive(true);
            Debug.Log("Elite UI is shown.");
        }
    }
    public void ShowBossUI()
    {
        if (BossUI != null)
        {
            BossUI.gameObject.SetActive(true);
            Debug.Log("Boss UI is shown.");
        }
    }

}

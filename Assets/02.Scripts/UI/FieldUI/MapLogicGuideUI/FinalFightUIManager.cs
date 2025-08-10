using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFightUIManager : Singleton<FinalFightUIManager>
{
    [SerializeField] private GameObject EliteUI;
    [SerializeField] private GameObject BossUI;


    private void Start()
    {
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

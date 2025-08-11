using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFightUIManager : Singleton<FinalFightUIManager>
{
    [SerializeField] private GameObject eliteUI;
    [SerializeField] private GameObject bossUI;
    [SerializeField] private GameObject bossmapUI;


    private void Start()
    {
    }
    public void ShowEliteUI()
    {
        if (eliteUI != null)
        {
            eliteUI.gameObject.SetActive(true);
            Debug.Log("Elite UI is shown.");
        }
    }
    public void ShowBossUI()
    {
        if (bossUI != null)
        {
            bossUI.gameObject.SetActive(true);
            Debug.Log("Boss UI is shown.");
        }
    }

    public void ShowBossMapUI()
    {
        if (bossmapUI != null)
        {
            bossmapUI.gameObject.SetActive(true);
            Debug.Log("Boss Map UI is shown.");
        }
    }

    public void HideEliteUI()
    {
        if (eliteUI != null)
        {
            eliteUI.gameObject.SetActive(false);
            Debug.Log("Elite UI is hidden.");
        }
    }

}

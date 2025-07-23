using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    protected override bool IsDontDestroy => false;

    [SerializeField] private GameObject shopUIObject;
    [SerializeField] private GameObject shopUI_SellObject;

    public void OpenShopUI()
    {
        if (shopUIObject == null)
        {
            Debug.LogError("Shop UI Object is not assigned in the ShopManager.");
            return;
        }
        shopUIObject.SetActive(true);
    }
    public void OpenShopUI_Sell()
    {
        if (shopUI_SellObject == null)
        {
            Debug.LogError("Shop UI Sell Object is not assigned in the ShopManager.");
            return;
        }
        shopUI_SellObject.SetActive(true);
    }


    public void CloseShopUI()
    {
        if (shopUIObject == null)
        {
            Debug.LogError("Shop UI Object is not assigned in the ShopManager.");
            return;
        }
        shopUIObject.SetActive(false);
    }

}
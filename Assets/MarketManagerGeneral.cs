using Controller;
using Sdkbox;
using System.Collections;
using UnityEngine;

public class MarketManagerGeneral : MonoBehaviour
{
    public void onInitialized(bool status)
    {
    }

    public void onSuccess(Product product)
    {
        switch (product.name)
        {
            case "full_life":
                MarketController.Instance.AddEnergy(MarketController.Instance.MaxEnergy - MarketController.Instance.PMone.Energy);
                break;

            case "100_money":
                MarketController.Instance.AddMoney(100);
                break;

            case "50_money":
                MarketController.Instance.AddMoney(50);
                break;

            case "5_leves":
                MarketController.Instance.AddEnergy(5);
                break;
        }
    }

    public void onFailure(Product product, string message)
    {
    }

    public void onCanceled(Product product)
    {
        AndroidNativeFunctions.ShowAlert("Bad operation!", "Error", "Ok", "Cancel", "Ok", new UnityEngine.Events.UnityAction<DialogInterface>((DialogInterface d) => { Debug.Log(d.ToString()); }));
    }

    public void onRestored(Product product)
    {
    }

    public void onProductRequestSuccess(Product[] products)
    {
        foreach (var p in products)
        {
        }
    }

    public void onProductRequestFailure(string message)
    {
    }

    public void onRestoreComplete(string message)
    {
    }
}
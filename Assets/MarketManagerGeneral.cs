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

            case "100_batteries":
                MarketController.Instance.AddMoney(100);
                break;

            case "50_batteries":
                MarketController.Instance.AddMoney(50);
                break;

            case "20_batteries":
                MarketController.Instance.AddMoney(20);
                break;

            case "250_batteries":
                MarketController.Instance.AddMoney(250);
                break;

            case "no_ads":
                MarketController.Instance.PMone.ShowAD = false;
                break;
        }

        AchController.Instance.ShowAch("Boost!", 50);
    }

    public void onFailure(Product product, string message)
    {
    }

    public void onCanceled(Product product)
    {
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
using Controller;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class MainMarketManager : MonoBehaviour
    {
        public void RestoreFullEnergy()
        {
            MarketController.Instance.AddEnergy(MarketController.Instance.MaxEnergy);
        }

        public void Get1000Bat()
        {
            MarketController.Instance.AddMoney(1000);
        }
    }
}
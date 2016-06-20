using Controller;
using Sdkbox;
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

        // Release;

        private IAP _iap;

        public void Start()
        {
            _iap = FindObjectOfType<IAP>();
        }

        private bool CheckInternet()
        {
            if (Application.isMobilePlatform)
            {
                if (!AndroidNativeFunctions.isConnectInternet())
                {
                    AndroidNativeFunctions.ShowToast("No internet connection");
                    return false;
                }
                return true;
            }
            return false;
        }

        public void Buy10B()
        {
            if (_iap && CheckInternet())
            {
                _iap.purchase("10_batteries");
            }
        }

        public void Buy50B()
        {
            if (_iap && CheckInternet())
            {
                _iap.purchase("50_batteries");
            }
        }

        public void Buy100B()
        {
            if (_iap && CheckInternet())
            {
                _iap.purchase("100_batteries");
            }
        }

        public void Buy250B()
        {
            if (_iap && CheckInternet())
            {
                _iap.purchase("250_batteries");
            }
        }

        public void BuyNoAd()
        {
            if (_iap && MarketController.Instance.PMone.ShowAD && CheckInternet())
            {
                _iap.purchase("no_ads");
            }
        }

        public void BuyFullLifeB()
        {
            if (_iap && MarketController.Instance.MaxEnergy != MarketController.Instance.PMone.Energy && CheckInternet())
            {
                _iap.purchase("full_energy");
            }
        }

        public void BuyLevelPack()
        {
            if (_iap && !LevelController.Instance.IsAllLP() && CheckInternet())
            {
                _iap.purchase("level_pack");
            }

            //Debug
            /*
            else
            {
                LevelController.Instance.OpenNexLevelPack();
            }
            */
        }
    }
}
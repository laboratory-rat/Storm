using admob;
using Controller;
using System.Collections;
using UnityEngine;

namespace Game.UI
{
    public class NoEnergyManager : MonoBehaviour
    {
        public GameObject StopUI;
        public GameObject Continue;

        private void Start()
        {
            StopUI.SetActive(false);
            MarketController.Instance.OnEnergyChanged += OnEnergy;
        }

        public void StopGame()
        {
            StopUI.SetActive(true);
            GameController.Instance.PauseGame(0f);
            Continue.SetActive(false);

            Admob.Instance().initAdmob("ca-app-pub-9869209397937230/7682387909", "ca-app-pub-9869209397937230/7682387909");//admob id with format ca-app-pub-279xxxxxxxx/xxxxxxxx
            Admob.Instance().showBannerRelative(AdSize.IABBanner, AdPosition.BOTTOM_CENTER, 0);
            Admob.Instance().bannerEventHandler += NoEnergyManager_bannerEventHandler;
            rev = true;
        }

        public void Unpause()
        {
            StopUI.SetActive(false);
            GameController.Instance.PauseGame(1f);

            if (rev)
            {
                rev = false;
                Admob.Instance().bannerEventHandler -= NoEnergyManager_bannerEventHandler;
                Admob.Instance().removeBanner();
            }
        }

        public void OnEnergy()
        {
            if (MarketController.Instance.PMone.Energy < 1 && !StopUI.activeInHierarchy)
            {
                StopGame();
            }
            else if (MarketController.Instance.PMone.Energy > 0)
            {
                Continue.SetActive(true);
            }
        }

        public void Buy5()
        {
            if (MarketController.Instance.Byu10ForBattery())
                Unpause();
            else
            {
                AndroidNativeFunctions.ShowToast("Мало батареек");
            }
        }

        public void BuyFull()
        {
            if (MarketController.Instance.ByuFullForBattery())
                Unpause();
            else
            {
                AndroidNativeFunctions.ShowToast("Мало батареек");
            }
        }

        private bool rev = false;

        private void NoEnergyManager_bannerEventHandler(string eventName, string msg)
        {
            if (eventName == AdmobEvent.onAdOpened)
            {
                MarketController.Instance.AddEnergy(2);
            }

            rev = false;
            Admob.Instance().bannerEventHandler -= NoEnergyManager_bannerEventHandler;
            Admob.Instance().removeBanner();
        }

        public void Menu()
        {
            SceneController.Instance.ChangeScene("Menu", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        private void OnDestroy()
        {
            if (rev)
                Admob.Instance().bannerEventHandler -= NoEnergyManager_bannerEventHandler;

            MarketController.Instance.OnEnergyChanged -= OnEnergy;
        }
    }
}
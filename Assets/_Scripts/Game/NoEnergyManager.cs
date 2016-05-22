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
        }

        public void Unpause()
        {
            StopUI.SetActive(false);
            GameController.Instance.PauseGame(1f);
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
            if (MarketController.Instance.Byu5ForBattery())
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

        public void AddTwo()
        {
            MarketController.Instance.ShowRewardMessage();
        }

        public void Menu()
        {
            SceneController.Instance.ChangeScene("Menu", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        private void OnDestroy()
        {
            MarketController.Instance.OnEnergyChanged -= OnEnergy;
        }
    }
}
﻿using admob;
using Controller;
using UnityEngine;

namespace Game.UI
{
    public class PauseMenuManager : MonoBehaviour
    {
        public Animator Animator;
        public GameObject MenuObject;

        public GameObject SoundOnGO;
        public GameObject SoundOffGO;

        public void Start()
        {
            var i = ConfigController.Instance.Config.Backgroud;
            if (i == "1")
            {
                SoundOnGO.SetActive(true);
                SoundOffGO.SetActive(false);
            }
            else
            {
                SoundOnGO.SetActive(false);
                SoundOffGO.SetActive(true);
            }
        }

        public void Show()
        {
            MenuObject.SetActive(true);
            Animator.SetTrigger("Show");
            GameController.Instance.PauseGame(0f);

            if (MarketController.Instance.PMone.ShowAD && Application.isMobilePlatform)
            {
                Admob.Instance().initAdmob("ca-app-pub-9869209397937230/7043369900", "ca-app-pub-9869209397937230/7043369900");//admob id with format ca-app-pub-279xxxxxxxx/xxxxxxxx
                Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.MIDDLE_RIGHT, 0);
                SceneController.Instance.OnSceneChanged += CloseAd;
            }
        }

        private void CloseAd()
        {
            Admob.Instance().removeBanner();
        }

        public void Hide()
        {
            Animator.SetTrigger("Hide");
            GameController.Instance.PauseGame(1f);
        }

        public void Restart()
        {
            GameController.Instance.RestartLevel();
        }

        public void Info()
        {
        }

        public void SoundOn()
        {
            SoundOnGO.SetActive(true);
            SoundOffGO.SetActive(false);

            ConfigController.Instance.Config.Backgroud = "1";
            ConfigController.Instance.SaveConfig();
        }

        public void SoundOff()
        {
            SoundOnGO.SetActive(false);
            SoundOffGO.SetActive(true);

            ConfigController.Instance.Config.Backgroud = "0";
            ConfigController.Instance.SaveConfig();
        }

        public void Market()
        {
        }

        public void Menu()
        {
            SceneController.Instance.ChangeScene(SceneController.MENU, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}
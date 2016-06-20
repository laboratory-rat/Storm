using admob;
using Controller;
using System.Collections;
using UnityEngine;

namespace Game.UI
{
    public class PauseMenuManager : MonoBehaviour
    {
        public Animator Animator;
        public GameObject MenuObject;

        public GameObject SoundOnGO;
        public GameObject SoundOffGO;

        private bool _adIsOn = false;

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

            StartCoroutine(CloseRevard());
        }

        public IEnumerator CloseRevard()
        {
            while (true)
            {
                if (!_adIsOn)
                {
                    try
                    {
                        Admob.Instance().removeBanner();
                    }
                    catch (System.Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                }

                yield return new WaitForSeconds(2f);
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
                Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.TOP_CENTER, 0);
                _adIsOn = true;
            }
        }

        public void Hide()
        {
            if (_adIsOn)
            {
                Admob.Instance().removeBanner();
                _adIsOn = false;
            }

            Animator.SetTrigger("Hide");
            GameController.Instance.PauseGame(1f);
        }

        public void Restart()
        {
            GameController.Instance.RestartLevel();

            if (_adIsOn)
            {
                Admob.Instance().removeBanner();
                _adIsOn = false;
            }

            StopAllCoroutines();
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
            if (_adIsOn)
            {
                Admob.Instance().removeBanner();
                _adIsOn = false;
            }

            try
            {
                Admob.Instance().removeBanner();
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }

            StopAllCoroutines();

            SceneController.Instance.ChangeScene(SceneController.MENU, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}
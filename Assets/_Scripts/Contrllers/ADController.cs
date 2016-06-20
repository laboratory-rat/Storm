using admob;
using System.Collections;
using UnityEngine;

namespace Controller
{
    public class ADController : MonoBehaviour
    {
        private static ADController _instance;

        public static ADController Instance
        {
            get { return _instance; }
        }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else if (_instance != this)
                Destroy(this);
        }

        private void Start()
        {
            if (!Application.isMobilePlatform)
                return;

            SceneController.Instance.OnSceneChanged += CloseAD;
            OpenAD();
        }

        private void OnSceneWasLoaded()
        {

            try
            {
                Admob.Instance().removeBanner();
            }
            catch(System.Exception e)
            {
                Debug.Log(e.Message);
            }

            OpenAD();
        }

        private bool _ad = false;

        private void OpenAD()
        {
            if (_ad)
                return;

            _ad = true;

            if (SceneController.Instance.GetSceneName() == "Menu")
                ShowMenuAd();
            else if (SceneController.Instance.GetSceneName() == "Loading")
                ShowLoadingAd();
            else
                _ad = false;
        }

        private void CloseAD()
        {
            if (!_ad)
                return;

            Admob.Instance().removeBanner();
            _ad = false;
        }

        public void ShowMenuAd()
        {
            try
            {
                Admob.Instance().removeBanner();
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }

            Admob.Instance().initAdmob("ca-app-pub-9869209397937230/5747570306", "ca-app-pub-9869209397937230/5747570306");//admob id with format ca-app-pub-279xxxxxxxx/xxxxxxxx
            Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.TOP_CENTER, 0);
        }

        public void ShowLoadingAd()
        {
            try
            {
                Admob.Instance().removeBanner();
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }

            //Admob.Instance().initAdmob("ca-app-pub-9869209397937230/9227793503", "ca-app-pub-9869209397937230/9227793503");//admob id with format ca-app-pub-279xxxxxxxx/xxxxxxxx
            //Admob.Instance().showBannerRelative(AdSize.Leaderboard, AdPosition.TOP_CENTER, 0);
        }
    }
}
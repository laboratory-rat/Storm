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

            SceneController.Instance.OnSceneChanged += Start;
        }

        private void Start()
        {
            if (!Application.isMobilePlatform)
                return;

            if (SceneController.Instance.GetSceneName() == "Menu")
            {
                ShowMenuAd();
                SceneController.Instance.OnSceneChanged += CloseAD;
            }
            else if (SceneController.Instance.GetSceneName() == "Loading")
            {
                ShowLoadingAd();
                SceneController.Instance.OnSceneChanged += CloseAD;
            }
        }

        private void CloseAD()
        {
            Admob.Instance().removeBanner();
            SceneController.Instance.OnSceneChanged -= CloseAD;
        }

        public void ShowMenuAd()
        {
            Admob.Instance().initAdmob("ca-app-pub-9869209397937230/5747570306", "ca-app-pub-9869209397937230/5747570306");//admob id with format ca-app-pub-279xxxxxxxx/xxxxxxxx
            Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.BOTTOM_CENTER, 0);
        }

        public void ShowLoadingAd()
        {
            Admob.Instance().initAdmob("ca-app-pub-9869209397937230/9227793503", "ca-app-pub-9869209397937230/9227793503");//admob id with format ca-app-pub-279xxxxxxxx/xxxxxxxx
            Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.MIDDLE_RIGHT, 0);
        }
    }
}
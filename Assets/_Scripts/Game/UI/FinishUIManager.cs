using admob;
using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class FinishUIManager : MonoBehaviour
    {
        public GameObject FinishObject;
        public Text TimeText;
        public Animator Animator;
        public Image FFlash;
        public Image SFlash;
        public Image TFlash;

        public bool GoToMenu = false;
        public bool LastLevelInWorld = false;
        public string NextWorld = "";
        public string NextLevel;

        private string _world;
        private Level _level;

        public void Start()
        {
            GameController.Instance.OnLevelFinished += FinishLevel;
            FinishObject.SetActive(false);
        }

        public void OnDestroy()
        {
            GameController.Instance.OnLevelFinished -= FinishLevel;
        }

        public void FinishLevel()
        {
            if (MarketController.Instance.PMone.ShowAD && Application.isMobilePlatform)
            {
                Admob.Instance().initAdmob("ca-app-pub-9869209397937230/5747570306", "ca-app-pub-9869209397937230/5747570306");//admob id with format ca-app-pub-279xxxxxxxx/xxxxxxxx
                Admob.Instance().showBannerRelative(AdSize.SmartBanner, AdPosition.TOP_CENTER, 0);
                SceneController.Instance.OnSceneChanged += CloseAd;
            }

            FinishObject.SetActive(true);

            GameController.Instance.PauseGame(0f);

            _world = LevelController.Instance.CurrentWorld;
            _level = LevelController.Instance.GetLevel(_world, LevelController.Instance.CurrentLevel.Name);

            int time = FindObjectOfType<TimerUI>().IntTime;

            if (_level.BestTime > time || _level.BestTime == -1)
                _level.BestTime = time;

            TimeText.text = time.ToString();

            int t = _level.Times[0];
            int s = _level.Times[1];

            if (time <= t)
            {
                _level.Flash = FlashRate.Three;
                Animator.SetBool("Third", true);
                Animator.SetBool("Second", true);

                LevelController.Instance.UpLevelRate(_world, _level.Name, FlashRate.Three);
            }
            else if (time <= s)
            {
                _level.Flash = FlashRate.Two;
                Animator.SetBool("Second", true);

                LevelController.Instance.UpLevelRate(_world, _level.Name, FlashRate.Two);
            }
            else
            {
                LevelController.Instance.UpLevelRate(_world, _level.Name, FlashRate.One);
            }

            if (NextLevel != "")
            {
                var w = LastLevelInWorld ? NextWorld : LevelController.Instance.CurrentWorld;
                LevelController.Instance.OpenNew(w, NextLevel);
            }
        }

        public void Next()
        {
            if (GoToMenu)
            {
                AndroidNativeFunctions.ShowToast(LocalController.Instance.L("game_menu", "last_level"));
                return;
            }

            if (LastLevelInWorld)
                _world = NextWorld;

            var l = LevelController.Instance.GetLevelByScene(_world, NextLevel);
            if (l != null)
            {
                LevelController.Instance.CurrentLevel = l;
                LevelController.Instance.CurrentWorld = _world;
                SceneController.Instance.ChangeScene(l.LevelName, UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }

        public void Restart()
        {
            GameController.Instance.RestartLevel();
        }

        public void Menu()
        {
            LevelController.Instance.CurrentLevel = null;
            LevelController.Instance.CurrentWorld = "";
            SceneController.Instance.ChangeScene(SceneController.MENU, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        private void CloseAd()
        {
            Admob.Instance().removeBanner();
            SceneController.Instance.OnSceneChanged -= CloseAd;
        }
    }
}
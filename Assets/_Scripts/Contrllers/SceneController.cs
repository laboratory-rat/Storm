using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controller
{
    public class SceneController : MonoBehaviour
    {
        #region Instance

        private static SceneController _instance = null;

        public static SceneController Instance
        {
            get
            {
                return _instance;
            }
        }

        private void Init()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(this);
            }
        }

        #endregion Instance

        public const string LOADING = "Loading";
        public const string MENU = "Menu";

        public delegate void SceneChanged();

        public event SceneChanged OnSceneChanged;

        private string sl = "";

        private void Awake()
        {
            Init();
        }

        public void ChangeScene(string scene, LoadSceneMode mode, bool LoadScene = true)
        {
            if (scene != SceneManager.GetActiveScene().name)
            {
                if (OnSceneChanged != null)
                    OnSceneChanged.Invoke();

                try
                {
                    if (LoadScene)
                    {
                        SceneManager.LoadScene(LOADING, LoadSceneMode.Single);
                        sl = scene;
                    }
                    else
                        SceneManager.LoadScene(scene, mode);
                }
                catch (Exception e)
                {
                    ErrorController.Instance.Send(this, e.Message);
                }
            }
        }

        public string GetSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }

        private void OnLevelWasLoaded()
        {
            try
            {
                if (sl != "" && SceneManager.GetActiveScene().name == LOADING)
                {
                    SceneManager.LoadScene(sl);
                    sl = "";
                }
            }
            catch (Exception e)
            {
                ErrorController.Instance.Send(this, e.Message);
            }
        }
    }
}
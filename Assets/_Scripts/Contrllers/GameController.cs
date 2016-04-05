using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class GameController : MonoBehaviour
    {
        #region Instance

        private static GameController _instance = null;

        public static GameController Instance
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

        #region Events

        public delegate void SimpleVoid();

        public event SimpleVoid OnPlayerDestroy;

        public event SimpleVoid OnLevelStart;

        public event SimpleVoid OnLevelRestart;

        public event SimpleVoid OnLevelFinished;

        public event SimpleVoid OnTimescaleChanged;

        public event SimpleVoid OnCheckBox;

        public event SimpleVoid OnPlayerRotation;

        #endregion Events

        private readonly List<string> _nonGameLevels = new List<string> { "Menu", "Loading" };
        private GameObject FinishUI;

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
        }

        public void PauseGame()
        {
            var c = Time.timeScale > 0 ? true : false;

            if (OnTimescaleChanged != null)
                OnTimescaleChanged.Invoke();

            if (c)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }

        #region Player

        public void PlayerDestroy()
        {
            if (OnPlayerDestroy != null)
                OnPlayerDestroy.Invoke();
        }

        public void PlayerRotate()
        {
            if (OnPlayerRotation != null)
                OnPlayerRotation.Invoke();
        }

        public void CheckBoxTrigger()
        {
            if (OnCheckBox != null)
                OnCheckBox.Invoke();
        }

        public void FinishLevel()
        {
            if (OnLevelFinished != null)
                OnLevelFinished.Invoke();

            FinishUI.SetActive(true);
            PauseGame();
        }

        #endregion Player

        private void OnLevelWasLoaded(int i)
        {
            Time.timeScale = 1;
            FinishUI = GameObject.FindGameObjectWithTag("UIFinish");
            if (FinishUI)
                FinishUI.SetActive(false);
        }
    }
}
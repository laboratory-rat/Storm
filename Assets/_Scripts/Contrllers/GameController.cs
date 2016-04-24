using Game;
using Game.UI;
using GameUI;
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

        public const string PLAYER_ASSET = "Prefabs/Game/Player/Player";

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

        public void RestartLevel()
        {
            if (MarketController.Instance.MinusEnergy())
            {
                SceneController.Instance.ReloadLevel(true);
            }
            else
                AndroidNativeFunctions.ShowToast("Energy");
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
        }

        #endregion Player

        private void OnLevelWasLoaded(int i)
        {
            Time.timeScale = 1;

            if (!_nonGameLevels.Contains(SceneController.Instance.GetSceneName()))
            {
                var player = FindObjectOfType<PlayerController>();
                if (!player)
                {
                    GameObject go = Instantiate(Resources.Load<GameObject>(PLAYER_ASSET));
                    player = go.GetComponentInChildren<PlayerController>();
                }

                FindObjectOfType<PlayerUIManager>().Init(player);
            }
        }
    }
}
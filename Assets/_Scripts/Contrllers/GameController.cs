using Game;
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

        public delegate void RotateVoid(GravityVector gv);

        public delegate void PauseVoid(float scale);

        public delegate void GameObjectVoid(GameObject go);

        public event SimpleVoid OnPlayerDestroy;

        public event SimpleVoid OnLevelStart;

        public event SimpleVoid OnLevelRestart;

        public event SimpleVoid OnLevelFinished;

        public event PauseVoid OnTimescaleChanged;

        public event GameObjectVoid OnCheckBox;

        public event RotateVoid OnPlayerRotation;

        public event SimpleVoid OnPlayerAlive;

        public event SimpleVoid OnNewVerion;

        public event SimpleVoid OnFirstEnter;

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

        #region System

        public void FirstEnter()
        {
            if (OnFirstEnter != null)
                OnFirstEnter.Invoke();

            //MarketController.Instance.AddMoney(200);
        }

        public void NewVersion()
        {
            if (OnNewVerion != null)
                OnNewVerion.Invoke();

            //MarketController.Instance.PMone.Money = 199;
            //MarketController.Instance.AddEnergy(1);
        }

        #endregion System

        #region Level

        public void PauseGame(float scale)
        {
            Time.timeScale = scale;

            if (OnTimescaleChanged != null)
                OnTimescaleChanged.Invoke(scale);
        }

        public void RestartLevel()
        {
            if (OnLevelRestart != null)
                OnLevelRestart.Invoke();

            SceneController.Instance.ReloadLevel(true);
        }

        #endregion Level

        #region Player

        public void PlayerDestroy()
        {
            //MarketController.Instance.MinusEnergy(1);
            if (OnPlayerDestroy != null)
                OnPlayerDestroy.Invoke();
        }

        public void PlayerAlive()
        {
            if (OnPlayerAlive != null)
                OnPlayerAlive.Invoke();
        }

        public void PlayerRotate(GravityVector newGV)
        {
            if (OnPlayerRotation != null)
                OnPlayerRotation.Invoke(newGV);
        }

        public void CheckBoxTrigger(GameObject cb)
        {
            if (OnCheckBox != null)
                OnCheckBox.Invoke(cb);
        }

        public void FinishLevel()
        {
            if (OnLevelFinished != null)
                OnLevelFinished.Invoke();

            Time.timeScale = 0;
        }

        #endregion Player

        #region Achiv

        private bool _destroyOnLevel = false;
        private int _levelsNoDestroy = 0;
        private int _deathOnPlay = 0;

        private void Start()
        {
            OnPlayerDestroy += () =>
            {
                _destroyOnLevel = true;
                _deathOnPlay++;

                if (_deathOnPlay == 5)
                    AchController.Instance.ShowAch("Do not give up!", 20);
                else if (_deathOnPlay == 50)
                    AchController.Instance.ShowAch("DDOS", 120);
            };

            OnLevelFinished += () =>
            {
                if (!_destroyOnLevel)
                    _levelsNoDestroy++;

                if (_levelsNoDestroy == 5)
                    AchController.Instance.ShowAch("Unlimited power!", 60);
            };

            OnLevelStart += () =>
            {
                _destroyOnLevel = false;
            };

            OnLevelRestart += () =>
            {
                _destroyOnLevel = false;
            };
        }

        #endregion Achiv

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
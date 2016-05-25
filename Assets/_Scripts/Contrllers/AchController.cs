using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Controller
{
    public class AchController : MonoBehaviour
    {
        #region Instance

        private static AchController _instance = null;

        public static AchController Instance
        {
            get
            {
                return _instance;
            }
        }

        private void Awake()
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

        public const string FILE = "aac.bin";

        private Ach _achivs = null;

        public const int RateInSession = 3;
        public const int TimePerFirst = 100;
        public const int RateSleepTime = 300;

        //public int TimePerFirst = 20;
        //public int RateSleepTime = 50;

        public List<string> RateText = new List<string>();
        public DateTime LastRateCheck;
        public int rateInSession = 0;

        private void Start()
        {
            _achivs = Ach.Load();

            RateText = new List<string>()
            {
            LocalController.Instance.L("rate_message", "1"),
            LocalController.Instance.L("rate_message", "2"),
            LocalController.Instance.L("rate_message", "3"),
            LocalController.Instance.L("rate_message", "4"),
            LocalController.Instance.L("rate_message", "5"),
            };

            if (_achivs == null)
            {
                _achivs = new Ach();
                Ach.Save(_achivs);
            }

            if (!Application.isMobilePlatform)
                return;

            if (!_achivs.IsRated && MarketController.Instance.PMone.ShowAD)
            {
                LastRateCheck = DateTime.Now;

                int x = Mathf.Abs((int)_achivs.SessionData.Subtract(LastRateCheck).TotalDays);
                if (x > 0)
                {
                    _achivs.SessionData = DateTime.Now;
                    _achivs.RateToday = true;
                    Save();
                }

                if (!_achivs.RateToday)
                    return;

                GameController.Instance.OnLevelFinished += () =>
                {
                    if (!_achivs.RateToday)
                        return;

                    if (_achivs.IsRated || !MarketController.Instance.PMone.ShowAD)
                        return;

                    if (rateInSession >= RateInSession)
                    {
                        _achivs.RateToday = false;
                        Save();
                        return;
                    }

                    if (!AndroidNativeFunctions.isConnectInternet())
                        return;

                    int i = Mathf.Abs((int)LastRateCheck.Subtract(DateTime.Now).TotalSeconds);

                    if (rateInSession == 0)
                    {
                        if (i >= TimePerFirst)
                        {
                            ShowRateMessage();
                            LastRateCheck = DateTime.Now;
                            rateInSession++;
                        }
                    }
                    else
                    {
                        if (i >= RateSleepTime)
                        {
                            ShowRateMessage();
                            LastRateCheck = DateTime.Now;
                            rateInSession++;
                        }
                    }
                };
            }
        }

        public void ShowRateMessage()
        {
            int index = UnityEngine.Random.Range(0, RateText.Count - 1);
            string text = RateText[index];
            AndroidNativeFunctions.ShowAlert(text, "Rate", "Go to market", "Later", "", (DialogInterface di) =>
            {
                if (di == DialogInterface.Positive)
                {
                    AndroidNativeFunctions.OpenGooglePlay("com.MadRat.Strom");
                    _achivs.IsRated = true;
                    Save();
                }
            });
        }

        public void ShowAch(string name, int revard)
        {
            if (_achivs != null)
            {
                if (_achivs.Add(name, revard))
                {
                    AndroidNativeFunctions.ShowToast(string.Format("{0}! +{1} batteries", name, revard.ToString()));
                    MarketController.Instance.AddMoney(revard);
                }
            }
        }

        private void Save()
        {
            Ach.Save(_achivs);
        }

        private void OnDestroy()
        {
            if (_achivs != null)
                Ach.Save(_achivs);
        }

        [Serializable]
        public class Ach
        {
            public List<KeyValuePair<string, int>> ach;
            public bool IsRated = false;
            public DateTime SessionData = DateTime.Now;
            public bool RateToday = true;

            public bool InAch(string s)
            {
                if (ach == null || ach.Count == 0)
                    return false;

                foreach (var kvp in ach)
                {
                    if (kvp.Key == s)
                    {
                        return true;
                    }
                }

                return false;
            }

            public bool Add(string s, int revard)
            {
                if (InAch(s))
                    return false;

                KeyValuePair<string, int> k = new KeyValuePair<string, int>(s, revard);
                ach.Add(k);

                Save(this);
                return true;
            }

            public Ach()
            {
                ach = new List<KeyValuePair<string, int>>();
                IsRated = false;
                SessionData = DateTime.Now;
                RateToday = true;

                Save(this);
            }

            public static Ach Load()
            {
                if (File.Exists(Application.persistentDataPath + FILE))
                {
                    try
                    {
                        using (FileStream stream = new FileStream(Application.persistentDataPath + FILE, FileMode.Open))
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            return (Ach)bf.Deserialize(stream);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorController.Instance.Send(AchController.Instance, ex.Message);
                        File.Delete(Application.persistentDataPath + FILE);
                        return null;
                    }
                }
                return null;
            }

            public static void Save(Ach pm)
            {
                try
                {
                    using (FileStream stream = new FileStream(Application.persistentDataPath + FILE, FileMode.Create))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(stream, pm);
                    }
                }
                catch (Exception ex)
                {
                    ErrorController.Instance.Send(MarketController.Instance, ex.Message);
                }
            }
        }
    }
}
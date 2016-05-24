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

        public const string FILE = "aac.bin";

        private Ach _achivs = null;

        private void Start()
        {
            _achivs = Ach.Load();

            if (_achivs == null)
            {
                _achivs = new Ach();
                Ach.Save(_achivs);
            }
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

        private void OnDestroy()
        {
            if (_achivs != null)
                Ach.Save(_achivs);
        }

        public class Ach
        {
            public List<KeyValuePair<string, int>> ach;

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
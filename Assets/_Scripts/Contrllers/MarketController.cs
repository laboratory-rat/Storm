using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Controller
{
    public class MarketController : MonoBehaviour
    {
        #region Instance

        private static MarketController _instance = null;

        public static MarketController Instance
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

        public delegate void SimpleVoid();

        public event SimpleVoid OnPortmoneChanged;

        public event SimpleVoid OnEnergyChanged;

        public event SimpleVoid OnMoneyChanged;

        public const float DoubleEnergyCosts = 1.5f;
        public const float HalfRestCost = 1.5f;
        public const float NoADCost = 3;
        public const float UnlimitedEnergyCost = 3f;
        public const float FullRestCost = 1f;

        public Portmone PMone { get; private set; }

        public int MaxEnergy
        {
            get
            {
                if (PMone.DoubleEnergy)
                    return Portmone.MAX_ENERGY * 2;
                return Portmone.MAX_ENERGY;
            }
        }

        private void Awake()
        {
            Init();

            PMone = Portmone.Load();
            if (PMone == null)
            {
                PMone = new Portmone
                {
                    Energy = Portmone.MAX_ENERGY,
                    Money = 0,
                    LastRest = DateTime.Now,
                };

                Save();
            }
        }

        public bool MinusEnergy()
        {
            return MinusEnergy(1);
        }

        public bool MinusEnergy(int i)
        {
            if (PMone.UnlimitedEnergy)
                return true;

            if (PMone.Energy >= i)
            {
                PMone.Energy -= i;

                Save();

                return true;
            }

            return false;
        }

        public void AddEnergy(int i)
        {
            if (PMone.DoubleEnergy && PMone.Energy < Portmone.MAX_ENERGY || !PMone.DoubleEnergy && PMone.Energy < Portmone.MAX_ENERGY)
            {
                int x = Mathf.Min(i + PMone.Energy, Portmone.MAX_ENERGY);
                PMone.Energy = x;

                Save();
            }
        }

        public bool MinusMoney(int i)
        {
            if (PMone.Money >= i)
            {
                PMone.Money -= i;

                Save();

                return true;
            }

            return false;
        }

        public void AddMoney(int i)
        {
            PMone.Money += i;

            Save();
        }

        private void FixedUpdate()
        {
            if (PMone.DoubleEnergy && PMone.Energy < Portmone.MAX_ENERGY || !PMone.DoubleEnergy && PMone.Energy < Portmone.MAX_ENERGY)
            {
                TimeSpan span = DateTime.Now - PMone.LastRest;
                double seconds = span.TotalSeconds;
                double k = PMone.HalfRest ? 2 : 1;

                if (seconds > Portmone.REST_SECONDS / k)
                {
                    int i = (int)Math.Floor(seconds / Portmone.REST_SECONDS / k);
                    PMone.Energy += i;

                    if (PMone.DoubleEnergy && PMone.Energy > Portmone.MAX_ENERGY * 2)
                        PMone.Energy = Portmone.MAX_ENERGY * 2;
                    else if (PMone.Energy > Portmone.MAX_ENERGY)
                        PMone.Energy = Portmone.MAX_ENERGY;

                    PMone.LastRest = DateTime.Now;

                    Save();
                }
            }
        }

        private void Save()
        {
            Portmone.Save(PMone);

            if (OnPortmoneChanged != null)
                OnPortmoneChanged.Invoke();

            if (OnMoneyChanged != null)
                OnMoneyChanged.Invoke();

            if (OnEnergyChanged != null)
                OnEnergyChanged.Invoke();
        }
    }

    [Serializable]
    public class Portmone
    {
        public const string FILE = "/pmp.bin";

        public const int REST_SECONDS = 600;
        public const int MAX_ENERGY = 10;

        public int Energy;
        public bool DoubleEnergy = false;
        public bool HalfRest = false;
        public bool UnlimitedEnergy = false;

        public int Money;
        public bool ShowAD = true;

        public DateTime LastRest;

        #region SaveLoad

        public static Portmone Load()
        {
            if (File.Exists(Application.persistentDataPath + FILE))
            {
                try
                {
                    using (FileStream stream = new FileStream(Application.persistentDataPath + FILE, FileMode.Open))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        return (Portmone)bf.Deserialize(stream);
                    }
                }
                catch (Exception ex)
                {
                    ErrorController.Instance.Send(MarketController.Instance, ex.Message);
                    File.Delete(Application.persistentDataPath + FILE);
                    return null;
                }
            }
            return null;
        }

        public static void Save(Portmone pm)
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

        #endregion SaveLoad
    }
}
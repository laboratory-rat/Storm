using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace Controller
{
    public class LocalController : MonoBehaviour
    {
        #region Instance

        private static LocalController _instance = null;

        public static LocalController Instance
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
                Destroy(gameObject);
            }
        }

        #endregion Instance

        private bool _inst = false;

        public string Local { get; private set; }
        public const string LOCAL_DIR = "Local/";
        public const string BaseLocal = "ENGLISH";

        public delegate void ReLocaliztion();

        public event ReLocaliztion LocalizationChanged;

        public int LangInt
        {
            get
            {
                return dk.FirstOrDefault(x => x.Value == Local).Key;
            }
        }

        private Dictionary<string, Dictionary<string, string>> _baseLocalDictionary = new Dictionary<string, Dictionary<string, string>>();
        private Dictionary<string, Dictionary<string, string>> _localDictionary = new Dictionary<string, Dictionary<string, string>>();

        private const string _error = "Localization error.";

        public readonly Dictionary<int, string> dk = new Dictionary<int, string>
        {
            { 0 , "ENGLISH" },
            { 1 , "RUSSIAN" },
        };

        private void Awake()
        {
            if (!_inst)
            {
                Init();
                _inst = true;
            }

            LoadLocal(BaseLocal, false);
            if (ConfigController.Instance.Config.Local == "NULL")
            {
                Local = Application.systemLanguage.ToString().ToUpper();
                LoadLocal(Local, true);

                if (_localDictionary.Count < 1)
                {
                    Local = BaseLocal;
                    LoadLocal(Local, true);
                }

                ConfigController.Instance.Config.Local = Local;
                ConfigController.Instance.SaveConfig();
            }
            else
            {
                Local = ConfigController.Instance.Config.Local;
                LoadLocal(Local, true);
            }
        }

        public void ChangeLocal(int i)
        {
            if (i <= dk.Count)
                ChangeLocal(dk[i]);
        }

        public void ChangeLocal(string name)
        {
            LoadLocal(name);
            if (LocalizationChanged != null)
                LocalizationChanged.Invoke();

            ConfigController.Instance.Config.Local = name;
            ConfigController.Instance.SaveConfig();
        }

        public void LoadLocal(string name, bool local = true)
        {
            var d = local ? _localDictionary : _baseLocalDictionary;
            d.Clear();

            TextAsset xmlAsset;

            try
            {
                xmlAsset = Resources.Load(LOCAL_DIR + name.ToUpper()) as TextAsset;
                XDocument doc = XDocument.Load(new StringReader(xmlAsset.text));

                var dd = doc.Descendants("area").ToDictionary(
                    ns => ns.Attribute("id").Value,
                    ns => ns.Descendants("element").ToDictionary(
                        el => el.Attribute("id").Value,
                        el => el.Value
                        ));

                if (local)
                {
                    _localDictionary = dd;
                    Local = name.ToUpper();
                }
                else
                    _baseLocalDictionary = dd;
            }
            catch (Exception e)
            {
                ErrorController.Instance.Send(this, "Can`t load dictionary file. Name = " + name + "\n" + e.Message);
            }
        }

        public string L(string area_id, string element_id)
        {
            area_id = area_id.ToLower();
            element_id = element_id.ToLower();

            if (_localDictionary != null && _localDictionary.ContainsKey(area_id) && _localDictionary[area_id].ContainsKey(element_id))
                return _localDictionary[area_id][element_id];
            else if (_localDictionary != null && _baseLocalDictionary.ContainsKey(area_id) && _baseLocalDictionary[area_id].ContainsKey(element_id))
                return _baseLocalDictionary[area_id][element_id];
            else
                return _error; //Error
        }
    }
}
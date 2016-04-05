using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Controller
{
    public class ConfigController : MonoBehaviour
    {
        public const string FileName = "/Config.xml";

        private static ConfigController _instance = null;

        public static ConfigController Instance
        {
            get { return _instance; }
        }

        public ConfigFile Config { get; private set; }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else if (_instance != this)
                Destroy(this);

            LoadConfig();
        }

        private void LoadConfig()
        {
            string path = Application.persistentDataPath + FileName;
            if (File.Exists(path))
            {
                try
                {
                    using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(ConfigFile));
                        Config = ser.Deserialize(stream) as ConfigFile;
                    }
                    return;
                }
                catch (Exception e)
                {
                    ErrorController.Instance.Send(this, "Bad config file. Creating new. \n " + e.Message);
                    File.Delete(path);
                }
            }

            Config = new ConfigFile();
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                XmlSerializer ser = new XmlSerializer(typeof(ConfigFile));
                ser.Serialize(stream, Config);
            }
        }

        public void SaveConfig()
        {
            if (Config != null)
            {
                string path = Application.persistentDataPath + FileName;
                if (File.Exists(path))
                    File.Delete(path);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(ConfigFile));
                    ser.Serialize(stream, Config);
                }
            }
        }

        [Serializable]
        public class ConfigFile
        {
            public string Local = "NULL";
            public string Version = "0.1";
            public string Xyu = "Xyu";

            public ConfigFile()
            {
            }
        }
    }
}
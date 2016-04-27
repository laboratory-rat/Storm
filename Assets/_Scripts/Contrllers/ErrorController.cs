using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Controller
{
    public class ErrorController : MonoBehaviour
    {
        #region Instance

        private static ErrorController _instance = null;

        public static ErrorController Instance
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

        public int ECount { get { return Errors.Count; } }

        public const string DIR = "/Error/";

        [XmlArray("Errors")]
        private List<Error> Errors = new List<Error>();

        private void Awake()
        {
            Init();

            CheckDir();
        }

        public void Send(UnityEngine.Object ob, string message)
        {
            if (Debug.isDebugBuild)
                Debug.LogError(message);

            Errors.Add(new Error { Time = DateTime.Now, Object = ob.ToString(), AppTimeMinutes = Time.time / 60, Subject = message });
        }

        private void OnApplicationQuit()
        {
            if (ECount > 0)
            {
                CheckDir();
                string name = DateTime.Now.ToString().Replace('/', '_').Replace(' ', '_').Replace(':', '-');

                using (FileStream stream = new FileStream(Application.persistentDataPath + DIR + name + ".xml", FileMode.Create, FileAccess.Write))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(List<Error>));
                    xs.Serialize(stream, Errors);
                }
            }
        }

        private void CheckDir()
        {
            string path = Application.persistentDataPath + DIR;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        [Serializable]
        public struct Error
        {
            public DateTime Time;
            public float AppTimeMinutes;
            public string Object;
            public string Subject;
        }
    }
}
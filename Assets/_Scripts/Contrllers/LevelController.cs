using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Controller
{
    public class LevelController : MonoBehaviour
    {
        #region Instance

        private static LevelController _instance = null;

        public static LevelController Instance
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

        private void Awake()
        {
            Init();
        }

        public readonly List<World> Worlds = new List<World>
        {
            new World {Name = "First", Levels = new List<Level>
            {
                new Level ("W1L1"),
                new Level ("W1L2"),
                new Level ("W1L3"),
            } },

            new World {Name = "Second", Levels = new List<Level>
            {
                new Level ("W2L1"),
                new Level ("W2L2"),
                new Level ("W2L3"),
            } },
        };

        public List<World> OpenWords = null;
        //public GameObject ButtonPrefab;

        private const string SAVE = "/slc.bin";
        private string path;

        private void Start()
        {
            path = Application.persistentDataPath + SAVE;
            LoadProgress();
        }

        public List<Level> GetLevels(string world)
        {
            foreach (var w in Worlds)
            {
                if (w.Name == world)
                    return w.Levels;
            }

            return new List<Level>();
        }

        private void OpenNew(string world, string level)
        {
            foreach (var w in OpenWords)
            {
                if (w.Name == world)
                {
                    foreach (var l in w.Levels)
                    {
                        if (l.Name == level)
                            return;
                    }
                    w.Levels.Add(new Level(level));
                    return;
                }
            }
            OpenWords.Add(new World { Name = world, Levels = new List<Level> { new Level(level), } });
        }

        public void SetScore(string world, string level, int time, int rate)
        {
            var w = from ww in OpenWords where ww.Name == world select ww;
            var l = from ll in w.FirstOrDefault().Levels where ll.Name == level select ll;

            var le = l.FirstOrDefault();
            le.Rate = rate;
            le.Seconds = time;
            SaveProgress();
        }

        #region Bool

        public bool IsWorldOpen(string world)
        {
            foreach (var w in OpenWords)
            {
                if (w.Name == world)
                    return true;
            }

            return false;
        }

        public bool IsLevelOpen(string world, string level)
        {
            foreach (var w in OpenWords)
            {
                if (w.Name == world)
                {
                    foreach (var l in w.Levels)
                    {
                        if (l.Name == level)
                            return true;
                    }
                    return false;
                }
            }

            return false;
        }

        #endregion Bool

        #region Save/Load

        public void SaveProgress()
        {
            if (OpenWords == null || OpenWords.Count == 0)
            {
                OpenWords = new List<World>();
                OpenWords.Add(new World { Name = "First", Levels = new List<Level> { new Level("W1L1"), } });
            }

            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, OpenWords);
            }
        }

        public void LoadProgress()
        {
            if (File.Exists(path))
            {
                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        OpenWords = (List<World>)bf.Deserialize(fs);
                    }
                }
                catch (Exception ex)
                {
                    ErrorController.Instance.Send(this, ex.Message);
                    OpenWords = null;
                    SaveProgress();
                }
            }
            else
            {
                OpenWords = null;
                SaveProgress();
            }
        }

        public void Reset()
        {
            if (File.Exists(path))
                File.Delete(path);

            OpenWords.Clear();
            SaveProgress();
        }

        #endregion Save/Load
    }

    [Serializable]
    public struct World
    {
        public string Name;
        public List<Level> Levels;
        public int Count { get { return Levels.Count; } }

        public World(string name)
        {
            Name = name;
            Levels = new List<Level>();
        }
    }

    [Serializable]
    public struct Level
    {
        public string Name;
        public int Seconds;
        public int Rate;

        public Level(string name)
        {
            Name = name;
            Seconds = 0;
            Rate = 0;
        }
    }
}
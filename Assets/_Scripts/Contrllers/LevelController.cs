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

        public delegate void SimpleVoid();

        public event SimpleVoid OnLevelsChanged;

        public List<World> OpenWords = null;

        public Level CurrentLevel;
        public string CurrentWorld;

        private const string SAVE = "/slc.bin";
        private string path;

        private void Start()
        {
            path = Application.persistentDataPath + SAVE;
            LoadProgress();
        }

        public void OpenNew(string world, string level)
        {
            World w;
            if ((w = GetWorld(world)) == null)
            {
                w = LevelPackage.GetWorld(world);
                var newW = new World(w);
                OpenWords.Add(newW);
            }
            else if (GetLevelByScene(w.Name, level) == null)
            {
                Level l = new Level(LevelPackage.GetLevelByScene(world, level));
                w.Levels.Add(l);
            }
            else
                return;

            SaveProgress();

            if (OnLevelsChanged != null)
                OnLevelsChanged.Invoke();
        }

        public void UpLevelRate(string world, string level, FlashRate rate)
        {
            var l = GetLevel(world, level);

            if (l != null)
            {
                if ((int)l.Flash < (int)rate)
                    l.Flash = rate;

                SaveProgress();
            }
        }

        #region Bool

        public World GetWorld(string world)
        {
            foreach (var w in OpenWords)
            {
                if (w.Name == world)
                    return w;
            }

            return null;
        }

        public Level GetLevel(string world, string level)
        {
            foreach (var w in OpenWords)
            {
                if (w.Name == world)
                {
                    foreach (var l in w.Levels)
                    {
                        if (l.Name == level)
                            return l;
                    }
                    return null;
                }
            }

            return null;
        }

        public Level GetLevelByScene(string world, string scene)
        {
            foreach (var w in OpenWords)
            {
                if (w.Name == world)
                {
                    foreach (var l in w.Levels)
                    {
                        if (l.LevelName == scene)
                            return l;
                    }
                    return null;
                }
            }

            return null;
        }

        public Level GetLevel(World world, string level)
        {
            foreach (var l in world.Levels)
            {
                if (l.Name == level)
                    return l;
            }
            return null;
        }

        #endregion Bool

        #region Save/Load

        public void SaveProgress()
        {
            if (OpenWords == null || OpenWords.Count == 0)
            {
                OpenWords = new List<World>();
                World w = new World(LevelPackage.GameWorlds[0]);
                OpenWords.Add(w);
            }

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, OpenWords);
                }
            }
            catch (Exception ex)
            {
                ErrorController.Instance.Send(this, ex.Message);
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

            if (OnLevelsChanged != null)
                OnLevelsChanged.Invoke();
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
    public class World
    {
        public string Name;

        public int RequireFlash;
        public List<Level> Levels;
        public int Cost;

        public int Count { get { return Levels.Count; } }

        public int Flash
        {
            get
            {
                int i = 0;
                foreach (var l in Levels)
                    i += (int)l.Flash;
                return i;
            }
        }

        public World(World w)
        {
            Name = w.Name;

            RequireFlash = w.RequireFlash;
            Cost = w.Cost;
            Levels = new List<Level>();
            Levels.Add(new Level(w.Levels[0]));
        }

        public World()
        {
        }

        public void Reset()
        {
            var l = Levels[0];
            l.Reset();
            Levels = new List<Level>() { l };
        }
    }

    [Serializable]
    public class Level
    {
        public string Name;
        public string LevelName;
        public int Cost;
        public int[] Times;
        public FlashRate Flash = FlashRate.Zero;

        public Level(Level l)
        {
            Name = l.Name;
            LevelName = l.LevelName;
            Cost = l.Cost;
            Times = (int[])l.Times.Clone();
            Flash = l.Flash;
        }

        public Level()
        {
        }

        public void Reset()
        {
            Flash = FlashRate.Zero;
        }
    }

    public enum FlashRate { Zero = 0, One, Two, Three };
}
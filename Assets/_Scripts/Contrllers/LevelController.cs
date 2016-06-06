using System;
using System.Collections.Generic;
using System.IO;
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

            GameController.Instance.OnFirstEnter += OnFirstEnter;
            GameController.Instance.OnNewVerion += OnNewVersion;

            //for (int i = 0; i < 9; i++)
            //{
            //    OpenNew("First", "W1L" + (i + 1).ToString());
            //}

            OpenOnLoad();
        }

        public bool OpenLevelsOnLoad = false;
        public string[] WorldNames;
        public int[] LevelCounts;

        private void OpenOnLoad()
        {
            if (OpenLevelsOnLoad)
            {
                if (WorldNames.Length > 0 && LevelCounts.Length > 0)
                {
                    for (int w = 0; w < WorldNames.Length; w++)
                    {
                        for (int l = 0; l < LevelCounts[w]; l++)
                        {
                            string level = "W" + (w + 1) + "L" + (l + 1);
                            Debug.Log("Open " + level);
                            OpenNew(WorldNames[w], level);
                        }
                    }
                }
            }
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
                if (l != null)
                    w.Levels.Add(l);
            }
            else
                return;

            SaveProgress();

            if (OnLevelsChanged != null)
                OnLevelsChanged.Invoke();
        }

        public void OpenWorld(string world)
        {
            foreach (var ww in OpenWords)
            {
                if (ww.Name == world)
                    return;
            }

            World w;

            if ((w = LevelPackage.GetWorld(world)) != null)
            {
                OpenWords.Add(new World(w));

                if (OnLevelsChanged != null)
                    OnLevelsChanged.Invoke();

                SaveProgress();
            }
            else
            {
                ErrorController.Instance.Send(this, "Bad level pack!");
            }
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

        public bool IsAllLP()
        {
            return OpenWords.Count == LevelPackage.GameWorlds.Count;
        }

        public void OpenNexLevelPack()
        {
            int index = OpenWords.Count;

            World w = new World(LevelPackage.GameWorlds[index]);

            if (OnLevelsChanged != null)
                OnLevelsChanged.Invoke();
        }

        #endregion Bool

        #region System

        public int GetFullScore()
        {
            int i = 0;

            foreach (var w in LevelPackage.GameWorlds)
            {
                foreach (var l in w.Levels)
                {
                    Level ll;
                    if ((ll = GetLevel(w.Name, l.Name)) != null)
                    {
                        if (ll.BestTime != -1)
                        {
                            int ii = ll.Times[1] - ll.BestTime;
                            i += (ii >= 0) ? ii : 0;
                        }
                    }
                }
            }

            return i;
        }

        private void OnFirstEnter()
        {
        }

        private void OnNewVersion()
        {
        }

        #endregion System

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

                if (OnLevelsChanged != null)
                    OnLevelsChanged.Invoke();
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

        private void OnDestroy()
        {
            GameController.Instance.OnFirstEnter -= OnFirstEnter;
            GameController.Instance.OnNewVerion -= OnNewVersion;
        }
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
        public int[] Times;
        public int BestTime = -1;
        public FlashRate Flash = FlashRate.Zero;

        public Level(Level l)
        {
            Name = l.Name;
            LevelName = l.LevelName;
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
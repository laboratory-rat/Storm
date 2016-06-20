using System.Collections.Generic;

namespace Controller
{
    public static class LevelPackage
    {
        public static List<World> GameWorlds = new List<World>
        {
            new World
            {
                Name = "First",
                RequireFlash = 30,
                Cost = 15,
                Levels = new List<Level>
                {
                    new Level
                    {
                        Name = "1",
                        LevelName = "W1L1",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 14, 18 },
                    },

                    new Level
                    {
                        Name = "2",
                        LevelName = "W1L2",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 16, 22 },
                    },

                    new Level
                    {
                        Name = "3",
                        LevelName = "W1L3",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 30, 45 },
                    },

                    new Level
                    {
                        Name = "4",
                        LevelName = "W1L4",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 18, 24 },
                    },

                    new Level
                    {
                        Name = "5",
                        LevelName = "W1L5",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 65, 90 },
                    },

                    new Level
                    {
                        Name = "6",
                        LevelName = "W1L6",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 50, 80 },
                    },

                    new Level
                    {
                        Name = "7",
                        LevelName = "W1L7",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 90, 120 },
                    },

                    new Level
                    {
                        Name = "8",
                        LevelName = "W1L8",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 120, 140 },
                    },

                    new Level
                    {
                        Name = "9",
                        LevelName = "W1L9",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 100, 150 },
                    },

                    new Level
                    {
                        Name = "10",
                        LevelName = "W1L10",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 70, 95 },
                    },

                    new Level
                    {
                        Name = "11",
                        LevelName = "W1L11",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 120, 160 },
                    },

                    new Level
                    {
                        Name = "12",
                        LevelName = "W1L12",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 80, 110 },
                    },

                    new Level
                    {
                        Name = "13",
                        LevelName = "W1L13",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 30, 50 },
                    },

                    new Level
                    {
                        Name = "14",
                        LevelName = "W1L14",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 80, 115 },
                    },

                    new Level
                    {
                        Name = "15",
                        LevelName = "W1L15",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 120, 180 },
                    },
                },
            },

            new World
            {
                Name = "Second",
                RequireFlash = 38,
                Cost = 25,
                Levels = new List<Level>
                {
                    new Level
                    {
                        Name = "1",
                        LevelName = "W2L1",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 42, 59 },
                    },

                    new Level
                    {
                        Name = "2",
                        LevelName = "W2L2",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 39, 51 },
                    },

                    
                    new Level
                    {
                        Name = "3",
                        LevelName = "W2L3",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 100, 200 },
                    },

                    

                    new Level
                    {
                        Name = "4",
                        LevelName = "W2L4",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 100, 200 },
                    },

                    

                    new Level
                    {
                        Name = "5",
                        LevelName = "W2L5",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 100, 200 },
                    },

                    

                    new Level
                    {
                        Name = "6",
                        LevelName = "W2L6",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 100, 200 },
                    },

                    

                    new Level
                    {
                        Name = "7",
                        LevelName = "W2L7",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 100, 200 },
                    },

                    

                    new Level
                    {
                        Name = "8",
                        LevelName = "W2L8",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 50, 65 },
                    },

                    /*

                    new Level
                    {
                        Name = "9",
                        LevelName = "W2L9",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 100, 200 },
                    },

                    new Level
                    {
                        Name = "10",
                        LevelName = "W2L10",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 100, 200 },
                    },

                    new Level
                    {
                        Name = "11",
                        LevelName = "W2L11",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 100, 200 },
                    },

                    new Level
                    {
                        Name = "12",
                        LevelName = "W2L12",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 100, 200 },
                    },

                    new Level
                    {
                        Name = "13",
                        LevelName = "W2L13",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 100, 200 },
                    },

                    new Level
                    {
                        Name = "14",
                        LevelName = "W2L14",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 100, 200 },
                    },

                    new Level
                    {
                        Name = "15",
                        LevelName = "W2L15",
                        Flash = FlashRate.Zero,
                        Times = new int[]{ 100, 200 },
                    },

                    */
                },
            },
        };

        public static World GetWorld(string world)
        {
            foreach (var w in GameWorlds)
            {
                if (w.Name == world)
                    return w;
            }

            return null;
        }

        public static Level GetLevel(string world, string level)
        {
            foreach (var w in GameWorlds)
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

        public static Level GetLevelByScene(string world, string level)
        {
            foreach (var w in GameWorlds)
            {
                if (w.Name == world)
                {
                    foreach (var l in w.Levels)
                    {
                        if (l.LevelName == level)
                            return l;
                    }
                    return null;
                }
            }

            return null;
        }
    }
}
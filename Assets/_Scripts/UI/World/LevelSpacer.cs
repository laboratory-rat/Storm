using Controller;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class LevelSpacer : MonoBehaviour
    {
        public Transform Parent;
        public GameObject LevelPrefab;

        private List<GameObject> _current = new List<GameObject>();

        public void PlaceLevels(string worldName)
        {
            Clear();

            World world = LevelPackage.GetWorld(worldName);

            for (int i = 0; i < world.Count; i++)
            {
                var level = world.Levels[i];
                GameObject newLevel = (GameObject)Instantiate(LevelPrefab, Parent.position, Parent.rotation);
                newLevel.GetComponent<LevelButton>().Init(worldName, level, (i + 1).ToString());
                newLevel.transform.SetParent(Parent);
                _current.Add(newLevel);
            }

            var r = GetComponent<RectTransform>();
            //r.sizeDelta.Set(r.sizeDelta.x, world.Count / 6 * 20);
        }

        public void Clear()
        {
            foreach (var go in _current)
                Destroy(go);

            _current.Clear();
        }
    }
}
using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WorldSpacer : MonoBehaviour
    {
        public string Name;
        public Text Title;
        public GameObject LevelPrefab;

        private List<Level> _levels = null;
        private List<GameObject> _used = new List<GameObject>();

        private void OnEnable()
        {
            _levels = LevelController.Instance.GetLevels(Name);
            if (_levels.Count > 0)
            {
                int i = 1;

                foreach (var l in _levels)
                {
                    GameObject go = Instantiate(LevelPrefab) as GameObject;

                    if (!LevelController.Instance.IsLevelOpen(Name, l.Name))
                        go.GetComponent<Button>().interactable = false;

                    LevelButton lb = go.GetComponent<LevelButton>();
                    lb.Title.text = i.ToString();
                    lb.LevelOnClick(l.Name);

                    go.transform.SetParent(transform);

                    _used.Add(go);

                    i++;
                }
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _used.Count; i++)
            {
                Destroy(_used[i]);
            }

            _used.Clear();
        }
    }
}
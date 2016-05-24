using Controller;
using System.Collections;
using UnityEngine;

namespace Achiv
{
    public class Ach3dLevel : MonoBehaviour
    {
        public string Key = "Good start!";
        public int Value = 10;

        private void Start()
        {
            if (SceneController.Instance.GetSceneName() == "W1L3")
                GameController.Instance.OnLevelFinished += () => { AchController.Instance.ShowAch(Key, Value); };
        }
    }
}
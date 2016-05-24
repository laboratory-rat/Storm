using Controller;
using System.Collections;
using UnityEngine;

namespace Achiv
{
    public class AchLastW1Level : MonoBehaviour
    {
        public string Key = "Access enabled!";
        public int Value = 70;

        private void Start()
        {
            if (SceneController.Instance.GetSceneName().Contains("W1"))
                GameController.Instance.OnLevelFinished += () => { AchController.Instance.ShowAch(Key, Value); };
        }
    }
}
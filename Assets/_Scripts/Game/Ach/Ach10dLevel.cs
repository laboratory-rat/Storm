using Controller;
using System.Collections;
using UnityEngine;

namespace Achiv
{
    public class Ach10dLevel : MonoBehaviour
    {
        public string Key = "Nice job!";
        public int Value = 40;

        private void Start()
        {
            if (SceneController.Instance.GetSceneName() == "W1L10")
                GameController.Instance.OnLevelFinished += () => { AchController.Instance.ShowAch(Key, Value); };
        }
    }
}
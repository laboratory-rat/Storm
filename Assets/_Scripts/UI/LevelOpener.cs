using Controller;
using UnityEngine;

namespace Game.UI
{
    public class LevelOpener : MonoBehaviour
    {
        public string World = "";
        public string LevelName = "";

        private void Awake()
        {
            GameController.Instance.OnLevelFinished += OpenLevel;
        }

        private void OpenLevel()
        {
            if (!LevelController.Instance.IsLevelOpen(World, LevelName))
                LevelController.Instance.OpenNew(World, LevelName);
        }

        private void OnDestroy()
        {
            GameController.Instance.OnLevelFinished -= OpenLevel;
        }
    }
}
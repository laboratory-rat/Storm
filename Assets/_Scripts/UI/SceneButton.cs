using Controller;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class SceneButton : MonoBehaviour
    {
        public string Target;
        public LoadSceneMode TargetMode;
        public bool LoaderScene = true;

        private Button _button;

        private void Start()
        {
            if (_button = GetComponent<Button>())
            {
                _button.onClick.AddListener(() => ChangeScene());
            }
        }

        private void ChangeScene()
        {
            SceneController.Instance.ChangeScene(Target, TargetMode, LoaderScene);
        }
    }
}
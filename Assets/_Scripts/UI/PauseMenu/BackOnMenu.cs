using Controller;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PauseMenu
{
    public class BackOnMenu : MonoBehaviour
    {
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            if (_button)
                _button.onClick.AddListener(() => ReturnToMenu());
        }

        private void ReturnToMenu()
        {
            SceneController.Instance.ChangeScene(SceneController.MENU, UnityEngine.SceneManagement.LoadSceneMode.Single, false);
        }
    }
}
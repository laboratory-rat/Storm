using Controller;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PauseMenu
{
    public class HideShowPause : MonoBehaviour
    {
        public GameObject Menu;

        private Button _button;
        private bool _paused = false;

        private void Start()
        {
            _button = GetComponent<Button>();

            if (_button)
            {
                _button.onClick.AddListener(() => OnOff());
            }

            if (Menu)
                Menu.SetActive(false);
        }

        public void OnOff()
        {
            GameController.Instance.PauseGame();

            _paused = !_paused;
            Menu.SetActive(_paused);
        }
    }
}
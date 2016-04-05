using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PauseMenu
{
    public class BackgroundOffMenu : MonoBehaviour
    {
        public GameObject ButtonMenu;

        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            if (ButtonMenu && _button)
            {
                _button.onClick.AddListener(() => OffMenu());
            }
        }

        private void OffMenu()
        {
            HideShowPause h = ButtonMenu.GetComponent<HideShowPause>();
            if (h)
            {
                h.OnOff();
            }
        }
    }
}
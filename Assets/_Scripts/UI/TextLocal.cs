using Controller;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TextLocal : MonoBehaviour
    {
        public string AreaID;
        public string ElementID;

        private Text _text;

        private void Start()
        {
            _text = GetComponent<Text>();
            if (_text != null)
            {
                LocalController.Instance.LocalizationChanged += Local;
                Local();
            }
            else
            {
                ErrorController.Instance.Send(this, "No text component");
            }
        }

        private void Local()
        {
            _text.text = LocalController.Instance.L(AreaID, ElementID);
        }

        private void OnDestroy()
        {
            LocalController.Instance.LocalizationChanged -= Local;
        }
    }
}
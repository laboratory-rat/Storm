using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class AccelBlockerButton : MonoBehaviour
    {
        private CameraRotator _camRot;
        private Button _button;

        private void Start()
        {
            if ((_camRot = FindObjectOfType<CameraRotator>()) != null && (_button = GetComponent<Button>()) != null)
            {
                _button.onClick.AddListener(() => _camRot.Block());
            }
        }
    }
}
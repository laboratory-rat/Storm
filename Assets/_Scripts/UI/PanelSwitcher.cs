using UnityEngine;

namespace UI
{
    public class PanelSwitcher : MonoBehaviour
    {
        public GameObject[] Panels;
        public GameObject StartPanel;

        private GameObject _cp = null;

        private void Start()
        {
            foreach (var g in Panels)
                g.SetActive(false);

            if (StartPanel)
            {
                StartPanel.SetActive(true);
                _cp = StartPanel;
            }
        }

        public void Switch(GameObject go)
        {
            if (_cp)
                _cp.SetActive(false);

            _cp = go;
            _cp.SetActive(true);
        }

        public void ToMain()
        {
            if (_cp)
                _cp.SetActive(false);

            _cp = StartPanel;
            _cp.SetActive(true);
        }
    }
}
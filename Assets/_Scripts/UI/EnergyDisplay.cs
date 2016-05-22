using Controller;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EnergyDisplay : MonoBehaviour
    {
        public Text Display;

        private void Start()
        {
            UD();
            MarketController.Instance.OnEnergyChanged += UD;
        }

        private void UD()
        {
            Display.text = string.Format("{0}/{1}", MarketController.Instance.PMone.Energy, MarketController.Instance.MaxEnergy);
        }

        // Update is called once per frame
        private void OnDestroy()
        {
            MarketController.Instance.OnEnergyChanged -= UD;
        }
    }
}
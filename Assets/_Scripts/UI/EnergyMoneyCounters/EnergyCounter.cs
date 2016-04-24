using Controller;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Text))]
    public class EnergyCounter : MonoBehaviour
    {
        private Text _text;

        private void Start()
        {
            _text = GetComponent<Text>();

            MarketController.Instance.OnEnergyChanged += UpdateCounter;
            UpdateCounter();
        }

        private void UpdateCounter()
        {
            _text.text = string.Format("{0} / {1}", MarketController.Instance.PMone.Energy, MarketController.Instance.MaxEnergy);
        }

        private void OnDestroy()
        {
            MarketController.Instance.OnEnergyChanged -= UpdateCounter;
        }
    }
}
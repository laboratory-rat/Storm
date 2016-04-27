using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Text))]
    public class MoneyCounter : MonoBehaviour
    {
        private Text _text;

        private void Start()
        {
            _text = GetComponent<Text>();

            MarketController.Instance.OnMoneyChanged += UpdateCounter;
            UpdateCounter();
        }

        private void UpdateCounter()
        {
            _text.text = string.Format("{0}", MarketController.Instance.PMone.Money);
        }

        private void OnDestroy()
        {
            MarketController.Instance.OnMoneyChanged -= UpdateCounter;
        }
    }
}
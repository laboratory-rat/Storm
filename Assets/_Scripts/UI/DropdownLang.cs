using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Dropdown))]
    public class DropdownLang : MonoBehaviour
    {
        private Dropdown _dd;

        private void Start()
        {
            _dd = GetComponent<Dropdown>();
            _dd.onValueChanged.AddListener(delegate
            {
                ValueChanged(_dd);
            });

            SetLangs();
        }

        private void SetLangs()
        {
            _dd.options.Clear();
            foreach (var k in LocalController.Instance.dk)
            {
                _dd.options.Add(new Dropdown.OptionData(LocalController.Instance.L("langs", k.Value)));
            }

            _dd.value = LocalController.Instance.LangInt;

            //SetDropdownIndex(LocalController.Instance.LangInt);
        }

        private void Destroy()
        {
            _dd.onValueChanged.RemoveAllListeners();
        }

        private void ValueChanged(Dropdown target)
        {
            LocalController.Instance.ChangeLocal(target.value);
            SetLangs();
        }

        public void SetDropdownIndex(int index)
        {
            _dd.value = index;
        }
    }
}
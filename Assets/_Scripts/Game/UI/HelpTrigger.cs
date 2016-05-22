using System.Collections;
using UnityEngine;

namespace Game.UI
{
    public class HelpTrigger : MonoBehaviour
    {
        public bool OneTime = false;
        public HUI hui = HUI.Warning;

        public bool UseTriggerCommand = false;
        public HUITrigger Trigger = HUITrigger.First;

        private HelpManaeger _help;
        private bool _active = true;

        private void Start()
        {
            _help = FindObjectOfType<HelpManaeger>();
        }

        private void OnTriggerEnter(Collider col)
        {
            if (_active)
            {
                if (!UseTriggerCommand)
                {
                    _help.PushSimpleCommand(hui);
                    _active = false;
                    StartCoroutine(Sleep());
                }
                else
                {
                    _help.PushTrainingCommand(Trigger);
                    _active = false;
                }
            }
        }

        private IEnumerator Sleep()
        {
            yield return new WaitForSeconds(3f);
            if (!OneTime)
                _active = true;
        }
    }
}
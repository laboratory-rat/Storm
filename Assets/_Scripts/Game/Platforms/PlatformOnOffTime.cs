using System.Collections;
using System.Timers;
using UnityEngine;

namespace Game.Platform
{
    public class PlatformOnOffTime : _PlatformBase
    {
        public float Time = 1.0f;
        public float SleepTime = 1.0f;
        public PlatformTriggerType Trigger = PlatformTriggerType.Enter;

        private bool _state = true;
        private bool _busy = false;

        public override void OnEnter(PlayerController player)
        {
            base.OnEnter(player);

            if (_state && Trigger == PlatformTriggerType.Enter && !_busy)
                StartCoroutine(Switcher(Time, true));
        }

        public override void OnExit(PlayerController player)
        {
            base.OnExit(player);
            if (_state && Trigger == PlatformTriggerType.Exit && !_busy)
                StartCoroutine(Switcher(Time, true));
        }

        private IEnumerator Switcher(float time, bool toOff = true)
        {
            _busy = true;
            yield return new WaitForSeconds(time);
            _state = !_state;

            _busy = false;

            gameObject.GetComponent<MeshRenderer>().enabled = _state;
            var c = gameObject.GetComponents<Collider>();
            foreach (var cc in c)
                cc.enabled = _state;

            if (!_state)
                _player.DeleteCollision(gameObject);
            if (toOff)
            {
                StartCoroutine(Switcher(SleepTime, false));
            }
        }
    }
}
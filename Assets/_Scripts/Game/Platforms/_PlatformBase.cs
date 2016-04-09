using Controller;
using Game;
using System.Collections;
using UnityEngine;

namespace Game.Platform
{
    public enum PlatformOnOffType { Time = 0, Trigger }

    public enum PlatformTriggerType { Enter = 0, Stay, Exit }

    public class _PlatformBase : MonoBehaviour
    {
        public bool DefaultActive = true;
        public bool IsMoving = false;

        protected PlayerController _player;

        protected virtual void Start()
        {
            gameObject.tag = "Platform";
            OnOff(DefaultActive);

            GameController gc;
            if (gc = GameController.Instance)
            {
                gc.OnPlayerDestroy += (() => _player = null);
            }
        }

        public virtual void OnEnter(PlayerController player)
        {
            if (!_player)
                _player = player;
        }

        public virtual void OnStay(PlayerController player)
        {
        }

        public virtual void OnExit(PlayerController player)
        {
        }

        public virtual void TriggerAction()
        {
        }

        public virtual void TriggerAction(bool b)
        {
            OnOff(b);
        }

        protected virtual void OnOff(bool b)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = b;
            var c = gameObject.GetComponents<Collider>();
            foreach (var cc in c)
                cc.enabled = b;

            if (!b && _player)
                _player.DeleteCollision(gameObject);
        }
    }
}
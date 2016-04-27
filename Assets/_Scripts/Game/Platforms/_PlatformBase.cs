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
        public bool IsMoving = false;

        protected PlayerController _player;

        protected virtual void Start()
        {
            gameObject.tag = "Platform";

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

        public virtual void TriggerEnter(PlayerController player)
        {
        }

        public virtual void TriggerStay(PlayerController player)
        {
        }

        public virtual void TriggerExit(PlayerController player)
        {
        }
    }
}
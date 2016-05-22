using Controller;
using UnityEngine;

namespace Game.Platform
{
    public class KeySetter : _PlatformBase
    {
        public string Command;
        public Material Material;

        [HideInInspector]
        public bool Success = false;

        private bool _active = true;
        private ParticleSystem _parts;

        protected override void Start()
        {
            base.Start();

            GameController.Instance.OnPlayerDestroy += Reset;
            _parts = GetComponentInChildren<ParticleSystem>();

            var e = _parts.emission;
            _parts.GetComponent<ParticleSystemRenderer>().material = Material;
        }

        public override void TriggerEnter(PlayerController player)
        {
            base.TriggerEnter(player);

            if (_active && !Success)
            {
                KeyController kc;
                if (kc = player.GetComponent<KeyController>())
                {
                    if (kc.Command != Command)
                    {
                        kc.SetCommand(Material, Command, this);
                    }

                    _active = false;

                    var e = _parts.emission;
                    e.enabled = false;
                }
            }
        }

        public void Reset()
        {
            if (Success)
                return;

            _active = true;
            var e = _parts.emission;
            e.enabled = true;
        }

        private void OnDestroy()
        {
            GameController.Instance.OnPlayerDestroy -= Reset;
        }
    }
}
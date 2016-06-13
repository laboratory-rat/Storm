using System.Collections;
using UnityEngine;

namespace Game.Platform
{
    public class TeleportExit : _PlatformBase
    {
        private ParticleSystem _particles;
        private AudioSource _audio;

        protected override void Start()
        {
            base.Start();

            _particles = GetComponentInChildren<ParticleSystem>();
            _audio = GetComponent<AudioSource>();
        }

        public override void TriggerEnter(PlayerController player)
        {
            base.TriggerEnter(player);
        }

        public void ExitSplash()
        {
            if (_particles)
            {
                _particles.Emit(30);
            }

            if (_audio)
            {
                _audio.Play();
            }
        }
    }
}
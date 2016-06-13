using System.Collections;
using UnityEngine;

namespace Game.Platform
{
    public class TeleportEnter : _PlatformBase
    {
        public GameObject ExitPoint;

        private AudioSource _audio;

        protected override void Start()
        {
            base.Start();
            _audio = GetComponent<AudioSource>();
        }

        public override void TriggerEnter(PlayerController player)
        {
            base.TriggerEnter(player);

            if (ExitPoint)
            {
                player.transform.position = ExitPoint.transform.position;

                TeleportExit exit;
                if ((exit = ExitPoint.GetComponent<TeleportExit>()))
                {
                    exit.ExitSplash();
                }

                if (_audio)
                {
                    _audio.Play();
                }
            }
            else
            {
                Controller.ErrorController.Instance.Send(this, "No teleport exit!");
            }
        }
    }
}
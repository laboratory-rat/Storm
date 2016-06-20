using System.Collections;
using UnityEngine;

namespace Game.Platform
{
    public class TeleportEnter : _PlatformBase
    {
        public GameObject ExitPoint;
        public Material NewMaterial;

        private AudioSource _audio;

        protected override void Start()
        {
            base.Start();
            _audio = GetComponent<AudioSource>();

            if (NewMaterial)
            {
                GetComponentInChildren<ParticleSystemRenderer>().material = NewMaterial;
            }
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
                    if (NewMaterial)
                    {
                        exit.ChangeMaterial(NewMaterial);
                    }

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
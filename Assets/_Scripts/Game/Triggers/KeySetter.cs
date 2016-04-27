using Controller;
using Game;
using UnityEngine;

namespace Game.Platform
{
    public class KeySetter : _PlatformBase
    {
        public Color NewColor;
        public string Command;

        protected override void Start()
        {
            base.Start();

            GetComponentInChildren<ParticleSystem>().startColor = NewColor;
        }

        public override void TriggerEnter(PlayerController player)
        {
            base.TriggerEnter(player);

            KeyController kc;
            if (kc = player.GetComponent<KeyController>())
            {
                if (kc.Command != Command)
                {
                    kc.SetCommand(NewColor, Command);
                }
            }
        }
    }
}
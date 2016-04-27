using Controller;
using Game;
using UnityEngine;

namespace Game.Platform
{
    public class KeyAction : _PlatformBase
    {
        public string RequiredCommand;
        public GameObject TargetObject;
        public bool Action;

        protected override void Start()
        {
            base.Start();
            TargetObject.SetActive(!Action);
        }

        public override void TriggerEnter(PlayerController player)
        {
            base.TriggerEnter(player);

            KeyController kc;
            if (kc = player.GetComponent<KeyController>())
            {
                if (kc.Command == RequiredCommand)
                {
                    kc.Reset();
                    TargetObject.SetActive(Action);
                }
            }
        }
    }
}
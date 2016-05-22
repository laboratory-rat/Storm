using UnityEngine;

namespace Game.Platform
{
    public class KeyAction : _PlatformBase
    {
        public string RequiredCommand;
        public GameObject TargetObject;
        public bool Action;

        private bool _active = true;
        private Animator _anim;

        protected override void Start()
        {
            base.Start();
            TargetObject.SetActive(!Action);
            _anim = GetComponent<Animator>();
        }

        public override void TriggerEnter(PlayerController player)
        {
            base.TriggerEnter(player);

            if (!_active)
                return;

            KeyController kc;
            if (kc = player.GetComponent<KeyController>())
            {
                if (kc.Command == RequiredCommand)
                {
                    kc.Success();
                    TargetObject.SetActive(Action);
                    _active = false;
                    _anim.SetTrigger("Activate");
                }
            }
        }
    }
}
using Controller;
using System.Collections;
using System.Timers;
using UnityEngine;

namespace Game.Platform
{
    public class ActionTrigger : _PlatformBase
    {
        [Header("Main")]
        public bool ActiveOnce = false;

        public GravityVector RequiredVector;
        public bool ResetOnDeath = true;

        [Header("Gravity")]
        public bool ChangeGrav = true;

        public GravityVector NewVector;

        [Header("Decoration")]
        public bool EnableDecoration = true;

        public GameObject Decoration;

        [Header("TargetObject")]
        public bool EnableObjectAction = false;

        public bool OCommand = false;
        public GameObject[] TargetObjects;

        [Header("Animation")]
        public bool TriggerAnimator = false;

        public Animator Anim;
        public string ACommand;

        [Header("CheckBoxController")]
        public bool EnableAfterCheckBox = false;

        public GameObject ECheckBox;
        public bool DisableAfterCheckBox = false;
        public GameObject DCheckBox;

        private bool _enabled = true;
        private bool _active = false;
        private bool _activated = false;

        protected override void Start()
        {
            base.Start();

            UpdateAction(FindObjectOfType<PlayerController>().GVector);
            GameController.Instance.OnPlayerRotation += UpdateAction;

            if (ResetOnDeath)
                GameController.Instance.OnPlayerDestroy += Reset;

            if (EnableAfterCheckBox || DisableAfterCheckBox)
                GameController.Instance.OnCheckBox += CheckCheckBox;

            if (EnableAfterCheckBox)
                _enabled = false;
        }

        private void CheckCheckBox(GameObject cb)
        {
            if (EnableAfterCheckBox && cb == ECheckBox)
                _enabled = true;
            else if (DisableAfterCheckBox && cb == DCheckBox)
                _enabled = false;
        }

        private void Reset()
        {
            if (!_enabled)
                return;

            if (EnableObjectAction)
            {
                foreach (var go in TargetObjects)
                    go.SetActive(!OCommand);
            }

            if (TriggerAnimator)
            {
                Anim.ResetTrigger(ACommand);
                Anim.SetTrigger("Reset");
            }

            _activated = false;
        }

        private void UpdateAction(GravityVector gv)
        {
            if (gv == RequiredVector)
                _active = true;
            else
                _active = false;

            if (EnableDecoration)
                Decoration.SetActive(_active);
        }

        public override void TriggerEnter(PlayerController player)
        {
            base.TriggerEnter(player);

            if (!_enabled)
                return;

            if (_active && !_activated)
            {
                if (ChangeGrav)
                    player.Rotate(NewVector);

                if (TriggerAnimator)
                {
                    Anim.SetTrigger(ACommand);

                    if (ResetOnDeath)
                        Anim.ResetTrigger("Reset");
                }
                if (EnableObjectAction)
                {
                    foreach (var go in TargetObjects)
                        go.SetActive(!OCommand);
                }
            }

            if (ActiveOnce && !_activated)
                _activated = true;
        }

        private void OnDestroy()
        {
            GameController.Instance.OnPlayerRotation -= UpdateAction;
            GameController.Instance.OnPlayerDestroy -= Reset;

            if (EnableAfterCheckBox || DisableAfterCheckBox)
                GameController.Instance.OnCheckBox -= CheckCheckBox;
        }
    }
}
using Controller;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class GameButton : MonoBehaviour
    {
        public bool IsActivateObjects = false;
        public bool ObjectCommand = false;
        public GameObject[] GObjects;

        [Tooltip("Reset / Start")]
        public bool IsStartAnim = false;

        public Animator Anim;

        public bool IsResetOnDeath = false;

        private bool _active = true;
        private Animator _anim;
        private ParticleSystem.EmissionModule _ps;

        private void Start()
        {
            if (IsResetOnDeath)
            {
                GameController.Instance.OnPlayerDestroy += ResetButton;
            }

            _anim = GetComponent<Animator>();
            _ps = GetComponentInChildren<ParticleSystem>().emission;

            _ps.enabled = false;
        }

        private void ResetButton()
        {
            if (IsActivateObjects)
            {
                foreach (var go in GObjects)
                {
                    go.SetActive(!ObjectCommand);
                }
            }

            if (IsStartAnim)
            {
                Anim.SetTrigger("Reset");
            }

            _active = true;
            _ps.enabled = false;
            _anim.SetTrigger("Reset");
        }

        private void OnTriggerEnter(Collider col)
        {
            PlayerController pc;

            if (pc = col.GetComponent<PlayerController>())
            {
                TriggerButton();
            }
        }

        private void TriggerButton()
        {
            if (!_active)
                return;

            if (IsActivateObjects)
            {
                foreach (var go in GObjects)
                {
                    go.SetActive(ObjectCommand);
                }
            }

            if (IsStartAnim)
            {
                Anim.SetTrigger("Start");
            }

            _ps.enabled = true;
            _anim.SetTrigger("Start");
            _active = false;
        }

        private void OnDestroy()
        {
            if (IsResetOnDeath)
            {
                GameController.Instance.OnPlayerDestroy -= ResetButton;
            }
        }
    }
}
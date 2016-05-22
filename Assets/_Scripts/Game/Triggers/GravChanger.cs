using Controller;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class GravChanger : MonoBehaviour
    {
        public GravityVector Required = GravityVector.Down;
        public GravityVector New = GravityVector.Left;

        private Animator _anim;
        private bool _active = false;

        // Use this for initialization
        private void Start()
        {
            _anim = GetComponent<Animator>();

            GameController.Instance.OnPlayerRotation += Check;

            Check(FindObjectOfType<PlayerController>().GVector);
        }

        private void Check(GravityVector gv)
        {
            if (gv == Required)
            {
                _anim.SetTrigger("Start");
                _active = true;
            }
            else
            {
                _anim.SetTrigger("Reset");
                _active = false;
            }
        }

        private void OnTriggerEnter(Collider coll)
        {
            if (!_active)
                return;

            PlayerController p;
            if (p = coll.GetComponent<PlayerController>())
            {
                p.Rotate(New);
            }
        }

        private void OnDestroy()
        {
            GameController.Instance.OnPlayerRotation -= Check;
        }
    }
}
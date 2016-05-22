using System.Collections;
using UnityEngine;

namespace Game.Platform
{
    public class PlatformD : _PlatformBase
    {
        public float WaitTime = 2f;
        public float SleepTime = 3f;
        public int Emit = 100;

        private ParticleSystem _parts;
        private Collider _col;
        private MeshRenderer _mesh;
        private bool _activated = false;

        private delegate void action();

        protected override void Start()
        {
            base.Start();
            _parts = GetComponentInChildren<ParticleSystem>();
            _col = GetComponent<Collider>();
            _mesh = GetComponent<MeshRenderer>();

            var e = _parts.emission;
            e.enabled = false;
        }

        public override void OnEnter(PlayerController player)
        {
            base.OnEnter(player);
            if (!_activated)
            {
                var e = _parts.emission;
                e.enabled = true;
                _parts.Play();
                Debug.Log("Activated");
                StartCoroutine(Sleep(WaitTime, new action(D)));
            }
        }

        private void D()
        {
            StopAllCoroutines();
            var e = _parts.emission;
            _parts.Emit(Emit);
            e.enabled = false;
            _col.enabled = false;
            _mesh.enabled = false;

            StartCoroutine(Sleep(SleepTime, new action(A)));
        }

        private void A()
        {
            StopAllCoroutines();
            _col.enabled = true;
            _mesh.enabled = true;
            _activated = false;
            var e = _parts.emission;
            e.enabled = true;
        }

        private IEnumerator Sleep(float time, action a)
        {
            yield return new WaitForSeconds(time);
            a();
        }
    }
}
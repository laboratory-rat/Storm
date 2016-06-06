using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class BaseBullet : MonoBehaviour, IBullet
    {
        public GameObject ExplosionPrefab;
        public float Speed = 1f;
        public float SleepTime = 2f;

        private Vector3 _v = Vector3.zero;
        private bool _destroyed = false;
        private GameObject _do;
        private float _range = 0f;
        private Vector3 _base;

        private AudioSource _audio;

        private void Start()
        {
            _audio = GetComponent<AudioSource>();
        }

        public void Init(Vector3 Reletive, float range)
        {
            _v = Reletive;
            _range = range;
            _base = transform.position;
        }

        private void OnCollisionEnter(Collision col)
        {
            if (_destroyed)
                return;

            var p = col.gameObject.GetComponent<PlayerController>();
            if (p && !p.IsDestroyed)
            {
                p.Destroy();
            }

            Destroy();
        }

        public void Destroy()
        {
            if (_audio)
            {
                _audio.Play();
            }

            _destroyed = true;
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            if (ExplosionPrefab)
            {
                _do = (GameObject)Instantiate(ExplosionPrefab, transform.position, transform.rotation);
                StartCoroutine(DoDestroy());
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private IEnumerator DoDestroy()
        {
            yield return new WaitForSeconds(SleepTime);
            Destroy(_do);
            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            if (!_destroyed && _v != Vector3.zero)
            {
                transform.Translate(_v * Speed * Time.fixedDeltaTime, Space.World);

                var dist = Vector3.Distance(_base, transform.position);
                if (dist > _range)
                    Destroy();
            }
        }
    }
}
using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class BaseGun : MonoBehaviour
    {
        public bool IsActive = true;
        public float RotationSpeed = 1f;
        public float ReloadTime = 2f;
        public float Range = 50f;

        public bool Round = false;
        public int IntRounds = 1;

        public GameObject BulletPrefab;
        public GameObject ShootGO;

        private PlayerController _player;
        private bool _reloaded = true;
        private bool _roundStarted = false;

        private float _roundTime = 0.25f;
        private AudioSource _audio;

        private void Start()
        {
            _player = FindObjectOfType<PlayerController>();

            _audio = GetComponent<AudioSource>();
        }

        protected void FixedUpdate()
        {
            if (_player && IsActive)
            {
                var dist = Vector3.Distance(transform.position, _player.transform.position);

                RaycastHit hit;
                if (Physics.Raycast(ShootGO.transform.position, (_player.transform.position - ShootGO.transform.position), out hit, Range))
                {
                    Vector3 relativePos = _player.transform.position - transform.position;
                    Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.forward);
                    rotation.y = 0;
                    rotation.x = 0;

                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * RotationSpeed);

                    var d1 = transform.position - ShootGO.transform.position;
                    var d2 = transform.position - _player.transform.position;

                    //Debug.Log(Vector3.Angle(d1, d2));

                    if (_reloaded && Vector3.Angle(d1, d2) <= 1f && !_player.IsDestroyed)
                    {
                        Shoot();
                    }
                }
            }
        }

        private void Shoot()
        {
            if (!Round)
            {
                var go = (GameObject)Instantiate(BulletPrefab, ShootGO.transform.position, ShootGO.transform.rotation);
                go.GetComponent<IBullet>().Init((_player.transform.position - ShootGO.transform.position).normalized, Range);
                _reloaded = false;
                _audio.Play();
                StartCoroutine(Reload());
            }
            else
            {
                StartCoroutine(RoundShoot());
            }
        }

        private IEnumerator RoundShoot()
        {
            _reloaded = false;
            for (int i = 0; i < IntRounds; i++)
            {
                _audio.Play();
                var go = (GameObject)Instantiate(BulletPrefab, ShootGO.transform.position, ShootGO.transform.rotation);
                go.GetComponent<IBullet>().Init((_player.transform.position - ShootGO.transform.position).normalized, Range);
                yield return new WaitForSeconds(_roundTime);
            }
            StartCoroutine(Reload());
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(ReloadTime);
            _reloaded = true;
        }
    }
}
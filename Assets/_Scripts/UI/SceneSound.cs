using Controller;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(AudioSource))]
    public class SceneSound : MonoBehaviour
    {
        public AudioClip[] Clips;
        public bool LoopOne = false;

        private AudioSource _source;
        private AudioClip _last = null;

        private void Start()
        {
            _source = GetComponent<AudioSource>();

            if (Clips.Length > 0)
            {
                if (Clips.Length == 1)
                {
                    _source.loop = true;
                    Play(Clips[0]);
                }
                else
                    RandomSound();
            }
            else
                ErrorController.Instance.Send(this, "No audio clips");
        }

        private void RandomSound()
        {
            AudioClip[] tmp;
            StopAllCoroutines();

            if (_last == null)
                tmp = Clips;
            else
                tmp = Clips.Where(val => val != _last).ToArray();

            int index = Random.Range(0, tmp.Length);

            _last = tmp[index];
            Play(tmp[index]);

            if (LoopOne)
                _source.loop = true;
            else
                StartCoroutine(Sleep(tmp[index].length));
        }

        private IEnumerator Sleep(float time)
        {
            yield return new WaitForSeconds(time);
            RandomSound();
        }

        private void Play(AudioClip clip)
        {
            _source.clip = clip;
            _source.Play();
        }
    }
}
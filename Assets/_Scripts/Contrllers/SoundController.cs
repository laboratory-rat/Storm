using System.Collections;
using UnityEngine;

namespace Controller
{
    public class SoundController : MonoBehaviour
    {
        #region Instance

        private static SoundController _instance = null;

        public static SoundController Instance
        {
            get
            {
                return _instance;
            }
        }

        private void Init()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(this);
            }
        }

        #endregion Instance

        public AudioSource EfxSource;                   //Drag a reference to the audio source which will play the sound effects.
        public AudioSource MusicSource;                 //Drag a reference to the audio source which will play the music.
        public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
        public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.

        private void Awake()
        {
            Init();
            CheckMusic();
            ConfigController.Instance.OnConfigSave += CheckMusic;
        }

        private void CheckMusic()
        {
            MusicSource.mute = ConfigController.Instance.Config.Backgroud == "1" ? false : true;
            EfxSource.mute = ConfigController.Instance.Config.Sfx == "1" ? false : true;
        }

        public void PlaySingle(AudioClip clip, bool rand = false)
        {
            float pitch = 1f;
            if (rand)
                pitch = Random.Range(lowPitchRange, highPitchRange);

            EfxSource.clip = clip;
            EfxSource.pitch = pitch;

            EfxSource.Play();
        }

        public void RandomizeSfx(params AudioClip[] clips)
        {
            int randomIndex = Random.Range(0, clips.Length);
            float randomPitch = Random.Range(lowPitchRange, highPitchRange);

            EfxSource.clip = clips[randomIndex];
            EfxSource.pitch = randomPitch;

            EfxSource.Play();
        }
    }
}
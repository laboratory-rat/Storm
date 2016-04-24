using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

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
        public AudioMixer Mixer;

        private void Awake()
        {
            Init();
            CheckMusic();
            ConfigController.Instance.OnConfigSave += CheckMusic;
        }

        public void CheckMusic()
        {
            float back = ConfigController.Instance.Config.Backgroud == "1" ? 0f : -80f;
            float sfx = ConfigController.Instance.Config.Sfx == "1" ? 0f : -80f;

            Mixer.SetFloat("Back", back);
            Mixer.SetFloat("Sfx", sfx);
        }
    }
}
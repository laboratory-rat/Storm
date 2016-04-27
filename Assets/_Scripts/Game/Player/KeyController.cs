using Controller;
using UnityEngine;

namespace Game
{
    public class KeyController : MonoBehaviour
    {
        public ParticleSystem Particles;
        public Color BaseColor;

        public string Command;

        private void Start()
        {
            GameController.Instance.OnPlayerDestroy += Pause;
            GameController.Instance.OnPlayerAlive += Play;

            Particles.startColor = BaseColor;
        }

        private void Pause()
        {
            Particles.Pause();
        }

        private void Play()
        {
            Particles.Play();
            Reset();
        }

        public void SetCommand(Color c, string command)
        {
            Particles.startColor = c;
            Command = command;
        }

        public void Reset()
        {
            Command = "";
            Particles.startColor = BaseColor;
        }

        private void OnDestroy()
        {
            GameController.Instance.OnPlayerDestroy -= Pause;
            GameController.Instance.OnPlayerAlive -= Play;
        }
    }
}
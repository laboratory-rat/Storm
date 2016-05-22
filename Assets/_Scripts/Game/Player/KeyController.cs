using Controller;
using Game.Platform;
using UnityEngine;

namespace Game
{
    public class KeyController : MonoBehaviour
    {
        public ParticleSystem Particles;
        public SkinnedMeshRenderer Mesh;

        public string Command;

        private Material _baseMat;
        private KeySetter _setter;

        private void Start()
        {
            GameController.Instance.OnPlayerDestroy += Pause;
            GameController.Instance.OnPlayerAlive += Play;

            _baseMat = Mesh.material;
        }

        private void Pause()
        {
            var emit = Particles.emission;
            emit.enabled = false;
        }

        private void Play()
        {
            var emit = Particles.emission;
            emit.enabled = true;

            Reset();
        }

        public void SetCommand(Material m, string command, KeySetter setter)
        {
            if (_setter == setter)
                return;
            else if (_setter != null)
            {
                _setter.Success = false;
                _setter.Reset();
                Reset();
            }

            Command = command;
            Mesh.material = m;
            _setter = setter;

            Particles.GetComponent<ParticleSystemRenderer>().material = m;
        }

        public Material GetMaterial()
        {
            return Mesh.material;
        }

        public void Success()
        {
            _setter.Success = true;
            Reset();
        }

        public void Reset()
        {
            Command = "";
            Mesh.material = _baseMat;
            Particles.GetComponent<ParticleSystemRenderer>().material = _baseMat;
        }

        private void OnDestroy()
        {
            GameController.Instance.OnPlayerDestroy -= Pause;
            GameController.Instance.OnPlayerAlive -= Play;
        }
    }
}
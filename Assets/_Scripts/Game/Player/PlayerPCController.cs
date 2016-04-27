using UnityEngine;

namespace Game
{
    public class PlayerPCController : MonoBehaviour
    {
        private PlayerController _player;

        private void Start()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
                _player = FindObjectOfType<PlayerController>();
        }

        private void FixedUpdate()
        {
            if (_player)
            {
                if (Input.GetKey(KeyCode.A))
                    _player.SetDirection(1);

                if (Input.GetKey(KeyCode.D))
                    _player.SetDirection(2);

                if (Input.GetKeyDown(KeyCode.Space))
                    _player.Jump();

                if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
                    _player.SetDirection(0);

                if (Input.GetKeyDown(KeyCode.J) && _player.IsGrounded)
                    _player.Rotate();
            }
        }
    }
}
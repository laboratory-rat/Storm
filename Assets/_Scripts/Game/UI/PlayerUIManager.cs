using Game;
using UnityEngine;

namespace GameUI
{
    public class PlayerUIManager : MonoBehaviour
    {
        private PlayerController _pc;

        public void Init(PlayerController pc)
        {
            _pc = pc;
        }

        public void Direction(int i)
        {
            _pc.SetDirection(i);
        }

        public void Jump()
        {
            _pc.Jump();
        }
    }
}
using UnityEngine;
using System.Collections;

namespace Game
{
    public enum StateBoxType { Start = 0, CheckPoint, Clear, Finish }

    public class StateBox : MonoBehaviour
    {
        public StateBoxType Type;

        public GravityVector GVector;

        public bool ChangeSpeed = false;
        public float Speed;

        public bool ChangeJumpPower = false;
        public float JumpPower;

        public bool ChangeGrav = false;
        public float Gravity;

        public bool ChangeSleepTime = false;
        public int SleepTime;
    }
}
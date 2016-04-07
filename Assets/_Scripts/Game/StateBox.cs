using System.Collections;
using UnityEngine;

namespace Game
{
    public enum StateBoxType { Start = 0, CheckPoint, Clear, Finish }

    public class StateBox : MonoBehaviour
    {
        public StateBoxType Type;

        public GravityVector GVector;

        public bool ChangeSpeed = false;
        public float Speed;

        public bool ChangeAllowJump = false;
        public bool CanJump = true;
        public bool ChangeJumpPower = false;
        public float JumpPower;

        public bool ChangeAllowRotate = false;
        public bool CanRotate = true;
        public bool ChangeGrav = false;
        public float Gravity;

        public bool ChangeSleepTime = false;
        public int SleepTime;
    }
}
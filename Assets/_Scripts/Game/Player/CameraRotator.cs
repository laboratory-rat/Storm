using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class CameraRotator : MonoBehaviour
    {
        public float MaxAngleX = 20f;
        public float MaxAngleY = 15f;
        public float SleepAccelX = 0.2f;
        public float SpleepAccelY = 0.2f;

        public float Speed = 5f;

        public Text TextField;

        private Vector3 _baseAccel = Vector3.zero;
        private Vector3 _baseVector = Vector3.zero;

        private float _currentX = 0f;
        private float _currentY = 0f;

        private bool _block = false;

        private void Start()
        {
            Camera c;
            if ((c = GetComponent<Camera>()) != null)
            {
                _baseAccel = Input.acceleration;
                _baseVector = transform.rotation.eulerAngles;
            }
            else
            {
                Controller.ErrorController.Instance.Send(this, "No camera on object");
            }
        }

        private void FixedUpdate()
        {
            if (_block)
                return;

            var x = Input.acceleration.x;
            var y = Input.acceleration.y;

            //Rotate X

            float xAbs = Mathf.Abs(x - _baseAccel.x);
            float yAbs = Mathf.Abs(y - _baseAccel.y);

            if (xAbs > SleepAccelX)
            {
                if (x < _baseAccel.x && _currentX < MaxAngleX)
                {
                    var rot = xAbs * Speed;
                    _currentX += rot;
                    transform.Rotate(Vector3.up, rot, Space.Self);
                }
                else if (x > _baseAccel.x && _currentX > -MaxAngleX)
                {
                    var rot = -xAbs * Speed;
                    _currentX += rot;
                    transform.Rotate(Vector3.up, rot, Space.Self);
                }
            }
            else if (Mathf.Abs(_currentX) > 0 && Mathf.Abs(_currentX) > 0.5f && xAbs < SleepAccelX * 0.75f + _baseAccel.x)
            {
                var back = _currentX > 0 ? -1 : 1;
                var rot = back * Speed * 0.5f;

                if (_currentX > 0 && _currentX + rot < 0)
                    rot = -_currentX;
                else if (_currentX < 0 && _currentX + rot > 0)
                    rot = _currentX;

                transform.Rotate(Vector3.up, rot, Space.Self);
                _currentX += rot;
            }

            //Rotate Y

            if (yAbs > SpleepAccelY)
            {
                if (y < _baseAccel.y && _currentY < MaxAngleY)
                {
                    var rot = yAbs * Speed;
                    _currentY += rot;
                    transform.Rotate(Vector3.left, rot, Space.Self);
                }
                else if (y > _baseAccel.y && _currentY > -MaxAngleY)
                {
                    var rot = -yAbs * Speed;
                    _currentY += rot;
                    transform.Rotate(Vector3.left, rot, Space.Self);
                }
            }
            else if (Mathf.Abs(_currentY) > 0 && Mathf.Abs(_currentY) > 0.5f && yAbs < SpleepAccelY * 0.75f + _baseAccel.y)
            {
                var back = _currentY > 0 ? -1 : 1;
                var rot = back * Speed * 0.5f;

                if (_currentY > 0 && _currentY + rot < 0)
                    rot = -_currentY;
                else if (_currentY < 0 && _currentY + rot > 0)
                    rot = _currentY;

                transform.Rotate(Vector3.left, rot, Space.Self);
                _currentY += rot;
            }

            //Debug
            var t = string.Format("x: {0}\ny: {1}\nbaseAccelX: {2}\nbaseAccelY: {3}\n_currentX: {4}\n_currentY: {5}", x, y, _baseAccel.x, _baseAccel.y, _currentX, _currentY);
            TextField.text = t;
        }

        public void RefrashAccel()
        {
            transform.Rotate(Vector3.up, -_currentX, Space.Self);
            transform.Rotate(Vector3.left, -_currentY, Space.Self);

            _currentX = 0;
            _currentY = 0;

            transform.eulerAngles = _baseVector;

            _baseAccel = Input.acceleration;
        }

        public void Block()
        {
            RefrashAccel();
            _block = !_block;
        }
    }
}
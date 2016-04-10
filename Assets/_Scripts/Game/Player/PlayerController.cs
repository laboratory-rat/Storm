using Controller;
using Game.Platform;
using Game.Trigger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum MoveDirection { None = 0, Left, Right };

    public enum RotateDirection { Left = 0, Right, Flip };

    public enum GravityVector { Down = 0, Left, Up, Right };

    [RequireComponent(typeof(Rigidbody), typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        public Transform ParentTransform;

        public float Speed = 5f;
        public bool CanJump = true;
        public float JumpPower = 5f;
        public float Gravity = 9.81f;
        public int SleepTime = 48;

        public bool CanRotate = true;
        public GravityVector GVector = GravityVector.Down;

        public GameObject DestroyPrefab;
        public float DestroySleep = 5f;

        [Header("Sounds")]
        public AudioClip SoundJump;

        public AudioClip SoundRotate;
        public AudioClip SoundDetroy;
        public AudioClip SoundAlive;

        //public UnityEngine.UI.Text DebugText;

        public bool IsGrounded { get { return _collisions.Count > 0; } }
        public bool IsDestroyed { get { return _destroySleep > 0; } }

        private float _destroySleep = 0f;

        //private

        private bool _canRotate = false;
        private Rigidbody _rb;
        private Animator _anim;
        private int _sleep = 0;
        private List<GameObject> _collisions = new List<GameObject>();
        private MoveDirection _currentDirection = MoveDirection.None;
        private float _zAccel = 0f;
        private GameObject _lastDestroy = null;

        private StateBox _start = null;
        private StateBox _checkPoint = null;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _anim = GetComponent<Animator>();

            _start = GameObject.FindGameObjectWithTag("Start").GetComponent<StateBox>();
            ApplyStayBox(_start);
            ApplyPosition(_start.gameObject);
        }

        public void FixedUpdate()
        {
            if (IsDestroyed)
            {
                _destroySleep -= Time.deltaTime;
                if (_destroySleep <= 0)
                {
                    _destroySleep = 0f;
                    Destroy(_lastDestroy);

                    if (_checkPoint)
                    {
                        ApplyPosition(_checkPoint.gameObject);
                        ApplyStayBox(_checkPoint);
                    }
                    else
                    {
                        ApplyPosition(_start.gameObject);
                        ApplyStayBox(_start);
                    }
                    _anim.SetTrigger("Alive");
                    SoundController.Instance.PlaySingle(SoundAlive, true);
                }
                else
                    return;
            }

            if (_sleep > 0)
                _sleep--;

            _rb.AddForce(GetGrav());

            if (_currentDirection != MoveDirection.None)
            {
                var speed = _currentDirection == MoveDirection.Left ? -Speed : Speed;
                _rb.velocity = GetMoveVector(speed);
            }

            // Bug. Мой пердак чуть не рванул.

            //if (Input.touchCount > 0 && !IsGrounded && _sleep < 1)
            //{
            //    for (int i = 0; i < Input.touchCount; i++)
            //    {
            //        if (Input.touches[i].phase == TouchPhase.Ended)
            //        {
            //            Vector2 v = Input.touches[i].deltaPosition;

            //            DebugText.text = string.Format("X: {0}\tY: {1} ", v.normalized.x, v.normalized.y);

            //            if (Mathf.Abs(v.normalized.y) > Mathf.Abs(v.normalized.x))
            //            {
            //                DebugText.text += "\nRotated";
            //                Rotate();
            //                break;
            //            }
            //        }
            //    }
            //}

            if (CanRotate && _canRotate && !IsGrounded && _sleep < 1 && Mathf.Abs(Input.acceleration.z - _zAccel) >= 0.3f)
            {
                if (GameController.Instance != null)
                    GameController.Instance.PlayerRotate();

                SoundController.Instance.PlaySingle(SoundRotate, true);
                Rotate();
            }
        }

        public void Destroy()
        {
            if (GameController.Instance != null)
            {
                GameController.Instance.PlayerDestroy();
            }

            _destroySleep += DestroySleep;
            _anim.SetTrigger("Destroy");
            SoundController.Instance.PlaySingle(SoundDetroy, true);
            _lastDestroy = (GameObject)Instantiate(DestroyPrefab, transform.position, transform.rotation);
        }

        #region Move

        public void SetDirection(int i)
        {
            MoveDirection md = (MoveDirection)i;
            _currentDirection = md;
        }

        public void Jump()
        {
            if (IsGrounded && !IsDestroyed && CanJump)
            {
                _canRotate = true;
                _rb.velocity = GetJumpVector();
                _anim.SetTrigger("Jump");
                SoundController.Instance.PlaySingle(SoundJump, true);
                _zAccel = Input.acceleration.z;
            }
        }

        private void Rotate()
        {
            switch (GVector)
            {
                case GravityVector.Down:
                    Rotate(GravityVector.Up);
                    break;

                case GravityVector.Left:
                    Rotate(GravityVector.Right);
                    break;

                case GravityVector.Right:
                    Rotate(GravityVector.Left);
                    break;

                case GravityVector.Up:
                    Rotate(GravityVector.Down);
                    break;
            }
        }

        public void Rotate(int dir)
        {
            GravityVector gv = GravityVector.Down;
            try
            {
                gv = (GravityVector)dir;
                Rotate(gv);
            }
            catch (System.Exception e)
            {
                ErrorController.Instance.Send(this, "Can`t ransform move direction " + dir + "\n" + e.Message);
            }
        }

        public void Rotate(GravityVector md)
        {
            if (md == GVector)
                return;

            switch (GVector)
            {
                case GravityVector.Down:
                    if (md == GravityVector.Left)
                    {
                        StartCoroutine(Rotation(-90));
                    }
                    else if (md == GravityVector.Up)
                    {
                        StartCoroutine(Rotation(180));
                    }
                    else
                    {
                        StartCoroutine(Rotation(90));
                    }
                    break;

                case GravityVector.Left:
                    if (md == GravityVector.Down)
                    {
                        StartCoroutine(Rotation(-90));
                    }
                    else if (md == GravityVector.Up)
                    {
                        StartCoroutine(Rotation(90));
                    }
                    else
                    {
                        StartCoroutine(Rotation(180));
                    }
                    break;

                case GravityVector.Up:
                    if (md == GravityVector.Left)
                    {
                        StartCoroutine(Rotation(90));
                    }
                    else if (md == GravityVector.Down)
                    {
                        StartCoroutine(Rotation(180));
                    }
                    else
                    {
                        StartCoroutine(Rotation(-90));
                    }
                    break;

                case GravityVector.Right:
                    if (md == GravityVector.Left)
                    {
                        StartCoroutine(Rotation(180));
                    }
                    else if (md == GravityVector.Up)
                    {
                        StartCoroutine(Rotation(-90));
                    }
                    else
                    {
                        StartCoroutine(Rotation(90));
                    }
                    break;

                default:
                    return;
            }
            GVector = md;
            _sleep += SleepTime;
        }

        private IEnumerator Rotation(float angle)
        {
            _canRotate = false;
            var speed = angle > 0 ? 10f : -10f;
            while (Mathf.Abs(angle) > 0)
            {
                transform.parent.RotateAround(transform.position, Vector3.forward, speed);
                angle -= speed;
                yield return new WaitForFixedUpdate();
            }
        }

        #endregion Move

        #region Vectors

        private Vector3 GetPlayerVector()
        {
            switch (GVector)
            {
                case GravityVector.Down:
                    return Vector3.up;

                case GravityVector.Up:
                    return Vector3.down;

                case GravityVector.Left:
                    return Vector3.right;

                case GravityVector.Right:
                    return Vector3.left;

                default:
                    return Vector3.zero;
            }
        }

        private Vector3 GetMoveVector(float speed)
        {
            switch (GVector)
            {
                case GravityVector.Down:
                    return new Vector3(speed, _rb.velocity.y);

                case GravityVector.Left:
                    return new Vector3(_rb.velocity.x, -speed);

                case GravityVector.Up:
                    return new Vector3(-speed, _rb.velocity.y);

                case GravityVector.Right:
                    return new Vector3(_rb.velocity.x, speed);

                default:
                    return Vector3.zero;
            }
        }

        private Vector3 GetGrav()
        {
            switch (GVector)
            {
                case GravityVector.Down:
                    return new Vector3(0f, -Gravity, 0f);

                case GravityVector.Left:
                    return new Vector3(-Gravity, 0f, 0f);

                case GravityVector.Up:
                    return new Vector3(0f, Gravity, 0f);

                case GravityVector.Right:
                    return new Vector3(Gravity, 0f, 0f);

                default:
                    return Vector3.zero;
            }
        }

        private Vector3 GetJumpVector()
        {
            switch (GVector)
            {
                case GravityVector.Down:
                    return new Vector3(_rb.velocity.x, JumpPower);

                case GravityVector.Left:
                    return new Vector3(JumpPower, _rb.velocity.x);

                case GravityVector.Up:
                    return new Vector3(-_rb.velocity.x, -JumpPower);

                case GravityVector.Right:
                    return new Vector3(-JumpPower, -_rb.velocity.x);

                default:
                    return Vector3.zero;
            }
        }

        #endregion Vectors

        #region Collisions

        private void OnCollisionEnter(Collision col)
        {
            var angle = Vector3.Angle(col.contacts[0].normal, GetPlayerVector());

            if (angle <= 30f && !_collisions.Contains(col.gameObject))
                _collisions.Add(col.gameObject);

            Debug.Log(Vector3.Angle(col.contacts[0].normal, GetPlayerVector()));

            if (_canRotate)
                _canRotate = false;

            var p = col.gameObject.GetComponent<_PlatformBase>();
            if (p != null)
            {
                p.OnEnter(this);

                if (p.IsMoving && angle <= 30f)
                {
                    ParentTransform.SetParent(p.gameObject.transform);
                }
            }
        }

        private void OnCollisionStay(Collision col)
        {
            var p = col.gameObject.GetComponent<_PlatformBase>();
            if (p != null)
            {
                p.OnStay(this);
            }
        }

        private void OnCollisionExit(Collision col)
        {
            if (_collisions.Contains(col.gameObject))
                _collisions.Remove(col.gameObject);

            var p = col.gameObject.GetComponent<_PlatformBase>();
            if (p != null)
            {
                p.OnExit(this);

                if (p.IsMoving && ParentTransform.parent == p.gameObject.transform)
                    ParentTransform.SetParent(null);
            }
        }

        public void DeleteCollision(GameObject go)
        {
            if (_collisions.Contains(go))
                _collisions.Remove(go);
        }

        #endregion Collisions

        #region Triggers

        public void OnTriggerEnter(Collider col)
        {
            _TriggerBase tb;
            if (tb = col.gameObject.GetComponent<_TriggerBase>())
            {
                tb.Activate(ActivationType.Enter);
            }

            StateBox sb;
            if (sb = col.GetComponent<StateBox>())
            {
                if (sb.Type != StateBoxType.Start)
                    UseStateBox(sb);
            }
        }

        public void OnTriggerStay(Collider col)
        {
            _TriggerBase tb;

            if (tb = col.gameObject.GetComponent<_TriggerBase>())
            {
                tb.Activate(ActivationType.Stay);
            }
        }

        public void OnTriggerExit(Collider col)
        {
            _TriggerBase tb;

            if (tb = col.gameObject.GetComponent<_TriggerBase>())
            {
                tb.Activate(ActivationType.Exit);
            }
        }

        #endregion Triggers

        #region StateBoxes

        private void UseStateBox(StateBox sb)
        {
            switch (sb.Type)
            {
                case StateBoxType.Start:
                    ApplyStayBox(sb);
                    break;

                case StateBoxType.CheckPoint:
                    if (_checkPoint != sb)
                    {
                        _checkPoint = sb;

                        if (GameController.Instance != null)
                            GameController.Instance.CheckBoxTrigger();

                        ApplyStayBox(sb);
                    }
                    break;

                case StateBoxType.Clear:
                    ApplyStayBox(sb);
                    break;

                case StateBoxType.Finish:
                    if (GameController.Instance != null)
                        GameController.Instance.FinishLevel();
                    break;

                default:
                    break;
            }
        }

        public void ApplyPosition(GameObject go)
        {
            var pos = new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z);
            transform.position = pos;

            var q = go.transform.rotation;
            transform.rotation = q;
        }

        private void ApplyStayBox(StateBox sb)
        {
            if (sb.GVector != GVector)
                Rotate(sb.GVector);

            if (sb.ChangeGrav)
                Gravity = sb.Gravity;

            if (sb.ChangeJumpPower)
                JumpPower = sb.JumpPower;

            if (sb.ChangeSleepTime)
                SleepTime = sb.SleepTime;

            if (sb.ChangeSpeed)
                Speed = sb.Speed;

            if (sb.ChangeAllowJump)
                CanJump = sb.CanJump;

            if (sb.ChangeAllowRotate)
                CanRotate = sb.CanRotate;
        }

        #endregion StateBoxes
    }
}
using Controller;
using Game.Platform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum MoveDirection { None = 0, Left, Right };

    public enum RotateDirection { Left = 0, Right, Flip };

    public enum GravityVector { Down = 0, Left, Up, Right };

    [RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
    public class PlayerController : MonoBehaviour
    {
        public Transform ParentTransform;
        public Animator Anim;
        public Transform Armature;

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
        private AudioSource _audio;
        private int _sleep = 0;
        private List<GameObject> _collisions = new List<GameObject>();
        private List<GameObject> _wallCollisions = new List<GameObject>();
        private MoveDirection _currentDirection = MoveDirection.None;
        private MoveDirection _lastDirection = MoveDirection.Right;
        private float _zAccel = 0f;
        private GameObject _lastDestroy = null;
        private SkinnedMeshRenderer _mesh;

        private StateBox _start = null;
        private StateBox _checkPoint = null;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _audio = GetComponent<AudioSource>();
            _mesh = GetComponentInChildren<SkinnedMeshRenderer>();

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
                    Anim.SetTrigger("Alive");
                    _audio.clip = SoundAlive;
                    _audio.Play();
                    _mesh.enabled = true;

                    _collisions.Clear();
                    _wallCollisions.Clear();

                    GameController.Instance.PlayerAlive();
                }

                return;
            }

            if (_sleep > 0)
                _sleep--;

            _rb.AddForce(GetGrav());
            Anim.SetBool("Ground", IsGrounded);

            if (_currentDirection != MoveDirection.None && _wallCollisions.Count == 0)
            {
                var speed = _currentDirection == MoveDirection.Left ? -Speed : Speed;
                //_rb.velocity = GetMoveVector(speed);
                transform.Translate(GetMoveVector(speed) * Time.deltaTime);
                Anim.SetBool("Run", true);
            }
            else
            {
                Anim.SetBool("Run", false);
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

            if (CanRotate && _canRotate && !IsGrounded && _sleep < 1 && Mathf.Abs(Input.acceleration.normalized.z - _zAccel) >= 0.3f)
            {
                _audio.clip = SoundRotate;
                _audio.Play();
                Anim.SetTrigger("Rotate");
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
            //Anim.SetTrigger("Destroy");
            _audio.clip = SoundDetroy;
            _audio.Play();
            _lastDestroy = (GameObject)Instantiate(DestroyPrefab, transform.position, transform.rotation);
            _mesh.enabled = false;

            if (ParentTransform.parent != null)
                ParentTransform.SetParent(null);
        }

        #region Move

        public void SetDirection(int i)
        {
            MoveDirection md = (MoveDirection)i;

            if (_currentDirection == MoveDirection.None && md != MoveDirection.None)
                Anim.SetTrigger("Run");
            else if (md == MoveDirection.None && _currentDirection != MoveDirection.None)
                Anim.SetTrigger("Stop");

            _currentDirection = md;

            if (md != MoveDirection.None && md != _lastDirection)
            {
                _wallCollisions.Clear();
                Armature.Rotate(Vector3.up, 180f, Space.Self);
                _lastDirection = md;
            }
        }

        public void Jump()
        {
            if (IsGrounded && !IsDestroyed && CanJump)
            {
                _canRotate = true;
                _rb.velocity = GetJumpVector();
                Anim.SetTrigger("Jump");
                _audio.clip = SoundJump;
                _audio.Play();
                _zAccel = Input.acceleration.normalized.z;
            }
        }

        public void Rotate()
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

            if (GameController.Instance != null)
                GameController.Instance.PlayerRotate(md);

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
                        StartCoroutine(Rotation(90));
                    }
                    else if (md == GravityVector.Up)
                    {
                        StartCoroutine(Rotation(-90));
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
                        StartCoroutine(Rotation(90));
                    }
                    else
                    {
                        StartCoroutine(Rotation(-90));
                    }
                    break;

                default:
                    return;
            }
            GVector = md;
            _sleep += SleepTime;
            _currentDirection = MoveDirection.None;
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
                //case GravityVector.Down:
                //    return new Vector3(speed, _rb.velocity.y);

                //case GravityVector.Left:
                //    return new Vector3(_rb.velocity.x, -speed);

                //case GravityVector.Up:
                //    return new Vector3(-speed, _rb.velocity.y);

                //case GravityVector.Right:
                //    return new Vector3(_rb.velocity.x, speed);

                case GravityVector.Down:
                    return new Vector3(speed, 0);

                case GravityVector.Left:
                    return new Vector3(speed, 0);

                case GravityVector.Up:
                    return new Vector3(speed, 0);

                case GravityVector.Right:
                    return new Vector3(speed, 0);

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
            if (IsDestroyed)
                return;

            var angle = Vector3.Angle(col.contacts[0].normal, GetPlayerVector());

            if (angle <= 25f && !_collisions.Contains(col.gameObject))
            {
                _collisions.Add(col.gameObject);

                if (_canRotate)
                {
                    _canRotate = false;
                }
            }
            else if (!_wallCollisions.Contains(col.gameObject))
                _wallCollisions.Add(col.gameObject);

            Debug.Log(Vector3.Angle(col.contacts[0].normal, GetPlayerVector()));

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
            if (IsDestroyed)
                return;

            var p = col.gameObject.GetComponent<_PlatformBase>();
            if (p != null)
            {
                p.OnStay(this);
            }
        }

        private void OnCollisionExit(Collision col)
        {
            if (IsDestroyed)
                return;

            if (_collisions.Contains(col.gameObject))
                _collisions.Remove(col.gameObject);
            else if (_wallCollisions.Contains(col.gameObject))
                _wallCollisions.Remove(col.gameObject);

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

            if (_wallCollisions.Contains(go))
                _wallCollisions.Remove(go);
        }

        #endregion Collisions

        #region Triggers

        public void OnTriggerEnter(Collider col)
        {
            if (IsDestroyed)
                return;

            StateBox sb;
            if (sb = col.GetComponent<StateBox>())
            {
                if (sb.Type != StateBoxType.Start)
                    UseStateBox(sb);
            }

            _PlatformBase pb;

            if (pb = col.GetComponent<_PlatformBase>())
            {
                pb.TriggerEnter(this);
            }
        }

        public void OnTriggerStay(Collider col)
        {
            if (IsDestroyed)
                return;

            _PlatformBase pb;

            if (pb = col.GetComponent<_PlatformBase>())
            {
                pb.TriggerStay(this);
            }
        }

        public void OnTriggerExit(Collider col)
        {
            if (IsDestroyed)
                return;

            _PlatformBase pb;

            if (pb = col.GetComponent<_PlatformBase>())
            {
                pb.TriggerExit(this);
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
                            GameController.Instance.CheckBoxTrigger(sb.gameObject);

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
            ParentTransform.position = pos;

            transform.localPosition = Vector3.zero;

            var q = new Quaternion(go.transform.rotation.x, go.transform.rotation.y, go.transform.rotation.z, go.transform.rotation.w);
            ParentTransform.rotation = q;
        }

        private void ApplyStayBox(StateBox sb)
        {
            if (sb.GVector != GVector)
            {
                GVector = sb.GVector;
                GameController.Instance.PlayerRotate(GVector);
            }
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
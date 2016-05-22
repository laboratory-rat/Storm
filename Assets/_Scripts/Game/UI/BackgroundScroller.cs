using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class BackgroundScroller : MonoBehaviour
    {
        public float Speed = 1f;

        private PlayerController _player;
        private Vector2 _lastVector;

        private Renderer _rand;

        // Use this for initialization
        private void Start()
        {
            _player = FindObjectOfType<PlayerController>();
            _lastVector = _player.transform.position;
            _rand = GetComponent<Renderer>();
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            Vector2 current = _player.transform.position;

            //switch (_player.GVector)
            //{
            //    case GravityVector.Up:
            //        vector.x *= -1;//= -vector.x;
            //        vector.y *= -1;// -vector.y;
            //        break;

            //    case GravityVector.Right:
            //        var z = vector.y;
            //        vector.y = vector.x;
            //        vector.x = z;
            //        break;

            //    case GravityVector.Left:
            //        var zz = vector.y;
            //        vector.y = -vector.x;
            //        vector.x = -zz;
            //        break;

            //    default: //GravityVector.Down
            //        break;
            //}

            // GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", _lastVector - current);
            //GetComponent<Image>().material.mainTextureOffset = GetComponent<Image>().material.mainTextureOffset + (_lastVector - current); //.material.SetTextureOffset("_MainTex", _lastVector - current); //.material.mainTextureOffset = (_lastVector - current);

            //_lastVector.x *= -1;
            //_lastVector.y *= -1;

            //current.x *= -1;
            //current.y *= -1;

            Vector2 vector;
            Vector2 tmpVector;

            switch (_player.GVector)
            {
                case GravityVector.Down:
                    tmpVector = (_lastVector - current) * Speed / 100 * -1;
                    break;

                case GravityVector.Up:
                    tmpVector = (_lastVector - current) * Speed / 100;
                    break;

                case GravityVector.Left:
                    //var v = new Vector2(_lastVector.y, _lastVector.x);
                    //var vv = new Vector2(current.y, current.x);
                    var v = new Vector2(_lastVector.y, current.x);
                    var vv = new Vector2(current.y, _lastVector.x);
                    tmpVector = (v - vv) * Speed / 100;
                    break;

                case GravityVector.Right:
                    //var r = new Vector2(_lastVector.y, _lastVector.x);
                    //var rr = new Vector2(current.y, current.x);
                    var r = new Vector2(_lastVector.y, current.x);
                    var rr = new Vector2(current.y, _lastVector.x);
                    tmpVector = (r - rr) * Speed / 100 * -1;
                    break;

                default:
                    tmpVector = Vector2.zero;
                    break;
            }

            vector = _rand.material.mainTextureOffset + tmpVector;

            _rand.sharedMaterial.SetTextureOffset("_MainTex", vector);

            if (Application.isEditor)
                Debug.Log(vector);

            _lastVector = current;
        }
    }
}
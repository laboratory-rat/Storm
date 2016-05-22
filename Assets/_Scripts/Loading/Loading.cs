using Controller;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Loader
{
    [RequireComponent(typeof(Text))]
    public class Loading : MonoBehaviour
    {
        private AsyncOperation _ao = null;
        private Text _text;

        private void Start()
        {
            _ao = SceneManager.LoadSceneAsync(SceneController.Instance.sl, LoadSceneMode.Single);
            _text = GetComponent<Text>();
            SceneController.Instance.sl = "";
        }

        private void Update()
        {
            if (_ao != null)
            {
                _text.text = System.Math.Round(_ao.progress, 3).ToString();
            }
        }

        //IEnumerator Load()
        //{
        //}
    }
}
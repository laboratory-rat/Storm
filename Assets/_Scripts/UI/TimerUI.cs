using Controller;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TimerUI : MonoBehaviour
    {
        public string StringTime
        {
            get
            {
                if (_tick == null)
                    return "";
                return string.Format("{0}:{1}", _tick.Elapsed.Minutes, _tick.Elapsed.Seconds);
            }
        }

        public int IntTime
        {
            get
            {
                if (_tick == null)
                    return int.MaxValue;
                return (int)_tick.Elapsed.TotalSeconds;
            }
        }

        private Text _text;
        private Stopwatch _tick;

        private void Start()
        {
            _text = GetComponent<Text>();
            if (_text == null)
            {
                ErrorController.Instance.Send(this, "No text component!");
                gameObject.SetActive(false);
            }

            if (GameController.Instance != null)
            {
                GameController.Instance.OnTimescaleChanged += Pause;
                GameController.Instance.OnLevelFinished += Stop;
            }

            _tick = new Stopwatch();
            _tick.Start();
        }

        private void Update()
        {
            _text.text = StringTime;
        }

        private void Stop()
        {
            _tick.Stop();
        }

        private void Pause()
        {
            if (Time.timeScale == 1)
                _tick.Start();
            else
                _tick.Stop();
        }
    }
}
using Controller;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class MarketManager : MonoBehaviour
    {
        public GameObject Market;

        private Animator _anim;

        private void Start()
        {
            _anim = Market.GetComponent<Animator>();
            Market.SetActive(false);
        }

        public void Show()
        {
            Market.SetActive(true);
            _anim.SetBool("Show", true);
        }

        public void Hide()
        {
            StartCoroutine(hide());
        }

        private IEnumerator hide()
        {
            _anim.SetBool("Show", false);
            yield return new WaitForFixedUpdate();
            RuntimeAnimatorController ac = _anim.runtimeAnimatorController;

            foreach (var anim in ac.animationClips)
            {
                if (anim.name == "Hide")
                {
                    yield return new WaitForSeconds(anim.length);
                    break;
                }
            }

            Market.SetActive(false);
        }

        public void ShowFB()
        {
            if (!Application.isMobilePlatform)
                return;

            if (AndroidNativeFunctions.isConnectInternet())
            {
                Application.OpenURL("https://www.facebook.com/MadRatTeam");
            }
            else
            {
                AndroidNativeFunctions.ShowToast(LocalController.Instance.L("errors", "no_internet"));
            }
        }

        public void ShowTW()
        {
            if (!Application.isMobilePlatform)
                return;

            if (AndroidNativeFunctions.isConnectInternet())
            {
                Application.OpenURL("https://twitter.com/Mad_Rat_Team");
            }
            else
            {
                AndroidNativeFunctions.ShowToast(LocalController.Instance.L("errors", "no_internet"));
            }
        }

        public void OpenGP()
        {
            if (!Application.isMobilePlatform)
                return;

            if (AndroidNativeFunctions.isConnectInternet())
            {
                AndroidNativeFunctions.OpenGooglePlay("com.MadRat.Strom");
            }
            else
            {
                AndroidNativeFunctions.ShowToast(LocalController.Instance.L("errors", "no_internet"));
            }
        }

        //private IEnumerator hide()
        //{
        //    _anim.SetBool("Show", false);
        //    _anim.GetCurrentAnimatorStateInfo.
        //}
    }
}
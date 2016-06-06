using Controller;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NewsplasherManager : MonoBehaviour
    {
        public Animator Anim;
        public Text TextArea;
        private bool _show = false;

        private void Start()
        {
            if (ConfigController.Instance.Config.FirstEnter == "1")
            {
                //Anim.SetTrigger("ShowNews");
                //TextArea.text = LocalController.Instance.L("news", "new");
                ConfigController.Instance.Config.Version = Application.version;
                ConfigController.Instance.Config.FirstEnter = "0";
                ConfigController.Instance.SaveConfig();
                GameController.Instance.FirstEnter();
                //_show = true;
            }
            else if (ConfigController.Instance.Config.Version != Application.version)
            {
                //Anim.SetTrigger("ShowNews");
                //TextArea.text = LocalController.Instance.L("news", "splash").Replace("{VERSION}", Application.version);
                ConfigController.Instance.Config.Version = Application.version;
                ConfigController.Instance.SaveConfig();
                GameController.Instance.NewVersion();
                //_show = true;
            }
        }

        public void Hide()
        {
            if (_show == true)
            {
                Anim.SetTrigger("HideNews");
                _show = false;
            }
        }
    }
}
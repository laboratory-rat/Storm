using Controller;
using UnityEngine;

namespace UI
{
    public class OptionsManager : MonoBehaviour
    {
        public GameObject MusicOn;
        public GameObject MusicOff;
        public GameObject SfxOn;
        public GameObject SfxOff;
        public GameObject NotificationOn;
        public GameObject NotificationOff;

        private void Start()
        {
            if (ConfigController.Instance.Config.Backgroud == "1")
            {
                MusicOn.SetActive(true);
                MusicOff.SetActive(false);
            }
            else
            {
                MusicOn.SetActive(false);
                MusicOff.SetActive(true);
            }

            if (ConfigController.Instance.Config.Sfx == "1")
            {
                SfxOn.SetActive(true);
                SfxOff.SetActive(false);
            }
            else
            {
                SfxOn.SetActive(false);
                SfxOff.SetActive(true);
            }

            if (ConfigController.Instance.Config.Notification == "1")
            {
                NotificationOn.SetActive(true);
                NotificationOff.SetActive(false);
            }
            else
            {
                NotificationOn.SetActive(false);
                NotificationOff.SetActive(true);
            }

            SoundController.Instance.CheckMusic();
        }

        public void SetMusic(int i)
        {
            ConfigController.Instance.Config.Backgroud = i.ToString();
            ConfigController.Instance.SaveConfig();
            Start();
        }

        public void SetSfx(int i)
        {
            ConfigController.Instance.Config.Sfx = i.ToString();
            ConfigController.Instance.SaveConfig();
            Start();
        }

        public void SetNotification(int i)
        {
            ConfigController.Instance.Config.Notification = i.ToString();
            ConfigController.Instance.SaveConfig();
            Start();
        }
    }
}
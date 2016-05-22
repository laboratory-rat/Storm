using Controller;
using System.Collections;
using UnityEngine;

namespace Game.UI
{
    public enum HUI { None = 0, Up, Down, Left, Right, Warning };

    public enum HUITrigger { First = 0, Second, Third };

    public class HelpManaeger : MonoBehaviour
    {
        public Animator Anim;

        public void PushSimpleCommand(HUI h)
        {
            switch (h)
            {
                case HUI.Down:
                    Anim.SetTrigger("Down");
                    break;

                case HUI.Left:
                    Anim.SetTrigger("Left");
                    break;

                case HUI.Right:
                    Anim.SetTrigger("Right");
                    break;

                case HUI.Up:
                    Anim.SetTrigger("Up");
                    break;

                case HUI.Warning:
                    Anim.SetTrigger("Warning");
                    break;

                default:
                    break;
            }
        }

        public void PushTrainingCommand(HUITrigger trigger)
        {
            GameController.Instance.PauseGame(0f);
            Anim.SetTrigger(trigger.ToString());
        }

        public void CloseTrainingCommand()
        {
            Anim.SetTrigger("Close");
            GameController.Instance.PauseGame(1f);
        }
    }
}
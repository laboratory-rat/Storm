using Game.Platform;
using System.Collections;
using UnityEngine;

namespace Game.Trigger
{
    public enum TargetType { Platform = 0, Trigger, UI }

    public enum ActivationType { Enter = 0, Stay, Exit }

    public class _TriggerBase : MonoBehaviour
    {
        public GameObject Target;
        public bool ActiveOneTime = true;
        public bool ActivationCommand = true;
        public TargetType TType = TargetType.Platform;
        public ActivationType AType = ActivationType.Enter;
        public bool IsActive = true;

        private bool _alreadyActivated = false;

        public void Activate(ActivationType type)
        {
            if (IsActive && Target && type == AType && (!ActiveOneTime || (ActiveOneTime && !_alreadyActivated)))
            {
                switch (TType)
                {
                    case TargetType.Platform:
                        var t = Target.GetComponent<_PlatformBase>();
                        if (t)
                        {
                            t.TriggerAction(ActivationCommand);
                        }
                        break;

                    case TargetType.Trigger:
                        var tt = Target.GetComponent<_TriggerBase>();
                        if (tt)
                        {
                            tt.Activate(AType);
                        }
                        break;

                    case TargetType.UI:
                        var ui = Target.GetComponent<Itrigger>();
                        ui.TriggerAction();

                        break;

                    default:
                        break;
                }

                if (ActiveOneTime)
                    _alreadyActivated = true;
            }
        }
    }
}
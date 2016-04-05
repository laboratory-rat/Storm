using System.Collections;
using UnityEngine;

namespace Game.Trigger
{
    public interface Itrigger
    {
        void TriggerAction();

        void TriggerAction(bool state);
    }
}
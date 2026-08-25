using System;
using UnityEngine;

namespace Zyntra.Objects.Events
{
    [Serializable]
    public class Event : TimelineObject, IEvent
    {
        public virtual void EventAction()
        {
            Debug.LogWarning("[Zyntra] This is a blank event.");
        }
    }
}
using UnityEngine;

namespace Zyntra.Objects.Events
{
    public class Event : TimelineObject, IEvent
    {
        public virtual void EventAction()
        {
            Debug.LogWarning("[Zyntra] This is a blank event.");
        }
    }
}
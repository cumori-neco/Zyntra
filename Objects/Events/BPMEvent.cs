using Zyntra.Audio;

namespace Zyntra.Objects.Events
{
    public class BPMEvent : Event
    {
        public double targetBPM;
        public Conductor conductor;

        public override void EventAction()
        {
            if (conductor != null)
                conductor.UpdateBPM(targetBPM, time);
        }
    }
}
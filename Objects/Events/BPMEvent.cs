using Zyntra.Audio;
using Zyntra.Player;

namespace Zyntra.Objects.Events
{
    public class BPMEvent : Event
    {
        public double targetBPM;
        private Conductor conductor;

        public override void EventAction()
        {
            conductor = ZyntraPlayerManager.AudioConductor;
            if (conductor != null)
                conductor.UpdateBPM(targetBPM, time);
        }
        
        // For my old code
        public BPMEvent()
        {
        }

        public BPMEvent(double BPM)
        {
            targetBPM = BPM;
        }
    }
}
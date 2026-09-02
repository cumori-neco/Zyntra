using System;
using Zyntra.Audio;
using Zyntra.Player;

namespace Zyntra.Objects.Events
{
    [Serializable]
    public class BPMEvent : Event
    {
        public double targetBPM;
        private Conductor _conductor;

        public override void EventAction()
        {
            _conductor = ZyntraPlayerManager.AudioConductor;
            if (_conductor != null)
                _conductor.UpdateBPM(targetBPM, time);
        }

        // For my old code
        public BPMEvent()
        {
        }

        public BPMEvent(double bpm, double time)
        {
            targetBPM = bpm;
            this.time = time;
        }
    }
}
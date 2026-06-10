namespace Zyntra.Objects.Notes
{
    public class Note : TimelineObject, INote
    {
        // To be added
        // public virtual Judgement judgement;
        
        public bool IsProceed { get; set; }
        
        public double TimeTillProceed { get; set; } // the metric here is ms

        public virtual void UpdateRemainTime(double currentTime)
        {
            TimeTillProceed = (currentTime - time) * 1000.0;
        }
        
        public virtual void Hit()
        {
            IsProceed = true;
        }

        public virtual void Miss()
        {
            IsProceed = true;
        }
    }
}
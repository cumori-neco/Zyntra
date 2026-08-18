namespace Zyntra.Objects.Notes
{
    public interface INote
    {
        public void Hit();

        public void Miss();
        
        public bool IsProceed { get; set; }
        
        public double TimeTillProceed { get; set; }

        public void UpdateRemainTime(double currentTime);
    }
}
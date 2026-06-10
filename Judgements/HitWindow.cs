using System;
using UnityEngine;

namespace Zyntra.Judgements
{
    [CreateAssetMenu(fileName = "ZyntraHitWindow", menuName = "Zyntra/HitWindow", order = 3)]
    public class HitWindow : ScriptableObject
    {
        // Judgements copied from Project Sekai
        public double WindowSize = 125;
        public double Perfect = 41.7;
        public double Great = 83.3;
        public double Good = 108.3;
        
        public Judgement GetJudgement(double time)
        {
            Judgement j = new Judgement();
            double absTime = Math.Abs(time);

            if (absTime <= 2) j.Timing = JudgementTiming.Critical;
            else if (time < 0) j.Timing = JudgementTiming.Early;
            else j.Timing = JudgementTiming.Late;
            
            if (absTime <= Perfect) j.Type = JudgementType.Perfect;
            else if (absTime <= Great) j.Type = JudgementType.Great;
            else if (absTime <= Good) j.Type = JudgementType.Good;
            else j.Type = JudgementType.Miss;
            
            return j;
        }
    }
}
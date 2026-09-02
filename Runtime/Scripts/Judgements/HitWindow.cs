using System;
using UnityEngine;

namespace Zyntra.Judgements
{
    [CreateAssetMenu(fileName = "ZyntraHitWindow", menuName = "Zyntra/HitWindow", order = 3)]
    public class HitWindow : ScriptableObject
    {
        // Judgements copied from Project Sekai
        public double windowSize = 125;
        public double perfect = 41.7;
        public double great = 83.3;
        public double good = 108.3;

        public Judgement GetJudgement(double time)
        {
            var j = new Judgement();
            var absTime = Math.Abs(time);

            if (absTime <= 2) j.Timing = JudgementTiming.Critical;
            else if (time < 0) j.Timing = JudgementTiming.Early;
            else j.Timing = JudgementTiming.Late;

            if (absTime <= perfect) j.Type = JudgementType.Perfect;
            else if (absTime <= great) j.Type = JudgementType.Great;
            else if (absTime <= good) j.Type = JudgementType.Good;
            else j.Type = JudgementType.Miss;

            return j;
        }
    }
}
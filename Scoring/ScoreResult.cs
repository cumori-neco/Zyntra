using System;
using Zyntra.Data;
using Zyntra.Judgements;
using Zyntra.Player;

namespace Zyntra.Scoring
{
    public class ScoreResult
    {
        /// <summary>
        /// The Accuracy of the score. 
        /// eg. 92.44%
        /// </summary>
        public float Accuracy { get; set; }

        /// <summary>
        /// The score number of the score, 
        /// a full perfect score should have a number
        /// around a million.
        /// This value should not be negative.
        /// </summary>
        public uint Score { get; set; }
        
        public uint Combo { get; set; }
        public uint MaxCombo { get; set; }
        
        public bool IsFailed { get; set; }

        private readonly uint _noteCount;
        private double _scorePerPerfect;
        private double _accPerPerfect;

        private double _currentRawScore;
        private double _currentRawAcc;

        public ScoreResult(LevelData lvl)
        {
            _noteCount = (uint)lvl.Objects.Count;

            if (_noteCount != 0)
            {
                _scorePerPerfect = Math.Abs(1000000.0 / _noteCount);
                _accPerPerfect = lvl.Objects.Count / 100.0;
            }

            Score = 0;
            Accuracy = 0f;
            Combo = 0;
            MaxCombo = 0;
        }

        public void AddJudgement(JudgementType type)
        {
            double weight = 0;
            if (_noteCount == 0) return;

            switch (type)
            {
                case JudgementType.Perfect:
                    weight = 1.0;
                    break;
                case JudgementType.Great:
                    weight = 0.5;
                    break;
                case JudgementType.Good:
                    weight = 0.25;
                    break;
                case JudgementType.Miss:
                    weight = 0;
                    break;
            }

            _currentRawScore += _scorePerPerfect * weight;
            
            Score = (uint)Math.Round(_currentRawScore);
            
            _currentRawAcc += _accPerPerfect * weight;
            
            Accuracy = (float)_currentRawAcc;
            
            if(weight == 0 || (weight == 0.25 && ZyntraPlayerManager.Settings.comboBreakOnGood))
            {
                Combo = 0;
            }
            else
            {
                Combo++;
                if(Combo > MaxCombo) MaxCombo = Combo;
            }
        }
    }
}
using UnityEngine;
using Zyntra.Audio;
using Zyntra.Data;
using Zyntra.Scoring;

namespace Zyntra.Player
{
    public class ZyntraPlayerManager : MonoBehaviour
    {
        [Header("Setup")] [SerializeField] private GameSettings gameSettings;
        public static GameSettings Settings;
        [SerializeField] private Conductor audioConductor;
        public static Conductor AudioConductor;

        public static ScoreResult currentScore;
        public static LevelData levelData;

        private void Start()
        {
            // The static variables
            Settings = gameSettings;
            AudioConductor = audioConductor;
            currentScore = new ScoreResult(levelData);
        }
    }
}
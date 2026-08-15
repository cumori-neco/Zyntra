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

        public int health = Settings.defaultHealth;

        private void Start()
        {
            // The static variables
            Settings = gameSettings;
            AudioConductor = audioConductor;
            currentScore = new ScoreResult(levelData);
        }

        public void DamagePlayer()
        {
            if (!Settings.useHealth) return;
            health -= Settings.healthDamage;

            if (health <= 0 && Settings.deathTrigger == GameSettings.DeathTrigger.Ignore) Fail();
        }

        public void Fail()
        {
            Time.timeScale = 0f;
        }

        public void AddHealth(int amount)
        {
            health += amount;
            if (health > Settings.defaultHealth) health = Settings.defaultHealth;
        }
    }
}
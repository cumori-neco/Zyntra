using UnityEngine;
using Zyntra.Audio;
using Zyntra.Data;
using Zyntra.Judgements;
using Zyntra.Scoring;

namespace Zyntra.Player
{
    public class ZyntraPlayerManager : MonoBehaviour
    {
        [Header("Setup")] [SerializeField] private GameSettings gameSettings;
        public static GameSettings Settings;
        [SerializeField] private Conductor audioConductor;
        public static Conductor AudioConductor;
        [SerializeField] private HitWindow hitWindow;
        public static HitWindow HitWindow;

        public static ScoreResult CurrentScore;
        public static LevelData LevelData;

        public int health = 1000;

        private void Start()
        {
            // The static variables
            Settings = gameSettings;
            AudioConductor = audioConductor;
            CurrentScore = new ScoreResult(LevelData);
            HitWindow = hitWindow;
            health = gameSettings.defaultHealth;
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
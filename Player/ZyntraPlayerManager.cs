using UnityEngine;
using Zyntra.Audio;

namespace Zyntra.Player
{
    public class ZyntraPlayerManager : MonoBehaviour
    {
        [Header("Setup")] [SerializeField] private GameSettings gameSettings;
        public static GameSettings Settings;
        [SerializeField] private Conductor audioConductor;
        public static Conductor AudioConductor;

        private void Start()
        {
            // The static variables
            Settings = gameSettings;
            AudioConductor = audioConductor;
        }
    }
}
using UnityEngine;

namespace Zyntra.Player
{
    public class ZyntraPlayerManager : MonoBehaviour
    {
        [SerializeField] private GameSettings gameSettings;
        public static GameSettings Settings;

        private void Start()
        {
            Settings = gameSettings;
        }
    }
}
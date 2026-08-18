using UnityEngine;

namespace Zyntra.Player
{
    [CreateAssetMenu(fileName = "ZyntraGameGeneralSettings", menuName = "Zyntra/General Game Settings", order = 3)]
    public class GameSettings : ScriptableObject
    {
        [Header("General Information")] public string gameName;

        [Space] [Header("Game Rules")] public double noteSpeed = 6.0;
        
        [Tooltip("Cut the combo if a \"Good\" judgement is made.")]
        public bool comboBreakOnGood = true;

        [Tooltip("If the game will use the health system or not.")]
        public bool useHealth = false;
        
        public int defaultHealth = 1000;
        public int maxHealth = 2000;
        public int healthDamage = 100;

        public enum DeathTrigger
        {
            Ignore,
            Menu,
            Quit
        }

        /// <summary>
        /// Ignore - Simply ignore death and keep playing.
        /// Menu - Shows the menu with retry, quit, etc.
        /// Quit - Brings you to the result screen after death.
        /// </summary>
        [Tooltip("How the client will handle game death once HP goes zero.")]
        public DeathTrigger deathTrigger = DeathTrigger.Ignore;
        
    }
}
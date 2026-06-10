using UnityEngine;

namespace Zyntra
{
    [CreateAssetMenu(fileName = "ZyntraGameGeneralSettings", menuName = "Zyntra/General Game Settings", order = 3)]
    public class GameSettings : ScriptableObject
    {
        [Header("General Information")] public string gameName;
        public Sprite gameIcon;

        [Space] [Header("Game Rules")] public double noteSpeed = 6.0;
        public bool allowOptionalNoteSpeed;
        
        [Tooltip("Cut the combo if a \"Good\" judgement is made.")]
        public bool comboBreakOnGood = true;

        [Tooltip("If the game will use the health system or not.")]
        public bool useHealth = false;

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
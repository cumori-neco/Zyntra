using UnityEngine;

namespace Zyntra.Player
{
    public class ZyntraManager : MonoBehaviour
    {
        public static ZyntraManager Instance;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);

            Instance = this;
        }
    }
}
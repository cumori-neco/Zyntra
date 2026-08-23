using UnityEngine;
using Zyntra.Data;

namespace Zyntra.Player
{
    public class ZyntraManager :  MonoBehaviour
    {
        public static ZyntraManager Instance;

        private void Start()
        {
            if(Instance != null) Destroy(gameObject);
            
            Instance = this;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Network.Sync
{
    public class SyncManager : MonoBehaviour
    {
        public static SyncManager Instance;

        public Dictionary<int, ISyncedVar> SyncedVariables;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }
            
            Destroy(gameObject);
        }
    }
}
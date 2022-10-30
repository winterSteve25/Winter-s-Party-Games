using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace Network.Sync
{
    public class SyncManager : MonoBehaviour
    {
        public static SyncManager Instance;

        private PhotonView _photonView;
        private Dictionary<int, ISyncedVar> _syncedVariables;
        private Dictionary<int, object> _cachedChanges;
        private int _id;

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

        private void Start()
        {
            _syncedVariables = new Dictionary<int, ISyncedVar>();
            _cachedChanges = new Dictionary<int, object>();
            _photonView = PhotonView.Get(this);
            InvokeRepeating(nameof(CleanCache), 0, 2f);
        }

        public int AddSynced(ISyncedVar syncedVar)
        {
            _syncedVariables.Add(_id, syncedVar);
            return _id++;
        }

        public void RemoveSynced(int id)
        {
            _syncedVariables.Remove(id);
        }

        public void Changed(int id, object newValue)
        {
            _photonView.RPC(nameof(NotifyChange), RpcTarget.AllBuffered, id, newValue);
        }

        private void CleanCache()
        {
            if (!_cachedChanges.Any()) return;
            
            foreach (var (id, value) in _cachedChanges)
            {
                _syncedVariables[id].Set(value);
            }
            
            _cachedChanges.Clear();
        }
        
        [PunRPC]
        private void NotifyChange(int id, object newValue)
        {
            if (!_syncedVariables.ContainsKey(id))
            {
                // this may happen when a client's synced var is not yet created but the sender already created it.
                // so we cache the changes and apply them whenever they become available
                if (_cachedChanges.ContainsKey(id))
                {
                    _cachedChanges.Remove(id);
                }
                _cachedChanges.Add(id, newValue);
            }
            _syncedVariables[id].Set(newValue);
            Debug.Log($"Received update to var with id {id} to new value {newValue}");
        }
    }
}
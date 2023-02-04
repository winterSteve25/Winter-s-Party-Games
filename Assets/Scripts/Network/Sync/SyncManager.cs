using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Utils;

namespace Network.Sync
{
    public class SyncManager : Singleton<SyncManager>
    {
        private PhotonView _photonView;
        private Dictionary<string, ISyncedVar> _syncedVariables;
        private Dictionary<string, object> _cachedChanges;
        private Dictionary<string, (bool, object)> _cachedListChanges;
        private Dictionary<string, int> _uniqueIds;
        private int _idCurrent;
        
        private void Start()
        {
            _syncedVariables = new Dictionary<string, ISyncedVar>();
            _cachedChanges = new Dictionary<string, object>();
            _cachedListChanges = new Dictionary<string, (bool, object)>();
            _photonView = GetComponent<PhotonView>();
            InvokeRepeating(nameof(CleanCache), 0, 2f);
        }

        public string AddSynced(ISyncedVar syncedVar, string id, bool uniqueId)
        {
            var generateId = string.IsNullOrEmpty(id);
            var finalID = generateId ? (_idCurrent++).ToString() : uniqueId ? $"{id}_{GetNext(id)}" : id;
            _syncedVariables.Remove(finalID);
            _syncedVariables.Add(finalID, syncedVar);
            return finalID;

            int GetNext(string uniqueId)
            {
                if (_uniqueIds.ContainsKey(uniqueId))
                {
                    var i = _uniqueIds[uniqueId];
                    _uniqueIds[uniqueId] = i + 1;
                    return i;
                }
                
                _uniqueIds.Add(uniqueId, 1);
                return 0;
            }
        }

        public void RemoveSynced(string id)
        {
            _syncedVariables.Remove(id);
        }

        public void Changed(string id, object newValue)
        {
            _photonView.RPC(nameof(NotifyChange), RpcTarget.OthersBuffered, id, newValue);
        }

        public void AddedToList(string id, object added)
        {
            _photonView.RPC(nameof(NotifyListChange), RpcTarget.OthersBuffered, id, added, false);
        }

        public void RemovedFromList(string id, object removed)
        {
            _photonView.RPC(nameof(NotifyListChange), RpcTarget.OthersBuffered, id, removed, true);
        }

        private void CleanCache()
        {
            if (!_cachedChanges.Any()) return;
            var cleanedIds = new List<string>();
            
            foreach (var (id, value) in _cachedChanges)
            {
                if (!_syncedVariables.ContainsKey(id)) continue;
                _syncedVariables[id].SyncSet(value);
                cleanedIds.Add(id);
            }

            foreach (var (id, (removed, value)) in _cachedListChanges)
            {
                if (!_syncedVariables.ContainsKey(id)) continue;
                var syncedVar = _syncedVariables[id];
                if (syncedVar is not ISyncedList syncedList) continue;
                if (removed)
                {
                    syncedList.SyncRemove(value);
                }
                else
                {
                    syncedList.SyncAdd(value);
                }
                cleanedIds.Add(id);
            }

            foreach (var cleanedId in cleanedIds)
            {
                _cachedChanges.Remove(cleanedId);
                _cachedListChanges.Remove(cleanedId);
            }
        }
        
        [PunRPC]
        private void NotifyChange(string id, object newValue)
        {
            Debug.Log($"Received update to var with id {id} to new value {newValue}");
            
            if (!_syncedVariables.ContainsKey(id))
            {
                // this may happen when a client's synced var is not yet created but the sender already created it.
                // so we cache the changes and apply them whenever they become available
                if (_cachedChanges.ContainsKey(id))
                {
                    _cachedChanges.Remove(id);
                }
                
                _cachedChanges.Add(id, newValue);
                return;
            }

            _syncedVariables[id].SyncSet(newValue);
        }
        
        [PunRPC]
        private void NotifyListChange(string id, object obj, bool removed)
        {
            Debug.Log($"Received update (Removed: {removed}) to list with id {id} with element {obj}");
            
            if (!_syncedVariables.ContainsKey(id))
            {
                // this may happen when a client's synced var is not yet created but the sender already created it.
                // so we cache the changes and apply them whenever they become available
                if (_cachedListChanges.ContainsKey(id))
                {
                    _cachedListChanges.Remove(id);
                }
                
                _cachedListChanges.Add(id, (removed, obj));
                return;
            }

            if (_syncedVariables[id] is not ISyncedList syncedList)
            {
                Debug.LogWarning($"Tried to update list with id {id} but synced variable with id is not a list");
                return;
            }
            
            if (removed)
            {
                syncedList.SyncRemove(obj);
            }
            else
            {
                syncedList.SyncAdd(obj);
            }
        }
    }
}
using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

namespace Network
{
    public static class RoomData
    {
        public static void Set(string key, object data)
        {
            CheckIsInRoom();

            var existingTable = PhotonNetwork.CurrentRoom.CustomProperties;
            var table = new Hashtable { { key, data } };

            if (existingTable.ContainsKey(key))
            {
                var origTable = new Hashtable { { key, existingTable[key] } };
                PhotonNetwork.CurrentRoom.SetCustomProperties(table, origTable);
                return;
            }
            
            PhotonNetwork.CurrentRoom.SetCustomProperties(table);
        }

        public static T Read<T>(string key)
        {
            CheckIsInRoom();
            return (T) PhotonNetwork.CurrentRoom.CustomProperties[key];
        }

        public static void ComputeIfPresent<T>(string key, Action<T> action)
        {
            CheckIsInRoom();

            var data = PhotonNetwork.CurrentRoom.CustomProperties;
            
            if (data.ContainsKey(key))
            {
                action((T) data[key]);
            }
        }

        private static void CheckIsInRoom()
        {
            if (PhotonNetwork.InRoom)
            {
                return;
            }
            
            Debug.LogError("Tried to access room data when not in a room!");
        }
    }
}
using System;
using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Utils.Data
{
    public class PlayerData
    {
        public static void Set(Player player, string key, object data)
        {
            var existingTable = player.CustomProperties;
            var table = new Hashtable { { key, data } };

            if (existingTable.ContainsKey(key))
            {
                var origTable = new Hashtable { { key, existingTable[key] } };
                player.SetCustomProperties(table, origTable);
                return;
            }
            
            player.SetCustomProperties(table);
        }

        public static T Read<T>(Player player, string key)
        {
            return (T) player.CustomProperties[key];
        }

        public static bool HasData(Player player, string key)
        {
            return player.CustomProperties.ContainsKey(key);
        }

        public static void ComputeIfPresent<T>(Player player, string key, Action<T> action)
        {
            var data = player.CustomProperties;

            if (data.ContainsKey(key))
            {
                action((T) data[key]);
            }
        }
    }
}
using System;
using ExitGames.Client.Photon;
using Games.ScrambledEggs.Helpers;
using Games.Utils.Paint;
using UnityEngine;

public static class Bootstrap
{
    [RuntimeInitializeOnLoadMethod]
    private static void Run()
    {
        Debug.Log("Registering custom types in PhotonPeer");
        PhotonPeer.RegisterType(typeof(Vector2), 253, SerializeVector2, DeserializeVector2);
        PhotonPeer.RegisterType(typeof(Quaternion), 252, SerializeQuaternion, DeserializeQuaternion);
    }
    
    private static object DeserializeVector2(byte[] data)
    {
        var floatArray2 = new float[data.Length / 4];
        Buffer.BlockCopy(data, 0, floatArray2, 0, data.Length);
        return new Vector2(floatArray2[0], floatArray2[1]);
    }

    private static byte[] SerializeVector2(object vec2)
    {
        var v = (Vector2)vec2;
        var floatArray1 = new[] { v.x, v.y };
        var byteArray = new byte[floatArray1.Length * 4];
        Buffer.BlockCopy(floatArray1, 0, byteArray, 0, byteArray.Length);
        return byteArray;
    }
    
    private static object DeserializeQuaternion(byte[] data)
    {
        var floatArray2 = new float[data.Length / 4];
        Buffer.BlockCopy(data, 0, floatArray2, 0, data.Length);
        return new Quaternion(floatArray2[0], floatArray2[1], floatArray2[2], floatArray2[3]);
    }

    private static byte[] SerializeQuaternion(object quaternion)
    {
        var v = (Quaternion)quaternion;
        var floatArray1 = new[] { v.x, v.y, v.z, v.w };
        var byteArray = new byte[floatArray1.Length * 4];
        Buffer.BlockCopy(floatArray1, 0, byteArray, 0, byteArray.Length);
        return byteArray;
    }
}
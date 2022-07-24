using System;
using ExitGames.Client.Photon;
using UnityEngine;

public static class Bootstrap
{
    [RuntimeInitializeOnLoadMethod]
    private static void Run()
    {
        Debug.Log("Yes");
        PhotonPeer.RegisterType(typeof(Color), 254, SerializeColor, DeserializeColor);
    }

    private static object DeserializeColor(byte[] data)
    {
        var values = new float[data.Length / 4];
        Buffer.BlockCopy(data, 0, values, 0, data.Length);
        return new Color(values[0], values[1], values[2], values[3]);
    }

    private static byte[] SerializeColor(object color)
    {
        var col = (Color)color;
        var values = new[] { col.r, col.g, col.b, col.a };

        var byteArray = new byte[values.Length * 4];
        Buffer.BlockCopy(values, 0, byteArray, 0, byteArray.Length);

        return byteArray;
    }
}
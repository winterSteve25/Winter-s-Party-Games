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
        PhotonPeer.RegisterType(typeof(Color), 254, SerializeColor, DeserializeColor);
    }

    private static object DeserializeColor(byte[] data)
    {
        return ColorPaletteExtension.GetColorFromCode(data[0]);
    }

    private static byte[] SerializeColor(object color)
    {
        return new[] { ColorPaletteExtension.GetCodeFromColor((Color)color) };
    }
}
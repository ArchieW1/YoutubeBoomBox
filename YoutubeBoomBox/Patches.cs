using System.Runtime.CompilerServices;
using System.Transactions;
using HarmonyLib;
using UnityEngine;

namespace YoutubeBoomBox;

public static class Patches
{
    private static AudioClip[] _defaultAudios;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(HUDManager), nameof(HUDManager.AddChatMessage))]
    public static void AddChatMessage_Postfix(string chatMessage, string nameOfUserWhoTyped, HUDManager __instance)
    {
        if (chatMessage.StartsWith("/add") && chatMessage.Length > 5)
        {
            string input = chatMessage[5..].Trim();
            if (!string.IsNullOrEmpty(input))
            {
                AudioQueue.Add(input);
            }
        }
        else if (chatMessage.StartsWith("/clear"))
        {
            AudioQueue.Clear();
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BoomboxItem), nameof(BoomboxItem.Start))]
    public static void Start_Postfix(BoomboxItem __instance)
    {
        _defaultAudios = __instance.musicAudios;
    }


    private static bool _play = false;
    private static bool _loading = false;
    [HarmonyPrefix]
    [HarmonyPatch(typeof(BoomboxItem), nameof(BoomboxItem.StartMusic))]
    public static bool StartMusic_Prefix(bool startMusic, bool pitchDown, BoomboxItem __instance)
    {
        if (_play)
        {
            _play = false;
            return true;
        }

        if (_loading) return false;

        if (!AudioQueue.IsEmpty() && startMusic)
        {
            HUDManager.Instance.AddChatMessage("Loading audio...");
            _loading = true;
            AudioStreamer.Instance.Load(AudioQueue.Next(), clip =>
            {
                __instance.musicAudios = [clip];
                HUDManager.Instance.AddChatMessage("Loaded!");
                _play = true;
                _loading = false;
                __instance.StartMusic(startMusic, pitchDown);
            });

            // DirectAudioStreamer.Instance.StreamFromYt(AudioQueue.Next(), clip =>
            // {
            //     __instance.musicAudios = [clip];
            //     __instance.StartMusic(startMusic, pitchDown);
            // });

            return false;
        }
        else if (startMusic)
        {
            __instance.musicAudios = _defaultAudios;
        }
        return true;
    }
}

using System;
using System.Collections;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Networking;

namespace YoutubeBoomBox;

[HarmonyPatch(typeof(HUDManager))]
public class HUDManagerPatch
{
    private static readonly YoutubeService Yt = new();

    [HarmonyPostfix]
    [HarmonyPatch(nameof(HUDManager.AddTextToChatOnServer))]
    public static async void AddPlayCommandPatch(string chatMessage, int playerId, HUDManager __instance)
    {
        if(chatMessage.StartsWith("/add "))
        {
            string name;
            YoutubeService.ClearVideos();
            try
            {
                string query = chatMessage[5..];
                name = await Yt.DownloadVideoAudioAsync(query);
            }
            catch (Exception e)
            {
                Plugin.Logger.LogError(e);
                return;
            }

            __instance.StartCoroutine(LoadAudioCoroutine(name));
        } 
        else if (chatMessage == "/clear")
        {
            AudioQueue.Clear();
            YoutubeService.ClearVideos();
            Plugin.Logger.LogInfo($"Cleared audio queue.");
        }
        return;
        
        IEnumerator LoadAudioCoroutine(string name)
        {
            string uri = $"file://{YoutubeInfo.FilePath}/{name}";
            using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.MPEG);
            yield return www.SendWebRequest();

            if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Plugin.Logger.LogError(www.error);
            }
            else
            {
                Plugin.Logger.LogInfo($"Loading audio clip at: {www.uri}");
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                Plugin.Logger.LogInfo($"Loaded audio clip.");
                AudioQueue.Add(clip);
            }
        }
    }
}
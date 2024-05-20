using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using YoutubeBoomBox.Download;
using YoutubeBoomBox.Utils;

namespace YoutubeBoomBox.Commands;

public class AddCommand : CommandBase
{
    private readonly YoutubeService _yt = new();
    
    protected override string Key => "/add ";

    protected override async void Action(string lastChatMessage)
    {
        string name;
        YoutubeService.ClearVideos();
        try
        {
            string query = lastChatMessage[5..];
            name = await _yt.DownloadVideoAudioAsync(query);
        }
        catch (Exception e)
        {
            YoutubeBoomBoxPlugin.Logger.LogError(e);
            return;
        }

        StartCoroutine(LoadAudioCoroutine(name));

        HUDManager.Instance.AddTextToChatOnServer($"Player loaded.");
        return;

        IEnumerator LoadAudioCoroutine(string query)
        {
            string uri = $"file://{YoutubeInfo.FilePath}/{query}";
            using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.MPEG);
            yield return www.SendWebRequest();

            if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                YoutubeBoomBoxPlugin.Logger.LogError(www.error);
            }
            else
            {
                YoutubeBoomBoxPlugin.Logger.LogInfo($"Loading audio clip at: {www.uri}");
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                YoutubeBoomBoxPlugin.Logger.LogInfo($"Loaded audio clip.");
                AudioQueue.Add(clip);
            }
        }
    }
}
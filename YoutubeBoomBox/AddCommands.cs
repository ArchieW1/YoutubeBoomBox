using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace YoutubeBoomBox;

public class AddCommands : MonoBehaviour
{
    private readonly YoutubeService _yt = new();
    private string _lastMessage = string.Empty;
    
    private void Update()
    {
        string lastChatMessage = HUDManager.Instance.lastChatMessage;
        if (lastChatMessage.StartsWith("/add ") && _lastMessage != lastChatMessage)
        {
            AddAudio(lastChatMessage, HUDManager.Instance);
            _lastMessage = lastChatMessage;
        }
        else if (lastChatMessage == "/clear" && _lastMessage != lastChatMessage)
        {
            ClearAudio();
            _lastMessage = lastChatMessage;
        }
    }
    
    private void ClearAudio()
    {
        AudioQueue.Clear();
        YoutubeService.ClearVideos();
    }
    
    private async void AddAudio(string chatMessage, HUDManager instance)
    {
        
        string name;
        YoutubeService.ClearVideos();
        try
        {
            string query = chatMessage[5..];
            instance.AddTextToChatOnServer($"Sending search: {query}.");
            name = await _yt.DownloadVideoAudioAsync(query);
        }
        catch (Exception e)
        {
            YoutubeBoomBoxPlugin.Logger.LogError(e);
            return;
        }

        StartCoroutine(LoadAudioCoroutine(name));

        instance.AddTextToChatOnServer($"Player loaded.");
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
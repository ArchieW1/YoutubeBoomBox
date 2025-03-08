using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace YoutubeBoomBox
{
    public class AudioStreamer : MonoBehaviour
    {
        private string _streamFilePath;
        private string _ytDlpPath;
        private string _ffmpegPath;

        private static AudioStreamer _instance;
        public static AudioStreamer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("AudioStreamer").AddComponent<AudioStreamer>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }

        private void Awake()
        {
            _streamFilePath = Path.Combine(Application.temporaryCachePath, "stream.ogg");
            string dllPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            _ffmpegPath = Path.Combine(dllPath, "ffmpeg.exe");
            _ytDlpPath = Path.Combine(dllPath, "yt-dlp.exe");
        }

        public void Load(string input, Action<AudioClip> onLoaded)
        {
            StartCoroutine(StartStreaming(input, onLoaded));
        }

        private bool _isStreaming = false;
        private bool _isLoading = false;
        private IEnumerator StartStreaming(string input, Action<AudioClip> onLoaded)
        {
            yield return StreamAudio(input);
            while (_isStreaming) yield return null;
            yield return LoadAudio(onLoaded);
            while (_isLoading) yield return null;

            yield break;
        }

        private bool _isFound = true;
        private IEnumerator StreamAudio(string url)
        {
            _isStreaming = true;

            string audioUrl = null;
            yield return GetYouTubeAudioUrl(_ytDlpPath, url, aUrl => audioUrl = aUrl);
            while (!_isFound) yield return null;

            YoutubeBoomBoxPlugin.Logger.LogInfo("Starting audio file stream...");

            if (string.IsNullOrEmpty(audioUrl))
            {
                YoutubeBoomBoxPlugin.Logger.LogError("Failed to get YouTube audio URL.");
                yield break;
            }

            Process process = new()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _ffmpegPath,
                    Arguments = $"-y -nostdin -loglevel error -i \"{audioUrl}\" -vn -acodec libvorbis -f ogg \"{_streamFilePath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            while (!process.HasExited) yield return null;

            YoutubeBoomBoxPlugin.Logger.LogInfo("Streamed.");
            _isStreaming = false;
        }

        private IEnumerator LoadAudio(Action<AudioClip> onLoaded)
        {
            _isLoading = true;
            YoutubeBoomBoxPlugin.Logger.LogInfo("Loading audio...");
            using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + _streamFilePath, AudioType.OGGVORBIS);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                YoutubeBoomBoxPlugin.Logger.LogError("Error loading audio: " + www.error);
            }
            else
            {
                YoutubeBoomBoxPlugin.Logger.LogInfo("Loaded!");
                onLoaded(DownloadHandlerAudioClip.GetContent(www));
            }
            _isLoading = false;
        }

        private IEnumerator GetYouTubeAudioUrl(string ytDlpPath, string input, Action<string> onUrlFound)
        {
            _isFound = false;
            YoutubeBoomBoxPlugin.Logger.LogInfo("Finding audio url...");
            string args;
            if (input.StartsWith("ytsearch:"))
            {
                args = $"-f bestaudio --match-filter \"duration < 1800\" \"{input}\" -g";
            }
            else
            {
                args = $"-f bestaudio --no-playlist -g -- \"{input}\"";
            }

            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ytDlpPath,
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            while (!process.HasExited) yield return null;

            string result = process.StandardOutput.ReadToEnd().Trim();
            string error = process.StandardError.ReadToEnd().Trim();

            if (process.ExitCode != 0 || string.IsNullOrEmpty(result))
            {
                YoutubeBoomBoxPlugin.Logger.LogError(
                    $"yt-dlp failed (Exit Code: {process.ExitCode}): {error}"
                );
                onUrlFound(null);
            }
            else
            {
                YoutubeBoomBoxPlugin.Logger.LogInfo("Found!");
                onUrlFound(result);
            }
            _isFound = true;
        }
    }
}

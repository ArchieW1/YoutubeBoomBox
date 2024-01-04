using System;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos.Streams;

namespace YoutubeBoomBox;

public class YoutubeService
{
    private readonly WebClient _webClient = new();
    private readonly YoutubeClient _youtubeClient = new();

    public async Task<string> DownloadVideoAudioAsync(string searchQuery)
    {
        Plugin.Logger.LogInfo($"Starting video audio download for query: {searchQuery}");
        
        string jsonString = await _webClient.DownloadStringTaskAsync($"{YoutubeInfo.UriBase}?part=id&q={searchQuery}&key={YoutubeInfo.APIKey}");
        YoutubeApiResponse json = JsonConvert.DeserializeObject<YoutubeApiResponse>(jsonString);
            
        string videoId = json.items[0].id.videoId;

        string name = YoutubeInfo.MusicName;
        string fullPath = $"{YoutubeInfo.FilePath}/{name}";
        Plugin.Logger.LogInfo($"Downloading to {fullPath}.");
        await _youtubeClient.Videos.DownloadAsync(videoId, fullPath, o => o
            .SetContainer(Container.Mp3)
            .SetPreset(ConversionPreset.UltraFast)
            .SetFFmpegPath($"{YoutubeInfo.FilePath}/ffmpeg"));
        Plugin.Logger.LogInfo($"Download complete.");

        return name;
    }

    public static void ClearVideos()
    {
        string directoryPath = YoutubeInfo.FilePath;

        if (Directory.Exists(directoryPath))
        {
            string[] mp3Files = Directory.GetFiles(directoryPath, "*.mp3");

            foreach (string file in mp3Files)
            {
                try
                {
                    File.Delete(file);
                    Plugin.Logger.LogInfo($"Deleted: {file}");
                }
                catch (IOException ioExp)
                {
                    Plugin.Logger.LogError($"An IO exception occurred: {ioExp.Message}");
                }
                catch (UnauthorizedAccessException unAuthExp)
                {
                    Plugin.Logger.LogError($"An Unauthorized Access exception occurred: {unAuthExp.Message}");
                }
            }
        }
        else
        {
            Plugin.Logger.LogWarning($"The directory {directoryPath} does not exist.");
        }
    }
}
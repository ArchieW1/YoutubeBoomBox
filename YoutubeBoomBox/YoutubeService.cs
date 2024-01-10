using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Converter;
using YoutubeExplode.Search;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace YoutubeBoomBox;

public class YoutubeService
{
    private readonly YoutubeClient _youtubeClient = new();

    public async Task<string> DownloadVideoAudioAsync(string searchQuery)
    {
        YoutubeBoomBoxPlugin.Logger.LogInfo($"Starting video audio download for query: {searchQuery}");

        string url = null;
        await foreach (Batch<ISearchResult> batch in _youtubeClient.Search.GetResultBatchesAsync(searchQuery))
        {
            url = batch.Items.First().Url;
            break;
        }

        if (url is null)
        {
            throw new Exception("No video found");
        }
        
        VideoId videoId = VideoId.Parse(url);
        
        string name = YoutubeInfo.MusicName;
        string fullPath = $"{YoutubeInfo.FilePath}/{name}";
        YoutubeBoomBoxPlugin.Logger.LogInfo($"Downloading to {fullPath}.");
        await _youtubeClient.Videos.DownloadAsync(videoId, fullPath, o => o
            .SetContainer(Container.Mp3)
            .SetPreset(ConversionPreset.UltraFast)
            .SetFFmpegPath($"{YoutubeInfo.FilePath}/ffmpeg"));
        YoutubeBoomBoxPlugin.Logger.LogInfo($"Download complete.");

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
                    YoutubeBoomBoxPlugin.Logger.LogInfo($"Deleted: {file}");
                }
                catch (IOException ioExp)
                {
                    YoutubeBoomBoxPlugin.Logger.LogError($"An IO exception occurred: {ioExp.Message}");
                }
                catch (UnauthorizedAccessException unAuthExp)
                {
                    YoutubeBoomBoxPlugin.Logger.LogError($"An Unauthorized Access exception occurred: {unAuthExp.Message}");
                }
            }
        }
        else
        {
            YoutubeBoomBoxPlugin.Logger.LogWarning($"The directory {directoryPath} does not exist.");
        }
    }
}
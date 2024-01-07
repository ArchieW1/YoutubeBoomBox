using System.IO;

namespace YoutubeBoomBox;

public static class YoutubeInfo
{
    public const string UriBase = "https://www.googleapis.com/youtube/v3/search";
    public const string APIKey = "AIzaSyCiziankYRC3ohO4nZb0jCeuD23ErJ2Fno";

    public static string FilePath { get; } =
        Path.Combine(BepInEx.Paths.PluginPath, $"Unknown-{MyPluginInfo.PLUGIN_NAME}");

    private static int _count = 1;
    public static string MusicName
    {
        get
        {
            string temp = $"{_count}.mp3";
            _count++;
            return temp;
        }
    }
}
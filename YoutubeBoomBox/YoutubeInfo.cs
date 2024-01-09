using System.IO;

namespace YoutubeBoomBox;

public static class YoutubeInfo
{
    public static string FilePath { get; } =
        Path.Combine(BepInEx.Paths.PluginPath, $"Archie-{MyPluginInfo.PLUGIN_NAME}");

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
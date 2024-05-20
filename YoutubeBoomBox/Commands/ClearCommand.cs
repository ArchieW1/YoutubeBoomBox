using YoutubeBoomBox.Download;
using YoutubeBoomBox.Utils;

namespace YoutubeBoomBox.Commands;

public class ClearCommand : CommandBase
{
    protected override string Key { get; } = "/clear";
    protected override void Action(string lastChatMessage)
    {
        AudioQueue.Clear();
        YoutubeService.ClearVideos();
    }
}
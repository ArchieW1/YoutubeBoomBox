namespace YoutubeBoomBox;

public class YoutubeApiResponse
{
    public class Item
    {
        public class ID
        {
            public string videoId { get; set; }
        }

        public ID id { get; set; }
    }

    public Item[] items { get; set; }
}
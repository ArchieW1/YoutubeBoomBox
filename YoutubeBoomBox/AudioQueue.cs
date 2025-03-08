using System.Collections.Generic;

namespace YoutubeBoomBox;

public class AudioQueue
{
    private static readonly List<string> _inputs = [];
    private static int _ptr = 0;

    public static void Add(string input)
    {
        if (input.Length == 11 && !input.Contains(" "))
        {
            _inputs.Add($"https://www.youtube.com/watch?v={input}");
        }
        else
        {
            _inputs.Add($"ytsearch:{input}");
        }
    }

    public static void Clear()
    {
        _inputs.Clear();
    }

    public static bool IsEmpty()
    {
        return _inputs.Count == 0;
    }

    public static string Next()
    {
        string temp = _inputs[_ptr];
        _ptr = (_ptr + 1) % _inputs.Count; // Loops
        return temp;
    }
}
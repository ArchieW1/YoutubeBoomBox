using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace YoutubeBoomBox;

public static class AudioQueue
{
    [CanBeNull] public static AudioClip[] DefaultAudios { get; set; }
    private static List<AudioClip> _clips = [];
    private static int _fp;
    
    public static void Clear()
    {
        _clips.Clear();
    }

    public static bool IsEmpty()
    {
        return _clips.Count == 0;
    }

    public static void Add(AudioClip clip)
    {
        _clips.Add(clip);
    }

    public static AudioClip Get()
    {
        if (_fp >= _clips.Count)
        {
            _fp = 0;
        }
        AudioClip clip = _clips[_fp];
        _fp++;
        return clip;
    }

    public static int Length()
    {
        return _clips.Count;
    }
}
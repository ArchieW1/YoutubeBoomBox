using HarmonyLib;
using YoutubeBoomBox.Utils;

namespace YoutubeBoomBox;

[HarmonyPatch(typeof(BoomboxItem))]
public class BoomboxItemPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(BoomboxItem.ItemActivate))]
    public static void SettingAudioPatch(bool used, BoomboxItem __instance)
    {
        if (!__instance.isPlayingMusic)
        {
            if (AudioQueue.DefaultAudios is null)
            {
                AudioQueue.DefaultAudios = __instance.musicAudios;
            }
        
            if (AudioQueue.IsEmpty())
            {
                __instance.musicAudios = AudioQueue.DefaultAudios;
            }
            else
            {
                __instance.musicAudios = new[] {AudioQueue.Get()};
            }
        }
    }
}
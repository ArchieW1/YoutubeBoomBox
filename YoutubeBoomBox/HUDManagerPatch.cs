using HarmonyLib;
using YoutubeBoomBox.Commands;

namespace YoutubeBoomBox;

[HarmonyPatch(typeof(HUDManager))]
public class HUDManagerPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(BoomboxItem.Start))]
    public static void AddCommandsToInit(BoomboxItem __instance)
    {
        __instance.gameObject.AddComponent<AddCommand>();
        __instance.gameObject.AddComponent<ClearCommand>();
    }
}
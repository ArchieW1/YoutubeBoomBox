using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace YoutubeBoomBox;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class YoutubeBoomBoxPlugin : BaseUnityPlugin
{
    private readonly Harmony _harmony = new(MyPluginInfo.PLUGIN_GUID);
    
    public new static ManualLogSource Logger { get; } = BepInEx.Logging.Logger.CreateLogSource(MyPluginInfo.PLUGIN_NAME);
    
    private void Awake()
    {
        _harmony.PatchAll(typeof(YoutubeBoomBoxPlugin));
        
        _harmony.PatchAll(typeof(BoomboxItemPatch));
        Logger.LogInfo($"Patch {typeof(BoomboxItemPatch)} is loaded!");
        
        _harmony.PatchAll(typeof(HUDManagerPatch));
        Logger.LogInfo($"Patch {typeof(HUDManagerPatch)} is loaded!");
        
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }
}
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace YoutubeBoomBox;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private readonly Harmony _harmony = new(PluginInfo.PLUGIN_GUID);

    public new static readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(PluginInfo.PLUGIN_NAME);
    
    private void Awake()
    {
        _harmony.PatchAll(typeof(Plugin));
        
        _harmony.PatchAll(typeof(HUDManagerPatch));
        Logger.LogInfo($"Patch {typeof(HUDManagerPatch)} is loaded!");
        
        _harmony.PatchAll(typeof(BoomboxItemPatch));
        Logger.LogInfo($"Patch {typeof(BoomboxItemPatch)} is loaded!");
        
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }
}
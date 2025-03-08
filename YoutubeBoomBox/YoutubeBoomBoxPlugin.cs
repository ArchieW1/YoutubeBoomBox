using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace YoutubeBoomBox;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
public class YoutubeBoomBoxPlugin : BaseUnityPlugin
{
    private const string PLUGIN_GUID = "YoutubeBoomBox";
    private const string PLUGIN_NAME = "YoutubeBoomBox";
    private const string PLUGIN_VERSION = "1.1.0";

    private readonly Harmony _harmony = new(MyPluginInfo.PLUGIN_GUID);
    
    public new static ManualLogSource Logger { get; } = BepInEx.Logging.Logger.CreateLogSource(MyPluginInfo.PLUGIN_NAME);
    
    private void Awake()
    {        
        _harmony.PatchAll(typeof(Patches));
        
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }
}
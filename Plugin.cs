using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace EjectCallouts;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("Void Crew.exe")]
[BepInDependency("VoidManager")]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource log;

    private void Awake()
    {
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        Configs.Load(this);

        //Logging Info
        log = Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!"); 
    }
}

public static class MyPluginInfo
{
    public const string PLUGIN_GUID = "Gruncle.EjectCallouts";
    public const string PLUGIN_NAME = "EjectCallouts";
    public const string PLUGIN_VERSION = "1.0.0";
}
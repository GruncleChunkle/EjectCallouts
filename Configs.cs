using BepInEx.Configuration;

namespace EjectCallouts
{
    internal class Configs
    {
        //Defines Config Entry
        internal static ConfigEntry<bool> funnyMessages;
        internal static ConfigEntry<bool> sendToAll;
        internal static ConfigEntry<Commands.CommandMode> commandsOn;

        //If config file doesn't exist it will create it with this category, label, value; If it exists then it references the previous value
        internal static void Load(Plugin plugin)
        {
            funnyMessages = plugin.Config.Bind("EjectCallouts", "funnyMessages", false);
            sendToAll = plugin.Config.Bind("EjectCallouts", "sendToAll", true);
            commandsOn = plugin.Config.Bind("EjectCallouts", "commandMode", Commands.CommandMode.ITEMSONLY);
        }
    }
}

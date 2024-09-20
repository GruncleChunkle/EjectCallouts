using BepInEx.Configuration;

namespace EjectCallouts
{
    internal class Configs
    {
        //Defines Config Entry
        internal static ConfigEntry<bool> funnyMessages;
        internal static ConfigEntry<bool> sendToAll;
        internal static ConfigEntry<Commands.CommandMode> commandsOn;
        internal static ConfigEntry<Commands.communicationShip> communicationShip;

        //internal static ConfigEntry<string[]> 

        //Omission Configs
        internal static ConfigEntry<bool> nanoOmit;       //Nano Alloy
        internal static ConfigEntry<bool> bioOmit;        //Biomass
        internal static ConfigEntry<bool> genericOmit;    //Generic Items[Hull Repair Plates + Beacon]
        internal static ConfigEntry<bool> ammoOmit;       //Ammo
        internal static ConfigEntry<bool> weaponOmit;     //[Just] Composite Weapons
        internal static ConfigEntry<bool> crateOmit;      //Crates[Blank and Filled]
        internal static ConfigEntry<bool> modOmit;        //Mods
        internal static ConfigEntry<bool> shardOmit;      //Data Shards
        internal static ConfigEntry<bool> classOmit;      //Class Specific Items[Ship System Upgrader / Oxygenated Blood Pack]
        internal static ConfigEntry<bool> relicOmit;      //Relics
        internal static ConfigEntry<bool> homunculusOmit; //Homunculus



        //If config file doesn't exist it will create it with this category, label, value; If it exists then it references the previous value
        internal static void Load(Plugin plugin)
        {
            //Settings Data
            funnyMessages = plugin.Config.Bind("EjectCallouts", "funnyMessages", false);
            sendToAll = plugin.Config.Bind("EjectCallouts", "sendToAll", true);
            commandsOn = plugin.Config.Bind("EjectCallouts", "commandMode", Commands.CommandMode.ITEMSONLY);

            //Omission Data
            nanoOmit = plugin.Config.Bind("EjectCallouts", "nanoOmit", false);
            bioOmit = plugin.Config.Bind("EjectCallouts", "bioOmit", false);
            genericOmit = plugin.Config.Bind("EjectCallouts", "genericOmit", false);
            ammoOmit = plugin.Config.Bind("EjectCallouts", "ammoOmit", false);
            weaponOmit = plugin.Config.Bind("EjectCallouts", "weaponOmit", false);
            crateOmit = plugin.Config.Bind("EjectCallouts", "crateOmit", false);
            modOmit = plugin.Config.Bind("EjectCallouts", "modOmit", false);
            shardOmit = plugin.Config.Bind("EjectCallouts", "shardOmit", false);
            classOmit = plugin.Config.Bind("EjectCallouts", "classOmit", false);
            relicOmit = plugin.Config.Bind("EjectCallouts", "relicOmit", false);
            homunculusOmit = plugin.Config.Bind("EjectCallouts", "homunculusOmit", false);

            communicationShip = plugin.Config.Bind("EjectCallouts", "communucationShip", Commands.communicationShip.BOTH);
        }
    }
}

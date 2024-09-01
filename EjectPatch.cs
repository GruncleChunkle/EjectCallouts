using BepInEx.Logging;
using Client.Utils;
using Gameplay.Carryables;
using HarmonyLib;
using System;
using UI.PlayerProfile;
using VoidManager;
using VoidManager.MPModChecks;

namespace EjectCallouts
{
    //This is gonna do player, on TryReleaseCarryable. Targeting info for Harmony
    [HarmonyPatch(typeof(Carrier), "TryEjectCarryable")]
    internal class EjectPatch
    {
        internal static bool HostPermission = false;

        //Runs when the event of a player joining the lobby is run
        internal static void getPermission(object _, Events.PlayerEventArgs eve)
        {
            //If the host has the mod installed
            if (eve.player.IsMasterClient && MPModCheckManager.Instance.NetworkedPeerHasMod(eve.player, MyPluginInfo.PLUGIN_GUID))
            {
                //Give them permission
                HostPermission = true;
            }
        }

        internal static void hostChange(object _, Events.PlayerEventArgs eve)
        {
            //Are you the host? Then give yourself permission
            if(eve.player.IsLocal) 
            { 
                HostPermission = true;
                return;
            }
            //If you are not, ask for permission.
            getPermission(_, eve);
        }

        private static readonly Random rand = new();

        //User wants to discard an item.
        static void Prefix(Carrier __instance)
        {

            //Get Item that we are holding
            string itemName = __instance.Payload.DisplayName;

            //Remove Animus Crate
            if(itemName.Contains("Animus Crate:")) 
            {
                string[] itemNames = itemName.Split(':');
                itemName = itemNames[1];
            }
            itemName.Trim();

            //Assess Command status, do they want callouts for when they throw?
            switch (Configs.commandsOn.Value)
            {
                case Commands.CommandMode.ITEMSONLY:
                    if(Configs.funnyMessages.Value)
                    {
                        //Display
                        VoidManager.Utilities.Messaging.Echo(getFunnyPhrase(itemName, rand.Next(funnyItemListSize)), !(Configs.sendToAll.Value && HostPermission));
                    }
                    else
                    {
                        VoidManager.Utilities.Messaging.Echo($"Dropped: {itemName}", !(Configs.sendToAll.Value && HostPermission));
                    }
                    break;
                case Commands.CommandMode.NOTIFICATIONSONLY:
                    if(Configs.funnyMessages.Value)
                    {
                        //Display
                        VoidManager.Utilities.Messaging.Echo(getFunnyPhrase(rand.Next(funnyNoteListSize)), !(Configs.sendToAll.Value && HostPermission));
                    }
                    else
                    {
                        VoidManager.Utilities.Messaging.Echo($"Hey, I dropped an Item!", !(Configs.sendToAll.Value && HostPermission));
                    }
                    
                    break;
                default:
                    break;
            }
        }

        //Funny Phrases that need an Item in the sentence, are gotten from this Method.
        private static readonly int funnyItemListSize = 9;
        private static string getFunnyPhrase(string ItemName, int num)
        {
            switch (num)
            {
                case 0:
                    return $"What is {ItemName}? Well, its over here.";
                case 1:
                    return $"I FOUND {ItemName.ToUpper()}! Why was it over there anyway?";
                case 2:
                    return $"OMG {ItemName.ToUpper()}, I definitely needed this.";
                case 3:
                    return $"{ItemName}, who are you to tell me I needed you.";
                case 4:
                    return $"BEGONE, {ItemName.ToUpper()}";
                case 5:
                    return $"I wonder how they got {ItemName} to be so heavy.";
                case 6:
                    return $"I'd love to fight a Hollow Reclaimer with {ItemName}.";
                case 7:
                    return $"METEM PRESERVE {ItemName.ToUpper()}.";
                case 8:
                    return $"I tried to eat {ItemName}, it was too spicy for my taste.";
                default: 
                    break;
            }
            return $"Whoops Wrong Number: {num}";
        }

        //Funny Phrases that don't need an Item in the sentence, are gotten from this Method.
        private static readonly int funnyNoteListSize = 9;
        private static string getFunnyPhrase(int num)
        {
            switch (num)
            {
                case 0:
                    return "I dropped some of my biomass, I mean an Item.";
                case 1:
                    return "HEY YOU, GET THAT ITEM PLEASE. I dropped it...";
                case 2:
                    return "Idk why I had this... better in your hands, I guess.";
                case 3:
                    return "Are you wanting this? Cause I don't need it.";
                case 4:
                    return "Oh god, I need to wash my hands.";
                case 5:
                    return "Do you need to hear the word of METEM?";
                case 6:
                    return "Maybe this item goes here. Hmm...";
                case 7:
                    return "A beauty this is to behold. An Item!";
                case 8:
                    return "This item is gonna solo the Reclaimer, I can feel it!";
                default:
                    break;
            }
            return $"Whoops Wrong Number: {num}";
        }
    }
}

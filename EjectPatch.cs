using Gameplay.Carryables;
using HarmonyLib;
using System;
using System.Collections.Generic;
using VoidManager;
using VoidManager.MPModChecks;

namespace EjectCallouts
{
    //This is gonna do player, on TryReleaseCarryable. Targeting info for Harmony
    [HarmonyPatch(typeof(Carrier), "TryEjectCarryable")]
    internal class EjectPatch
    {

        //We need the name and tier of the item.
        private static string itemName = "";
        private static int itemTier = -1;

        //Funny Messages
        internal static string[] funnyMessagesItem =
        {
            $"What is *? Well, its over here.",
            $"I FOUND *! Why was it over there anyway?",
            $"OMG *, I definitely needed this.",
            $"*, who are you to tell me I needed you.",
            $"BEGONE, *",
            $"I wonder how they got * to be so heavy.",
            $"I'd love to fight a Hollow Reclaimer with *.",
            $"METEM PRESERVE *.",
            $"I tried to eat *, it was too spicy for my taste."
        };

        //Funny Messages with no item reference
        internal static string[] funnyMessages =
        {
            "I dropped some of my biomass, I mean an Item.",
            "HEY YOU, GET THAT ITEM PLEASE. I dropped it...",
            "Idk why I had this... better in your hands, I guess.",
            "Are you wanting this? Cause I don't need it.",
            "Oh god, I need to wash my hands.",
            "Do you need to hear the word of METEM?",
            "Maybe this item goes here. Hmm...",
            "A beauty this is to behold. An Item!",
            "This item is gonna solo the Reclaimer, I can feel it!"
        };


        private static readonly Random rand = new();

        //User wants to discard an item.
        static void Prefix(Carrier __instance)
        {
            //Get Item that we are holding / remove animus crate from the name
            itemName = __instance.Payload.DisplayName;
            if (itemName.Contains("Animus Crate:") || itemName.Contains("Data Shard:"))
            {
                removePrefix(ref itemName);
            }
            itemName.TrimStart();
            itemName.TrimEnd();
            //Find out what tier the item is. If it has one.
            itemTier = itemNameToTier(itemName);
        }

        
        static void Postfix(Carrier __instance)
        {
            //If the player is using debug mode, then print the debug info and ignore the rest
            if (Commands.debugEjectCallouts)
            {
                printDebugInfo(__instance);
                return;
            }

            //Checks if the name enters one of these categories, if the omission value is true, then reset the name of the item and ignore the rest of the code.
            if (needsOmission(itemName, nanoAlloyNames, Configs.nanoOmit.Value)) { resetItemNameTier(); return; }
            if (needsOmission(itemName, biomassNames, Configs.bioOmit.Value)) { resetItemNameTier(); return; }
            if (needsOmission(itemName, genericItemNames, Configs.genericOmit.Value)) { resetItemNameTier(); return; }
            if (needsOmission(itemName, ammoNames, Configs.ammoOmit.Value)) { resetItemNameTier(); return; }
            if (needsOmission(itemName, animusCrateNames, Configs.crateOmit.Value)) { resetItemNameTier(); return; }
            if (needsOmission(itemName, weaponNames, Configs.weaponOmit.Value)) { resetItemNameTier(); return; }
            if (needsOmission(itemName, modNames, Configs.modOmit.Value)) { resetItemNameTier(); return; }
            if (needsOmission(itemName, classItemNames, Configs.classOmit.Value)) { resetItemNameTier(); return; }
            if (needsOmission(itemName, shardNames, Configs.shardOmit.Value)) { resetItemNameTier(); return; }
            if (needsOmission(itemName, relicNames, Configs.relicOmit.Value)) { resetItemNameTier(); return; }
            if (needsOmission(itemName, "Homunculus", Configs.homunculusOmit.Value)) { resetItemNameTier(); return; }

            
            //If the payload is outside the ship and the user wants inside only
            if (Configs.communicationShip.Value == Commands.communicationShip.INSIDEONLY && __instance.Owner.IsInSpace)
            {
                resetItemNameTier();
                return;
            }
            //If the payload is inside the ship and the user wants outside only
            else if (Configs.communicationShip.Value == Commands.communicationShip.OUTSIDEONLY && !__instance.Owner.IsInSpace)
            {
                resetItemNameTier();
                return;
            }

            //Assess Command status, do they want callouts for when they throw?
            switch (Configs.commandsOn.Value)
            {
                case Commands.CommandMode.ITEMSONLY:
                    if (Configs.funnyMessages.Value)
                    {
                        //Gets a random number
                        int randomNumber = rand.Next(funnyMessagesItem.Length);

                        //Gets the phrase with the item in it
                        string realPhrase = combinePhrase(splitPhrase(funnyMessagesItem[randomNumber]), itemName);

                        //Display to the text chat
                        VoidManager.Utilities.Messaging.Echo(realPhrase, !(Configs.sendToAll.Value && VoidManagerPlugin.sessionFeaturesEnabled));
                    }
                    else
                    {
                        VoidManager.Utilities.Messaging.Echo($"Dropped: {itemName}", !(Configs.sendToAll.Value && VoidManagerPlugin.sessionFeaturesEnabled));
                    }
                    break;
                case Commands.CommandMode.NOTIFICATIONSONLY:
                    if (Configs.funnyMessages.Value)
                    {
                        //Display
                        VoidManager.Utilities.Messaging.Echo(funnyMessages[rand.Next(funnyMessages.Length)], !(Configs.sendToAll.Value && VoidManagerPlugin.sessionFeaturesEnabled));
                    }
                    else
                    {
                        VoidManager.Utilities.Messaging.Echo($"Hey, I dropped an Item!", !(Configs.sendToAll.Value && VoidManagerPlugin.sessionFeaturesEnabled));
                    }
                    break;
                default:
                    break;
            }

            //Reset ItemName
            resetItemNameTier();
        }

        //Tells which items need to be omitted in each category
        private readonly static string[] animusCrateNames =
        {
            "Power Generator",
            "Gravity Scoop",
            "Arc Shield",
            "Kinetic Point Defense",
            "Life Support", 
            "Thurster Booster Station",
            "Charge Station",
            "Blank Animus Crate"
        };
        private readonly static string[] genericItemNames =
        {
            "Hull Repair Plate",
            "Decoy Signature Lure"
        };
        private readonly static string[] ammoNames =
        {
            "Power Cell",
            "Light Caliber",
            "Heavy Caliber"
        };

        /*
         "Ancestral Core"
         "Meat Chassis Charity"
         "Celestial Encirclement"
         "All Things Sideways"
         "Blessed Station of Thrusters"
         "Sanctuary of Power"
         "Rookie Support"
         "Heat Flipper"
         "In the Name of Metem"
         "Possessed Momentum"
         */

        private readonly static string[] relicNames =
        {
            "The Lone Ectype",
            "Starboard Blessing",
            "Port Blessing",
            "Sacred Power Conduit",
            "Void-powered Shell",
            "Forward Emissary",
            "Aftshock Rebuke",
            "Blessed Bossters",
            "Energy Ascension",
            "Kinetic Ascension",
            "Astral Gravity Well",
            "Point Defence Ascension",
            "Benediction Fury",
            "Carronade Psalm",
            "Litany Bullet Hymn",
            "Recusers Lament",
            "Heat Tempered Aegis",
            "Close-quarters",
            "Bullet Storm",
            "Energy Overload",
            "Drive By Shooting",
        };
        private readonly static string[] biomassNames =
        {
            "Biomass Canister"
        };
        private readonly static string[] nanoAlloyNames =
        {
            "Nano Alloy Cluster"
        };
        private readonly static string[] modNames =
        {
            "Heatsink",
            "Reload Optimizer",
            "Accuracy",
            "Damage",
            "Power Optimizer",
            "Zoom",
            "Fire Rate",
            "Close Quarter", 
            "Sniper Protocol",
            "Relentless Assault",
            "High Damage"
        };
        private readonly static string[] shardNames =
        {
            "Ancient Data Shard",
            "Minefield",
            "Escort",
            "Quantum Data Shard"
        };
        private readonly static string[] weaponNames = 
        { 
            "Composite Weapon"
        };
        private readonly static string[] classItemNames =
        {
            "Ship System Upgrader MkI",
            "Oxygenated Blood Pack"
        };

        //Checks to see if name is in list.
        private static bool isInList(string name, string[] list)
        {
            foreach (string s in list)
            {
                if (name.Contains(s)) { return true; }
            }
            return false;
        }

        //Checks to see if the item needs to be omitted using an array, uses isInList.
        private static bool needsOmission(string name, string[] list, bool b)
        {
            if(b && isInList(name, list))
            {
                return true;
            }
            return false;
        }

        //Checks to see if the item needs to be omitted using a single string.
        private static bool needsOmission(string name, string list, bool b)
        {
            if (b && name.Contains(list))
            {
                return true;
            }
            return false;
        }

        //Simple methods

        private static void resetItemNameTier()
        {
            itemName = "";
            itemTier = -1;
        }

        //Prints to the player only (for ease of use in logging to chat)
        private static void printPrivately(string text)
        {
            VoidManager.Utilities.Messaging.Echo(text, true);
        }

        //Prints Debug Information
        private static void printDebugInfo(Carrier instance)
        {
            VoidManager.Utilities.Messaging.Echo($"===================================", true);
            VoidManager.Utilities.Messaging.Echo($"Player Name: {instance.Owner.DisplayName}", true);
            VoidManager.Utilities.Messaging.Echo($"Item Name: {itemName}", true);
            VoidManager.Utilities.Messaging.Echo($"Send To All Value: {Configs.sendToAll.Value}", true);
            VoidManager.Utilities.Messaging.Echo($"Funny Message Value: {Configs.funnyMessages.Value}", true);
            VoidManager.Utilities.Messaging.Echo($"Command Type: {Configs.commandsOn.Value}", true);
            VoidManager.Utilities.Messaging.Echo($"Ship Setting: {Configs.communicationShip.Value}", true);
            VoidManager.Utilities.Messaging.Echo($"Host Permission / Payload in Space: {VoidManagerPlugin.sessionFeaturesEnabled} / {Commands.boolToString(instance.Owner.IsInSpace)}", true);
            VoidManager.Utilities.Messaging.Echo($"===================================", true);
        }

        //Check the name to see if the item has a tier
        private static int itemNameToTier(string itemN)
        {
            if (itemN == null || itemN.Length < 4) { return -1; }
            if(itemN.Contains("MkIII"))
            {
                return 3;
            }
            else if(itemN.Contains("MkII"))
            {
                return 2;
            }
            else if(itemN.Contains("MkI"))
            {
                return 1;
            }
            return -1;
        }

        //Remove Animus Crate from the name
        private static void removePrefix(ref string itemN)
        {
            string[] itemtemp = itemName.Split(':');
            itemN = itemtemp[1];
        }

        //Splits the phrase by the * character, * = itemName Position
        private static string[] splitPhrase(string phrase)
        {
            return phrase.Split('*');
        }

        //Combines the itemName string in between every string in phrase
        private static string combinePhrase(string[] phrase, string ItemName)
        {
            //Initialize
            string parts = "";
            int y = 0;

            //Flip flop between phrase[y] and ItemName adding them to parts
            for(int i = 0; i < (phrase.Length * 2 - 1); i++)
            {
                if(i % 2 == 0)
                {
                    parts += phrase[y];
                    y++;
                } 
                else
                {
                    parts += ItemName;
                }
            }
            return parts;
        }


    }
}

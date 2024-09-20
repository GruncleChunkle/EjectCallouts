using System.Collections.Generic;
using VoidManager.Chat.Router;

namespace EjectCallouts
{
    internal class Commands : ChatCommand
    {
        //Possible Status for CommandsOn variable.
        public enum CommandMode {
            NULL,
            NOTIFICATIONSONLY,
            ITEMSONLY
        }

        public enum communicationShip
        {
            INSIDEONLY,
            OUTSIDEONLY,
            BOTH
        }

        //Command Name
        public override string[] CommandAliases()
        {
            return ["ejc", "ejectcallouts"];
        }

        //Command Description 
        public override string Description()
        {
            return "Changes Eject Callout Status.";
        }

        //Just to put in omission arguements
        private readonly static string[] omitArguments = { "nano", "bio", "generic", "ammo", "weapons", "mods", "crates", "shards", "class", "relic", "homun" };

        //Autofills arguments 
        public override List<Argument> Arguments()
        {
            return [
                new Argument("item", "note", "null"),
                new Argument("local", new Argument("false", "true")),
                new Argument("funny", new Argument("false", "true")),
                new Argument("help", new Argument("%integer")),
                new Argument("omit", new Argument(omitArguments, new Argument("%integer")))
            ];
        }

        //Hidden from the configs and always set to false to avoid players using
        public static bool debugEjectCallouts = false;

        //Executes the code when the command is typed
        public override void Execute(string arguments)
        {
            string[] args = arguments.ToLower().Split(' ');

            //Check the first argument in the string array
            switch (args[0])
            {
                //If the user wants only the item's name to be called out
                case "item":
                    Configs.commandsOn.Value = CommandMode.ITEMSONLY;
                    VoidManager.Utilities.Messaging.Echo("Callouts Set to Items Only.", true);
                    break;
                //If the user wants only their name to be called out
                case "note":
                    Configs.commandsOn.Value = CommandMode.NOTIFICATIONSONLY;
                    VoidManager.Utilities.Messaging.Echo("Callouts Set to Notifications Only.", true);
                    break;
                //If the user doesn't want any callouts
                case "null":
                    Configs.commandsOn.Value = CommandMode.NULL;
                    VoidManager.Utilities.Messaging.Echo("Callouts Set to No Callouts.", true);
                    break;
                //Checks if they want the messages to only be local
                case "local":
                    if (args.Length < 2) //Checks Status
                    {
                        if (!Configs.sendToAll.Value) { VoidManager.Utilities.Messaging.Echo("Locality Currenty Set to: Only you can hear.", true); }
                        else { VoidManager.Utilities.Messaging.Echo("Locality Currenty Set to: Everyone can hear.", true); }
                    }
                    else if (args[1].Equals("true")) //Turns to Local Callouts
                    {
                        Configs.sendToAll.Value = false;
                        VoidManager.Utilities.Messaging.Echo("Callouts Set to Only you hear.", true);
                    }
                    else if (args[1].Equals("false")) //Turns to Server Callouts
                    {
                        Configs.sendToAll.Value = true;
                        VoidManager.Utilities.Messaging.Echo("Callouts Set to Everyone hear.", true);
                    }
                    else
                    {
                        VoidManager.Utilities.Messaging.Echo("[Invalid Command]: type \"/ejc help 3\", for more information", true);
                    }
                    break;
                //Checks if they want the callouts to be funny.
                case "funny":
                    if (args.Length < 2) //Checks status
                    {
                        if (Configs.funnyMessages.Value) { VoidManager.Utilities.Messaging.Echo("Callout Type: Funny :)", true); }
                        else { VoidManager.Utilities.Messaging.Echo("Callout Type: Serious", true); }
                    }
                    else if (args[1].Equals("true")) //Turns to Funny
                    {
                        Configs.funnyMessages.Value = true;
                        VoidManager.Utilities.Messaging.Echo("Set Callouts to Funny :)", true);
                    }
                    else if (args[1].Equals("false")) //Turns to Serious
                    {
                        Configs.funnyMessages.Value = false;
                        VoidManager.Utilities.Messaging.Echo("Set Callouts to Serious", true);
                    }
                    else
                    {
                        VoidManager.Utilities.Messaging.Echo("[Invalid Command]: Invalid Callout Type number.", true);
                    }
                    break;
                //In case the user needs help to understand the callouts command system.
                case "help":
                    if (args.Length < 2 || args[1].Equals("1"))
                    {
                        //Goes over the help pages
                        printHelpPage(helpPage1);
                    }
                    else if (args[1].Equals("2"))
                    {
                        //Help 2 Talks about the commands
                        printHelpPage(helpPage2);
                    }
                    else if (args[1].Equals("3"))
                    {
                        //Help 3 Talks about the local command
                        printHelpPage(helpPage3);
                    }
                    else if (args[1].Equals("4"))
                    {
                        //Help 4 Talks about the alternative command notation
                        printHelpPage(helpPage4);
                    }
                    else if (args[1].Equals("5"))
                    {
                        //Help 5 Talks about the funny command
                        printHelpPage(helpPage5);
                    }
                    else if (args[1].Equals("6"))
                    {
                        //Help 6 Talks about the omission command
                        printHelpPage(helpPage6);
                    }
                    else if (args[1].Equals("7"))
                    {
                        //Help 7 Talks about the omission types pt.1
                        printHelpPage(helpPage7);
                    }
                    else if (args[1].Equals("8"))
                    {
                        //Help 8 Talks about the omission types pt.2
                        printHelpPage(helpPage8);
                    }
                    else if (args[1].Equals("9"))
                    {
                        //Help 9 Talks about the omission types pt.3
                        printHelpPage(helpPage9);
                    }
                    else if (args[1].Equals("10"))
                    {
                        //Help 10 Talks about the omission types pt.4
                        printHelpPage(helpPage10);
                    }
                    else
                    {
                        VoidManager.Utilities.Messaging.Echo("[Invalid Command]: Invalid Help page number. [Max 10]", true);
                    }
                    break;
                //Checks if the user wants to omit a certain group of objects
                case "omit":
                    if (args.Length < 2) {
                        VoidManager.Utilities.Messaging.Echo("[Invalid Command]: Requires Additional arguments.");
                        break; 
                    }

                    string text = "";
                    //Check the second arguments
                    switch (args[1]) {
                        case "nano":
                        case "alloy":
                        case "nanoalloy":
                            text = "Nano Alloy";
                            if(args.Length < 3)
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: {boolToString(Configs.nanoOmit.Value)}", true);
                            }
                            else if (args[2].Equals("1"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to True", true);
                                Configs.nanoOmit.Value = true;
                            }
                            else if (args[2].Equals("0"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to False", true);
                                Configs.nanoOmit.Value = false;
                            }
                            else 
                            {
                                VoidManager.Utilities.Messaging.Echo($"[Invalid Command]: Omissions [{text}] only allows 0 or 1", true);
                            }
                            break;
                        case "biomass":
                        case "mass":
                        case "bio":
                            text = "Biomass";
                            if (args.Length < 3)
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: {boolToString(Configs.bioOmit.Value)}", true);
                            }
                            else if (args[2].Equals("1"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to True", true);
                                Configs.bioOmit.Value = true;
                            }
                            else if (args[2].Equals("0"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to False", true);
                                Configs.bioOmit.Value = false;
                            }
                            else
                            {
                                VoidManager.Utilities.Messaging.Echo($"[Invalid Command]: Omissions [{text}] only allows 0 or 1", true);
                            }
                            break;
                        case "genericitems":
                        case "generic":
                            text = "Generic Items";
                            if (args.Length < 3)
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: {boolToString(Configs.genericOmit.Value)}", true);
                            }
                            else if (args[2].Equals("1"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to True", true);
                                Configs.genericOmit.Value = true;
                            }
                            else if (args[2].Equals("0"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to False", true);
                                Configs.genericOmit.Value = false;
                            }
                            else
                            {
                                VoidManager.Utilities.Messaging.Echo($"[Invalid Command]: Omissions [{text}] only allows 0 or 1", true);
                            }
                            break;
                        case "ammo":
                        case "weaponammo":
                            text = "Weapon Ammo";
                            if (args.Length < 3)
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: {boolToString(Configs.ammoOmit.Value)}", true);
                            }
                            else if (args[2].Equals("1"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to True", true);
                                Configs.ammoOmit.Value = true;
                            }
                            else if (args[2].Equals("0"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to False", true);
                                Configs.ammoOmit.Value = false;
                            }
                            else
                            {
                                VoidManager.Utilities.Messaging.Echo($"[Invalid Command]: Omissions [{text}] only allows 0 or 1", true);
                            }
                            break;
                        case "weapons":
                        case "composite":
                            text = "Composite Weapons";
                            if (args.Length < 3)
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: {boolToString(Configs.weaponOmit.Value)}", true);
                            }
                            else if (args[2].Equals("1"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to True", true);
                                Configs.weaponOmit.Value = true;
                            }
                            else if (args[2].Equals("0"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to False", true);
                                Configs.weaponOmit.Value = false;
                            }
                            else
                            {
                                VoidManager.Utilities.Messaging.Echo($"[Invalid Command]: Omissions [{text}] only allows 0 or 1", true);
                            }
                            break;
                        case "mods":
                        case "weaponmods":
                            text = "Weapon Mods";
                            if (args.Length < 3)
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: {boolToString(Configs.modOmit.Value)}", true);
                            }
                            else if (args[2].Equals("1"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to True", true);
                                Configs.modOmit.Value = true;
                            }
                            else if (args[2].Equals("0"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to False", true);
                                Configs.modOmit.Value = false;
                            }
                            else
                            {
                                VoidManager.Utilities.Messaging.Echo($"[Invalid Command]: Omissions [{text}] only allows 0 or 1", true);
                            }
                            break;
                        case "crates":
                        case "animus":
                        case "animuscrates":
                            text = "Animus Crates";
                            if (args.Length < 3)
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: {boolToString(Configs.crateOmit.Value)}", true);
                            }
                            else if (args[2].Equals("1"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to True", true);
                                Configs.crateOmit.Value = true;
                            }
                            else if (args[2].Equals("0"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to False", true);
                                Configs.crateOmit.Value = false;
                            }
                            else
                            {
                                VoidManager.Utilities.Messaging.Echo($"[Invalid Command]: Omissions [{text}] only allows 0 or 1", true);
                            }
                            break;
                        case "datashards":
                        case "shards":
                        case "data":
                            text = "Data Shards";
                            if (args.Length < 3)
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: {boolToString(Configs.shardOmit.Value)}", true);
                            }
                            else if (args[2].Equals("1"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to True", true);
                                Configs.shardOmit.Value = true;
                            }
                            else if (args[2].Equals("0"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to False", true);
                                Configs.shardOmit.Value = false;
                            }
                            else
                            {
                                VoidManager.Utilities.Messaging.Echo($"[Invalid Command]: Omissions [{text}] only allows 0 or 1", true);
                            }
                            break;
                        case "classitems":
                        case "class":
                            text = "Class Items";
                            if (args.Length < 3)
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: {boolToString(Configs.classOmit.Value)}", true);
                            }
                            else if (args[2].Equals("1"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to True", true);
                                Configs.classOmit.Value = true;
                            }
                            else if (args[2].Equals("0"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to False", true);
                                Configs.classOmit.Value = false;
                            }
                            else
                            {
                                VoidManager.Utilities.Messaging.Echo($"[Invalid Command]: Omissions [{text}] only allows 0 or 1", true);
                            }
                            break;
                        case "homunculus":
                        case "homun":
                            text = "Homunculus";
                            if (args.Length < 3)
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: {boolToString(Configs.homunculusOmit.Value)}", true);
                            }
                            else if (args[2].Equals("1"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to True", true);
                                Configs.homunculusOmit.Value = true;
                            }
                            else if (args[2].Equals("0"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to False", true);
                                Configs.homunculusOmit.Value = false;
                            }
                            else
                            {
                                VoidManager.Utilities.Messaging.Echo($"[Invalid Command]: Omissions [{text}] only allows 0 or 1", true);
                            }
                            break;
                        case "relic":
                            text = "Relics";
                            if (args.Length < 3)
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: {boolToString(Configs.relicOmit.Value)}", true);
                            }
                            else if (args[2].Equals("1"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to True", true);
                                Configs.relicOmit.Value = true;
                            }
                            else if (args[2].Equals("0"))
                            {
                                VoidManager.Utilities.Messaging.Echo($"Omission [{text}]: Set to False", true);
                                Configs.relicOmit.Value = false;
                            }
                            else
                            {
                                VoidManager.Utilities.Messaging.Echo($"[Invalid Command]: Omissions [{text}] only allows 0 or 1", true);
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                //For Gruncle Chuck to help problem solve
                case "debug":
                    if (args.Length < 2)
                    {
                        VoidManager.Utilities.Messaging.Echo($"Debug Status: {debugEjectCallouts}", true);
                    }
                    else if (args[1].Equals("1"))
                    {
                        debugEjectCallouts = true;
                        VoidManager.Utilities.Messaging.Echo($"Debug Status: {debugEjectCallouts}", true);
                    }
                    else if (args[1].Equals("0"))
                    {
                        debugEjectCallouts = false;
                        VoidManager.Utilities.Messaging.Echo($"Debug Status: {debugEjectCallouts}", true);
                    }
                    else
                    {
                        VoidManager.Utilities.Messaging.Echo("[Invalid Command]: Input only 1 or 0 for Debug.", true);
                    }
                    break;
                default:
                    VoidManager.Utilities.Messaging.Echo("[Invalid Command]: type \"/ejc help\", for more information", true);
                    break;
            }
        }

        //Takes a boolean value and makes it a string
        public static string boolToString(bool b)
        {
            if(b)
            {
                return "True";
            }
            return "False";
        }

        //Takes in a help page and prints it to the chat box
        public static void printHelpPage(string[] helpPage)
        {
            foreach(string s in helpPage)
            {
                VoidManager.Utilities.Messaging.Echo(s, true);
            }
        }


        //Help Pages for help with commands; For more information check the Execute() function under args[1].Equals("Help"), They will be explained there.
        private readonly static string[] helpPage1 =
        {
            "==============[Eject Callouts: Help Page 1/10]===========",
            "!{NOTICE: It's far easier to use the F5 menu GUI}!",
            "/ejc help [number] to visit pages                     ",
            "You could also use {ejectcallouts}                    ",
            "Example: /ejectcallouts help [number]                 ",
            "=[End Help]=================={Next: Callout Commands}="
        };
        private readonly static string[] helpPage2 =
        {
            "============[Eject Callouts: Help Page 2/10]===========",
            "/ejc [item/note/null]                                 ",
            "[Item] sets to Only Display Items in Callouts         ",
            "[Note] sets to Only Display Player in Callouts        ",
            "[Null] sets to not display anything                   ",
            "Example: \"/ejc item\"                                ",
            "=[End Help]=========================={Next: Locality}="
        };
        private readonly static string[] helpPage3 =
        {
            "============[Eject Callouts: Help Page 3/10]============",
            "/ejc local [number] to change which players hear it.  ",
            "[1], allows only you to hear callouts.                ",
            "[0], tells everyone the callouts.                     ",
            "Example: \"/ejc local 1\"                             ",
            "=[End Help]========{Next: Alternate Command Notation}="
        };
        private readonly static string[] helpPage4 =
        {
            "============[Eject Callouts: Help Page 4/10]===========",
            "Alternate Command Keywords: Wherever [] is you can use",
            "[item]:  \"i\",   \"itemsonly\",    \"2\"             ",
            "[note]:  \"n\",   \"notification\", \"1\"             ",
            "[null]:  \"off\",                   \"0\"             ",
            "[local]: \"l\",   \"locality\",     \"onlyme\"        ",
            "=[End Help]===================={Next: Funny Callouts}="
        };
        private readonly static string[] helpPage5 =
        {
            "============[Eject Callouts: Help Page 5/10]=========",
            "/ejc funny [number] to change to funny callouts       ",
            "[1]: Funny Callouts (Random Lines)                    ",
            "[0]: Serious Callouts (Set Lines)                     ",
            "Example: \"/ejc funny 1\"                             ",
            "=[End Help]========================={Next: Omissions}="
        };
        private readonly static string[] helpPage6 =
        {
            "============[Eject Callouts: Help Page 6/10]===========",
            "/ejc omit [text] [number] to change whether you want",
            "that group of items to be called out when you drop",
            "them.",
            "Example: \"/ejc omit nano 1\", makes sure not to call",
            "out when you drop nano alloy chunks.",
            "=[End Help]================{Next: Omission Types pt.1}="
        };
        private readonly static string[] helpPage7 = 
        {
            "============[Eject Callouts: Help Page 7/10]===========",
            "Omission Types: Pt. 1",
            "/ejc omit nano [0/1], changes nano alloy callouts",
            "/ejc omit bio [0/1], changes biomass callouts",
            "/ejc omit generic [0/1], changes generic items callouts",
            "Generic = Hull repair plates / Beacons",
            "=[End Help]================{Next: Omission Types pt.2}="
        };
        private readonly static string[] helpPage8 =
        {
            "============[Eject Callouts: Help Page 8/10]===========",
            "Omission Types: Pt. 2",
            "/ejc omit ammo [0/1], changes ammo callouts",
            "/ejc omit weapons [0/1], changes weapon crate callouts",
            "/ejc omit mods [0/1], changes weapon mod callouts",
            "/ejc omit crates [0/1], changes animus crate callouts",
            "=[End Help]================{Next: Omission Types pt.3}="
        };
        private readonly static string[] helpPage9 =
        {
            "============[Eject Callouts: Help Page 9/10]===========",
            "Omission Types: Pt. 3",
            "/ejc omit shards [0/1], changes data shard callouts",
            " -Only works on data shards",
            "/ejc omit class [0/1], changes class specific item callouts",
            " -Only applies to ship system upgrader/oxygen infusor",
            "=[End Help]================{Next: Omission Types pt.4}="
        };
        private readonly static string[] helpPage10 =
        {
            "============[Eject Callouts: Help Page 10/10]==========",
            "Omission Types: Pt. 4",
            "/ejc omit homun [0/1], changes homunculus callouts",
            " -Only works on the homunculus",
            "/ejc omit relic [0/1], changes relic callouts",
            " -Only applies to relics",
            "=====================[End Help]========================"
        };

    }
}
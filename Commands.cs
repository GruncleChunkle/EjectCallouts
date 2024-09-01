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

        public override string[] CommandAliases()
        {
            return ["ejc", "ejectcallouts"];
        }

        public override string Description()
        {
            return "Changes Eject Callout Status.";
        }

        public override List<Argument> Arguments()
        {
            return [
                new Argument("item", "note", "null"),
                new Argument("local", new Argument("false", "true")),
                new Argument("funny", new Argument("false", "true")),
                new Argument("help", new Argument("%integer"))
            ];
        }

        public override void Execute(string arguments)
        {
            string[] args = arguments.ToLower().Split(' ');

            //Check the arguments in the string array
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
                    if(args.Length < 2) //Checks Status
                    {
                        if(!Configs.sendToAll.Value) { VoidManager.Utilities.Messaging.Echo("Locality Currenty Set to: Only you can hear.", true); }
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
                        VoidManager.Utilities.Messaging.Echo("==============[Eject Callouts: Help Page 1/5]============", true);
                        VoidManager.Utilities.Messaging.Echo("/ejc help [number] to visit pages                     ", true);
                        VoidManager.Utilities.Messaging.Echo("You could also use {ejectcallouts}                    ", true);
                        VoidManager.Utilities.Messaging.Echo("Example: /ejectcallouts help [number]                 ", true);
                        VoidManager.Utilities.Messaging.Echo("=[End Help]=================={Next: Callout Commands}=", true);
                    } 
                    else if (args[1].Equals("2"))
                    {
                        //Help 2 Talks about the commands
                        VoidManager.Utilities.Messaging.Echo("============[Eject Callouts: Help Page 2/5]============", true);
                        VoidManager.Utilities.Messaging.Echo("/ejc [item/note/null]                                 ", true);
                        VoidManager.Utilities.Messaging.Echo("[Item] sets to Only Display Items in Callouts         ", true);
                        VoidManager.Utilities.Messaging.Echo("[Note] sets to Only Display Player in Callouts        ", true);
                        VoidManager.Utilities.Messaging.Echo("[Null] sets to not display anything                   ", true);
                        VoidManager.Utilities.Messaging.Echo("Example: \"/ejc item\"                                ", true);
                        VoidManager.Utilities.Messaging.Echo("=[End Help]=========================={Next: Locality}=", true);
                    }
                    else if (args[1].Equals("3"))
                    {
                        //Help 3 Talks about the local command
                        VoidManager.Utilities.Messaging.Echo("============[Eject Callouts: Help Page 3/5]=============", true);
                        VoidManager.Utilities.Messaging.Echo("/ejc local [number] to change which players hear it.  ", true);
                        VoidManager.Utilities.Messaging.Echo("[1], allows only you to hear callouts.                ", true);
                        VoidManager.Utilities.Messaging.Echo("[0], tells everyone the callouts.                     ", true);
                        VoidManager.Utilities.Messaging.Echo("Example: \"/ejc local 1\"                             ", true);
                        VoidManager.Utilities.Messaging.Echo("=[End Help]=============={Next: Alt Command Notation}=", true);
                        
                    }
                    else if (args[1].Equals("4"))
                    {
                        //Help 4 Talks about the alternative command notation
                        VoidManager.Utilities.Messaging.Echo("============[Eject Callouts: Help Page 4/5]============", true);
                        VoidManager.Utilities.Messaging.Echo("Alternate Command Keywords: Wherever [] is you can use", true);
                        VoidManager.Utilities.Messaging.Echo("[item]:  \"i\",   \"itemsonly\",    \"2\"             ", true);
                        VoidManager.Utilities.Messaging.Echo("[note]:  \"n\",   \"notification\", \"1\"             ", true);
                        VoidManager.Utilities.Messaging.Echo("[null]:  \"off\",                   \"0\"             ", true);
                        VoidManager.Utilities.Messaging.Echo("[local]: \"l\",   \"locality\",     \"onlyme\"        ", true);
                        VoidManager.Utilities.Messaging.Echo("=[End Help]===================={Next: Funny Callouts}=", true);
                    }
                    else if (args[1].Equals("5"))
                    {
                        //Help 5 Talks about the funny command
                        VoidManager.Utilities.Messaging.Echo("============[Eject Callouts: Help Page 5/5]===========", true);
                        VoidManager.Utilities.Messaging.Echo("/ejc funny [number] to change to funny callouts       ", true);
                        VoidManager.Utilities.Messaging.Echo("[1]: Funny Callouts (Random Lines)                    ", true);
                        VoidManager.Utilities.Messaging.Echo("[0]: Serious Callouts (Set Lines)                     ", true);
                        VoidManager.Utilities.Messaging.Echo("Example: \"/ejc funny 1\"                             ", true);
                        VoidManager.Utilities.Messaging.Echo("====================[End Help]===================", true);
                    }
                    else
                    {
                        VoidManager.Utilities.Messaging.Echo("[Invalid Command]: Invalid Help page number. [Max 5]", true);
                    }
                    break;
                default:
                    VoidManager.Utilities.Messaging.Echo("[Invalid Command]: type \"/ejc help\", for more information", true);
                    break;
            }
        }
    }
}
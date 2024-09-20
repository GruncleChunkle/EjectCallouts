using VoidManager.CustomGUI;
using VoidManager.Utilities;
using static UnityEngine.GUILayout;

namespace EjectCallouts
{
    internal class GUI : ModSettingsMenu
    {
        //Name of the GUI
        public override string Name() => "Eject Callouts";

        //Runs on every frame, draws the GUI.
        public override void Draw()
        {

            //Checkboxes that control the status of the callouts
            Label("Eject Callout Settings: ");

            //Checks to see the type of callout the user wants
            bool itemsonly = Configs.commandsOn.Value == Commands.CommandMode.ITEMSONLY;
            if (GUITools.DrawCheckbox("Set Callouts to Name + Items", ref itemsonly)) 
            {
                if (Configs.commandsOn.Value != Commands.CommandMode.ITEMSONLY) 
                {
                    Configs.commandsOn.Value = Commands.CommandMode.ITEMSONLY; 
                }
            }

            bool noteonly = Configs.commandsOn.Value == Commands.CommandMode.NOTIFICATIONSONLY;
            if (GUITools.DrawCheckbox("Set Callouts to Name Only", ref noteonly))
            {
                if (Configs.commandsOn.Value != Commands.CommandMode.NOTIFICATIONSONLY)
                {
                    Configs.commandsOn.Value = Commands.CommandMode.NOTIFICATIONSONLY;
                }
            }

            bool nothing = Configs.commandsOn.Value == Commands.CommandMode.NULL;
            if (GUITools.DrawCheckbox("Set Callouts to off", ref nothing))
            {
                if (Configs.commandsOn.Value != Commands.CommandMode.NULL)
                {
                    Configs.commandsOn.Value = Commands.CommandMode.NULL;
                }
            }

            Label("");

            //Checks to see if the user wants callouts to activate only when outside/inside or both
            bool inside = Configs.communicationShip.Value == Commands.communicationShip.INSIDEONLY;
            if (GUITools.DrawCheckbox("Callouts Reserved for inside ship", ref inside))
            {
                if (Configs.communicationShip.Value != Commands.communicationShip.INSIDEONLY)
                {
                    Configs.communicationShip.Value = Commands.communicationShip.INSIDEONLY;
                }
            }

            bool outside = Configs.communicationShip.Value == Commands.communicationShip.OUTSIDEONLY;
            if (GUITools.DrawCheckbox("Callouts Reserved for outside ship", ref outside))
            {
                if (Configs.communicationShip.Value != Commands.communicationShip.OUTSIDEONLY)
                {
                    Configs.communicationShip.Value = Commands.communicationShip.OUTSIDEONLY;
                }
            }

            bool both = Configs.communicationShip.Value == Commands.communicationShip.BOTH;
            if (GUITools.DrawCheckbox("Callouts Reserved for inside & outside ship", ref both))
            {
                if (Configs.communicationShip.Value != Commands.communicationShip.BOTH)
                {
                    Configs.communicationShip.Value = Commands.communicationShip.BOTH;
                }
            }

            //Omission Settings
            Label("");
            Label("Omission Settings: ");

            //Checkboxes that control the omissions
            GUITools.DrawCheckbox(": Omit Nano Alloy", ref Configs.nanoOmit);
            GUITools.DrawCheckbox(": Omit Biomass", ref Configs.bioOmit);
            GUITools.DrawCheckbox(": Omit Generic Items", ref Configs.genericOmit);
            GUITools.DrawCheckbox(": Omit Weapon Ammo", ref Configs.ammoOmit);
            GUITools.DrawCheckbox(": Omit Composite Weapons", ref Configs.weaponOmit);
            GUITools.DrawCheckbox(": Omit Animus Crates", ref Configs.crateOmit);
            GUITools.DrawCheckbox(": Omit Weapon Mods", ref Configs.modOmit);
            GUITools.DrawCheckbox(": Omit Data Shards", ref Configs.shardOmit);
            GUITools.DrawCheckbox(": Omit Class Specific Items", ref Configs.classOmit);
            GUITools.DrawCheckbox(": Omit Relics", ref Configs.relicOmit);
            GUITools.DrawCheckbox(": Omit Homunculus", ref Configs.homunculusOmit);

            //Additional Settings 
            Label("");
            Label("Additional Settings: ");

            //Message sent to everyone checkbox
            GUITools.DrawCheckbox("Messages are shown to only self", ref Configs.sendToAll);

            //Messages are funny checkbox
            GUITools.DrawCheckbox("Messages are funny", ref Configs.funnyMessages);

            /*
            if(GUITools.DrawButtonSelected("Set Custom Message", false))
            {
                
            }
            GUITools.DrawTextField("Custom Message: ", );
            */
        }
    }
}

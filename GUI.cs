using VoidManager.CustomGUI;
using VoidManager.Utilities;
using static UnityEngine.GUILayout;

namespace EjectCallouts
{
    internal class GUI : ModSettingsMenu
    {
        public override string Name() => "Eject Callouts";

        public override void Draw()
        {

            //Buttons
            Label("Eject Callout Settings: ");
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
            //Additional Settings 
            Label("Additional Settings: ");

            //Message sent to everyone checkbox
            GUITools.DrawCheckbox("Messages are shown to only self", ref Configs.sendToAll);

            //Messages are funny checkbox
            GUITools.DrawCheckbox("Messages are funny", ref Configs.funnyMessages);
        }
    }
}

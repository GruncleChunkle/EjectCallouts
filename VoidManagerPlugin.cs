using VoidManager;
using VoidManager.MPModChecks;

namespace EjectCallouts
{
    internal class VoidManagerPlugin : VoidPlugin
    {
        public override string Author => "Gruncle Chuck";

        public override string Description => MyPluginInfo.PLUGIN_DESCRIPTION;

        public override MultiplayerType MPType => MultiplayerType.Client;

        internal static bool sessionFeaturesEnabled = false;

        public override SessionChangedReturn OnSessionChange(SessionChangedInput input)
        {
            if (input.IsHost || input.IsMod_Session)
            {
                sessionFeaturesEnabled = true;
            }
            return new SessionChangedReturn() { SetMod_Session = true };
        }
    }
}

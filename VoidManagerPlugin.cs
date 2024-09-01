using VoidManager;
using VoidManager.MPModChecks;

namespace EjectCallouts
{
    internal class VoidManagerPlugin : VoidPlugin
    {
        public VoidManagerPlugin()
        {
            //If a client joins the game then ask if they can get permission
            Events.Instance.ClientModlistRecieved += EjectPatch.getPermission;

            //If the host starts the game, give them permission 
            Events.Instance.HostStartSession += (_, _) => EjectPatch.HostPermission = true;

            //When the host leaves a game, make sure this detects when the host switches
            Events.Instance.MasterClientSwitched += EjectPatch.hostChange;

            //If a person leaves the game, deny their permission for other games
            Events.Instance.LeftRoom += (_, _) => EjectPatch.HostPermission = false;
        }

        public override string Author => "Gruncle Chuck";

        public override string Description => "Calls out to your allies when you drop an item. Also can tells which item you dropped.";

        public override MultiplayerType MPType => MultiplayerType.Client;
    }
}

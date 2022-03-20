namespace FreeMS;

using Maple;
using Net;

class ChannelSession : MapleSession<ChannelSession, ChannelClient>
{
    public int ChannelId => 1;

    protected override void OnStart()
    {
        Client.SendHello();
    }
}
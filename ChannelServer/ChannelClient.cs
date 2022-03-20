namespace FreeMS;

using System.Net.Sockets;

class ChannelClient : MapleClient
{
    public ChannelClient(Socket socket)
        : base(socket)
    {
    }

    public void SendHello()
    {
        var data = Cryptograph.Initialize();
        Socket.Send(data);
    }
}
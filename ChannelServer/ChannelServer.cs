namespace FreeMS;

using System.Net;
using System.Net.Sockets;

class ChannelServer : MapleServer<ChannelSession>
{
    public ChannelServer()
    {
        var ip = IPAddress.Parse("127.0.0.1");
        var port = 7575;
        EndPoint = new IPEndPoint(ip, port);
    }

    protected override ChannelSession CreateSession(Socket socket)
    {
        var session = new ChannelSession();
        var client = new ChannelClient(socket);
        session.Initialize(this, client);
        return session;
    }
}
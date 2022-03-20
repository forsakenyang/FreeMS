namespace FreeMS;

using System.Net;
using System.Net.Sockets;

class LoginServer : MapleServer<LoginSession>
{
    public LoginServer()
    {
        var ip = IPAddress.Parse("127.0.0.1");
        var port = 9595;
        EndPoint = new IPEndPoint(ip, port);
    }

    protected override LoginSession CreateSession(Socket socket)
    {
        var session = new LoginSession();
        var client = new LoginClient(socket);
        session.Initialize(this, client);
        return session;
    }
}
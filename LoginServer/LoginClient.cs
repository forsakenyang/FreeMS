namespace FreeMS;

using System.Net.Sockets;

class LoginClient : MapleClient
{
    public LoginClient(Socket socket)
        : base(socket)
    {
    }

    public void SendHello()
    {
        var data = Cryptograph.Initialize();
        Socket.Send(data);
    }
}
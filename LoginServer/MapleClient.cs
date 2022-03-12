namespace FreeMS;

using System.Net.Sockets;
using NLog;
using Security;

public class MapleClient
{
    public MapleClient(Socket socket)
    {
        mSocket = socket;
    }

    public void SendHello()
    {
        var data = mCryptograph.Initialize();
        mSocket.Send(data);
    }

    private static readonly ILogger mLog = LogManager.GetLogger("MapleClient");
    private readonly MapleCryptograph mCryptograph = new();
    private readonly Socket mSocket;
}
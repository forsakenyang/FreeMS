namespace FreeMS;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using NLog;

class LoginServer : MapleServer<LoginSession>
{
    protected override LoginSession CreateSession(Socket socket)
    {
        var session = new LoginSession();
        var client = new LoginClient(socket);
        session.Initialize(this, client);
        return session;
    }

    protected override ValueTask OnShutdown()
    {
        mTcpListener.Stop();
        return ValueTask.CompletedTask;
    }

    protected override ValueTask OnStartup()
    {
        var ip = IPAddress.Parse("127.0.0.1");
        mTcpListener = new TcpListener(ip, 9595);
        mTcpListener.Start();
        Task.Run(loop, mCancelSource.Token);
        return ValueTask.CompletedTask;
    }

    private async Task loop()
    {
        while (true)
        {
            var socket = await mTcpListener.AcceptSocketAsync();
            var session = CreateSession(socket);
            session.Start();
            await Task.Yield();
        }
    }

    private static readonly ILogger mLog = LogManager.GetLogger("LoginServer");
    private readonly CancellationTokenSource mCancelSource = new();
    private TcpListener mTcpListener;
}
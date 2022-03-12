namespace FreeMS;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using NLog;

class LoginServer
{
    public void Start()
    {
        if (mIsStarted)
            return;
        mLog.Info("正在启动");
        var ip = IPAddress.Parse("127.0.0.1");
        mTcpListener = new TcpListener(ip, 9595);
        mTcpListener.Start();
        mIsStarted = true;
        Task.Run(loop, mCancelSource.Token);
    }

    public void Stop()
    {
        if (!mIsStarted)
            return;
        mTcpListener.Stop();
        mLog.Info("停止");
    }

    private async Task loop()
    {
        mLog.Info("已启动");
        while (true)
        {
            var socket = await mTcpListener.AcceptSocketAsync();
            var mapleClient = new MapleClient(socket);
            mapleClient.SendHello();
            await Task.Yield();
        }
    }

    private static readonly ILogger mLog = LogManager.GetLogger("LoginServer");
    private readonly CancellationTokenSource mCancelSource = new();
    private bool mIsStarted;
    private TcpListener mTcpListener;
}
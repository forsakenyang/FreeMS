namespace FreeMS.Net;

using NLog;

public interface IMapleSession
{
    void HandlePacket(MaplePacket packet);

    void Initialize(IMapleServer server, MapleClient client);

    void Start();
}

public class MapleSession<TSession, TClient> : IMapleSession
    where TSession : IMapleSession
    where TClient : MapleClient
{
    public TClient Client { get; private set; }

    public MapleServer<TSession> Server { get; private set; }

    protected ILogger Log { get; }

    #region IMapleSession Members

    public void HandlePacket(MaplePacket packet)
    {
        packet.Position = 0;
        packet.ReadShort();

        var cmd = Server.GetCommand(packet.OpCode);
        if (cmd == null)
        {
            Log.Debug($"未处理的指令：0x{packet.OpCode:X2}");
            return;
        }

        try
        {
            cmd.Execute((TSession)(IMapleSession)this, packet);
        }
        catch (Exception e)
        {
            Log.Error($"执行命令{cmd}异常：{e}");
        }
    }

    public void Initialize(IMapleServer server, MapleClient client)
    {
        Server = (MapleServer<TSession>)server;
        Client = (TClient)client;
        Client.Session = this;
    }

    public void Start()
    {
        OnStart();
    }

    #endregion

    public MapleSession()
    {
        Log = LogManager.GetLogger(GetType().Name);
    }

    public void Send(MaplePacket packet)
    {
        Client.Send(packet);
    }

    protected virtual void OnStart()
    {
    }
}
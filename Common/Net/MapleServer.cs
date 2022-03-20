namespace FreeMS;

using System.Net.Sockets;
using System.Reflection;
using System.Text;
using IO;
using Net;
using NLog;

public interface IMapleServer
{
    ValueTask Shutdown();

    ValueTask Startup();
}

public abstract class MapleServer<TSession> : IMapleServer where TSession : IMapleSession
{
    public bool IsStarted { get; private set; }

    protected ILogger Log { get; }

    #region IMapleServer Members

    public async ValueTask Shutdown()
    {
        if (!IsStarted)
            return;
        try
        {
            Log.Info("正在停止");
            await OnStartup();
            IsStarted = true;
            Log.Info("已停止");
        }
        catch (Exception e)
        {
            Log.Error($"停止异常：{e}");
        }
    }

    public async ValueTask Startup()
    {
        if (IsStarted)
            return;
        try
        {
            Log.Info("正在启动");
            registerEncoding();
            loadCommands();
            await OnStartup();
            IsStarted = true;
            Log.Info("已启动");
        }
        catch (Exception e)
        {
            Log.Error($"启动异常：{e}");
        }
    }

    #endregion

    protected MapleServer()
    {
        Log = LogManager.GetLogger(GetType().Name);
    }

    public virtual IMapleCommand<TSession> GetCommand(ushort opCode)
    {
        return mCommands.TryGetValue(opCode, out var cmd) ? cmd : null;
    }

    protected abstract TSession CreateSession(Socket socket);

    protected virtual ValueTask OnShutdown()
    {
        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask OnStartup()
    {
        return ValueTask.CompletedTask;
    }

    private void loadCommands()
    {
        var query = from type in Assembly.GetEntryAssembly()?.GetTypes()
            where !type.IsAbstract && type.IsAssignableTo(typeof(IMapleCommand))
            select type;

        foreach (var type in query)
        {
            var cmd = (IMapleCommand<TSession>)Activator.CreateInstance(type);
            if (cmd == null)
            {
                Log.Warn($"创建指令异常，{type.Name}将不会执行");
                continue;
            }

            if (!mCommands.TryAdd(cmd.Key, cmd))
                Log.Warn($"重复定义的指令，{type.Name}将不会执行");
        }
        Log.Debug($"已加载{mCommands.Count}条命令");
    }

    private void registerEncoding()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        ByteBuffer.TextEncoding = Encoding.GetEncoding("GB2312");
    }

    private readonly Dictionary<ushort, IMapleCommand<TSession>> mCommands = new();
}
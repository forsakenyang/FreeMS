namespace FreeMS.Net;

public interface IMapleCommand
{
    ushort Key { get; }
}

public interface IMapleCommand<in TSession> : IMapleCommand where TSession : IMapleSession
{
    void Execute(TSession session, MaplePacket packet);
}
namespace FreeMS.Net;

using Constants;

public interface IMapleCommand
{
    ushort Key { get; }
}

public interface IMapleCommand<in TSession> : IMapleCommand where TSession : IMapleSession
{
    void Execute(TSession session, MaplePacket packet);
}

public abstract class MapleCommand<TSession> : IMapleCommand<TSession> where TSession : IMapleSession
{
    public ushort Key => (ushort)OpCode;

    public abstract OpCode OpCode { get; }

    #region IMapleCommand<TSession> Members

    public abstract void Execute(TSession session, MaplePacket packet);

    #endregion
}
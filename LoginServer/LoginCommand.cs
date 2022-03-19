namespace FreeMS;

using Constants;
using Net;

abstract class LoginCommand : IMapleCommand<LoginSession>
{
    public ushort Key => (ushort)OpCode;

    public abstract OpCode OpCode { get; }

    #region IMapleCommand<LoginSession> Members

    public abstract void Execute(LoginSession session, MaplePacket packet);

    #endregion
}
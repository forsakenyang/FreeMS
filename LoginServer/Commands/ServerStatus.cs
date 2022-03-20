namespace FreeMS.Commands;

using Constants;
using Net;

class ServerStatus : LoginCommand
{
    public override OpCode OpCode => OpCode.ServerStatus;

    public override void Execute(LoginSession session, MaplePacket packet)
    {
        // 0 = Select world normally
        // 1 = "Since there are many users, you may encounter some..."
        // 2 = "The concurrent users in this world have reached the max"

        var status = (ushort)Constants.ServerStatus.Normal;
        var outPacket = new MaplePacket(ClientOpCode.ServerStatus);
        outPacket.WriteUShort(status);
        session.Send(outPacket);
    }
}
namespace FreeMS.Commands;

using System.Net;
using Constants;
using Net;

class SelectCharacter : LoginCommand
{
    public override OpCode OpCode => OpCode.SelectCharacter;

    public override void Execute(LoginSession session, MaplePacket packet)
    {
        var characterId = packet.ReadInt();

        var ip = IPAddress.Parse("127.0.0.1");
        var port = (ushort)7575; // TODO: server end point

        var outPacket = new MaplePacket(ClientOpCode.ServerIp);
        outPacket.WriteShort(0); // TODO: unknown
        outPacket.WriteIPAddress(ip);
        outPacket.WriteUShort(port);
        outPacket.WriteInt(characterId);
        outPacket.WriteBytes(mUnknownBytes);

        session.Send(outPacket);
    }

    private static readonly byte[] mUnknownBytes = { 1, 0, 0, 0, 0 };
}
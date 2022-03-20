namespace FreeMS.Commands;

using Constants;
using Net;

class PlayerLogin : ChannelCommand
{
    public override OpCode OpCode => OpCode.PlayerLogin;

    public override void Execute(ChannelSession session, MaplePacket packet)
    {
        var playerId = packet.ReadInt();

        // var outPacket = new MaplePacket(ClientOpCode.WrapToMap);
        // outPacket.WriteInt(session.ChannelId);
        // outPacket.WriteByte(0); // TODO: unknown
        // outPacket.WriteByte(1); // TODO: unknown
        // outPacket.WriteByte(1); // TODO: unknown
        // outPacket.WriteShort(0); // TODO: unknown
        // outPacket.WriteBytes(new CharacterRandom().ToByteArray());
        //
        // // character info
        // outPacket.WriteLong(-1); // TODO: unknown
        // outPacket.WriteByte();
        //
        // outPacket.WriteLong(DateTime.UtcNow.ToTimestamp());
        // session.Send(outPacket);
    }
}
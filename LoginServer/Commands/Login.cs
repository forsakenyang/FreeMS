namespace FreeMS.Commands;

using System;
using Constants;
using Net;

class Login : LoginCommand
{
    public override OpCode OpCode => OpCode.Login;

    public override void Execute(LoginSession session, MaplePacket packet)
    {
        var username = packet.ReadString();
        var password = packet.ReadString();

        // TODO:账号名有效性校验

        session.Account = new Account
        {
            CreatedAt = DateTime.Now,
            Id = 1,
            Name = username
        };
        send(session, ErrorCode.Ok);
    }

    private static void send(LoginSession session, ErrorCode code)
    {
        var packet = new MaplePacket(0);

        if (code == ErrorCode.Ok)
        {
            var account = session.Account;
            packet.WriteByte();
            packet.WriteInt(account.Id);
            packet.WriteByte(); // TODO:gender
            packet.WriteBool(false); // TODO: is gm
            packet.WriteByte();
            packet.WriteString(account.Name);
            packet.WriteBytes(mUnknownBytes);
            packet.WriteInt();
            packet.WriteLong();
            packet.WriteString(account.Id.ToString());
            packet.WriteString(account.Name);
            packet.WriteByte(1);
        }
        else
        {
            packet.WriteInt((int)code);
            packet.WriteShort();
        }

        session.Send(packet);
    }

    private static readonly byte[] mUnknownBytes = { 0x00, 0x00, 0x00, 0x03, 0x01, 0x00, 0x00, 0x00, 0xE2, 0xED, 0xA3, 0x7A, 0xFA, 0xC9, 0x01 };
}
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
        sendOk(session);
        sendServerList(session);
        sendEndOfServerList(session);
    }

    private static void sendEndOfServerList(LoginSession session)
    {
        var packet = new MaplePacket(ClientOpCode.ServerList);
        packet.WriteByte(0xFF);
        session.Send(packet);
    }

    private static void sendFail(LoginSession session, ErrorCode code)
    {
        var packet = new MaplePacket(ClientOpCode.LoginStatus);
        packet.WriteInt((int)code);
        packet.WriteShort();
        session.Send(packet);
    }

    private static void sendOk(LoginSession session)
    {
        var packet = new MaplePacket(ClientOpCode.LoginStatus);
        var account = session.Account;
        packet.WriteByte();
        packet.WriteInt(account.Id);
        packet.WriteByte(0xFF); // TODO:gender
        packet.WriteShort(0); // TODO: is gm
        packet.WriteString(account.Name);
        packet.WriteBytes(mUnknownBytes);
        packet.WriteInt();
        packet.WriteLong();
        packet.WriteString(account.Id.ToString());
        packet.WriteString(account.Name);
        packet.WriteByte(1);
        session.Send(packet);
    }

    private static void sendServerList(LoginSession session)
    {
        var packet = new MaplePacket(ClientOpCode.ServerList);

        var serverId = (byte)0; // 0 = Aquilla, 1 = bootes, 2 = cass, 3 = delphinus
        var serverName = "FreeMS";
        var serverFlag = (byte)ServerFlag.Hot;

        packet.WriteByte(serverId);
        packet.WriteString(serverName);
        packet.WriteByte(serverFlag);
        packet.WriteString("活动消息"); // TODO:event message
        packet.WriteShort(100); // TODO: unknown
        packet.WriteShort(100); // TODO: unknown

        var lastChannel = (byte)1;
        packet.WriteByte(lastChannel);
        packet.WriteInt(500); // TODO: unknown

        var load = 1200;
        packet.WriteString($"{serverName}-{serverId}");
        packet.WriteInt(load);
        packet.WriteByte(serverId);
        packet.WriteShort(0);
        packet.WriteShort(0);

        session.Send(packet);
    }

    private static readonly byte[] mUnknownBytes = { 0x00, 0x00, 0x00, 0x03, 0x01, 0x00, 0x00, 0x00, 0xE2, 0xED, 0xA3, 0x7A, 0xFA, 0xC9, 0x01 };
}
namespace FreeMS.Commands;

using System;
using Constants;
using Net;

class CharacterList : LoginCommand
{
    public override OpCode OpCode => OpCode.CharacterList;

    public override void Execute(LoginSession session, MaplePacket packet)
    {
        var worldId = packet.ReadByte();
        var channelId = packet.ReadByte();
        packet.ReadInt(); // TODO: unknown

        var outPacket = new MaplePacket(ClientOpCode.CharacterList);
        outPacket.WriteByte(0); // TODO: unknown
        outPacket.WriteInt(0); // TODO: unknown

        var characters = new[]
        {
            new Character
            {
                Exp = 100,
                Face = 20100,
                Frame = 100,
                Gender = Gender.Male,
                Hair = 30730,
                Id = 1,
                InitialSpawnPoint = 35,
                Job = 200,
                Level = 25,
                MapId = 101000000,
                Name = "test",
                AvailableAp = 0,
                AvailableSp = 0,
                Skin = 2
            }
        };

        outPacket.WriteByte((byte)characters.Length);

        foreach (var character in characters)
        {
            writeStats(outPacket, character);
            writeLook(outPacket, character);
            outPacket.WriteByte(0); // TODO: unknown

            if (character.Job == 900)
                outPacket.WriteByte(2); // TODO: GM
        }

        outPacket.WriteShort(3); // TODO: second pw request
        outPacket.WriteInt(3); // TODO: character slots

        session.Send(outPacket);
    }

    private static void writeLook(MaplePacket packet, Character character)
    {
        packet.WriteByte((byte)character.Gender);
        packet.WriteByte(character.Skin);
        packet.WriteInt(character.Face);
        packet.WriteByte(1); // TODO: mega?
        packet.WriteInt(character.Hair);

        // TODO: equip

        packet.WriteByte(0xFF);

        // TODO: masked items

        packet.WriteByte(0xFF);

        packet.WriteInt(0); // weapon

        // TODO: pet
        for (var i = 0; i < 3; i++)
        {
            packet.WriteInt(0);
        }
    }

    private static void writeStats(MaplePacket packet, Character character)
    {
        packet.WriteInt(character.Id);
        packet.WriteStringFixed(character.Name, 13);
        packet.WriteByte((byte)character.Gender);
        packet.WriteByte(character.Skin);
        packet.WriteInt(character.Face);
        packet.WriteInt(character.Hair);
        for (var i = 0; i < 24; i++)
            packet.WriteByte();
        packet.WriteByte((byte)character.Level);
        packet.WriteShort(character.Job);

        packet.WriteShort(4); // str
        packet.WriteShort(4); // dex
        packet.WriteShort(4); // int
        packet.WriteShort(4); // luk
        packet.WriteShort(100); // hp
        packet.WriteShort(100); // max hp
        packet.WriteShort(100); // mp
        packet.WriteShort(100); // max mp

        packet.WriteShort(character.AvailableAp);
        packet.WriteShort(character.AvailableSp);
        packet.WriteInt(character.Exp);
        packet.WriteShort(character.Frame);
        packet.WriteInt(0); // TODO: unknown
        packet.WriteLong(DateTime.UtcNow.ToTimestamp());
        packet.WriteInt(character.MapId);
        packet.WriteByte((byte)character.InitialSpawnPoint);
    }
}
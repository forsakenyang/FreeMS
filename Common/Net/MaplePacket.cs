namespace FreeMS.Net;

using Constants;
using IO;

public class MaplePacket : ByteBuffer
{
    public ushort OpCode { get; }

    public MaplePacket(ClientOpCode opCode)
        : this((ushort)opCode)
    {
    }

    public MaplePacket(OpCode opCode)
        : this((ushort)opCode)
    {
    }

    public MaplePacket(ushort opCode)
    {
        OpCode = opCode;
        WriteUShort(opCode);
    }

    public MaplePacket(byte[] data)
        : base(data)
    {
        OpCode = ReadUShort();
    }
}
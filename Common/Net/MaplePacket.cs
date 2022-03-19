namespace FreeMS.Net;

using IO;

public class MaplePacket : ByteBuffer
{
    public ushort OpCode { get; }

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
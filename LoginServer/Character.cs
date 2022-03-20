namespace FreeMS;

using Constants;

record Character
{
    public int Exp { get; set; }

    public int Face { get; set; }

    public short Frame { get; set; }

    public Gender Gender { get; set; }

    public int Hair { get; set; }

    public int Id { get; set; }

    public int InitialSpawnPoint { get; set; }

    public short Job { get; set; }

    public short Level { get; set; }

    public int MapId { get; set; }

    public string Name { get; set; }

    public short AvailableAp { get; set; }

    public short AvailableSp { get; set; }

    public byte Skin { get; set; }
}
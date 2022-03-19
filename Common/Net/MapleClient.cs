namespace FreeMS;

using System.Net.Sockets;
using IO;
using Net;
using NLog;
using Security;

public class MapleClient
{
    public IMapleSession Session { get; set; }

    protected MapleCryptograph Cryptograph { get; } = new();

    protected ILogger Log { get; }

    protected Socket Socket { get; }

    public MapleClient(Socket socket)
    {
        Socket = socket;
        mTask = Task.Run(loop, mCancelSource.Token);
        Log = LogManager.GetLogger(GetType().Name);
    }

    public void Close()
    {
        mCancelSource.Cancel();
        Socket.Close();
    }

    public void Send(MaplePacket packet)
    {
        packet.SafeFlip();
        var plain = packet.GetContent();
        var encrypted = Cryptograph.Encrypt(plain);
        Socket.Send(encrypted);
    }

    private async Task loop()
    {
        while (true)
        {
            var received = await Socket.ReceiveAsync(mReceiveBuffer.Array, SocketFlags.None, mCancelSource.Token);
            if (received == 0)
            {
                Close();
                return;
            }

            var processed = 0;
            var reset = false;

            mReceiveBuffer.Limit += received;
            while (mReceiveBuffer.Remaining >= 4)
            {
                var header = mReceiveBuffer.ReadBytes(4);
                var length = AesCryptograph.RetrieveLength(header);

                if (mReceiveBuffer.Remaining < length)
                {
                    mReceiveBuffer.Position -= 4;
                    Buffer.BlockCopy(mReceiveBuffer.Array, mReceiveBuffer.Position, mReceiveBuffer.Array, 0, mReceiveBuffer.Remaining);
                    reset = true;
                    break;
                }

                mReceiveBuffer.Position -= 4;

                var encrypted = mReceiveBuffer.ReadBytes(length + 4);
                var decrypted = Cryptograph.Decrypt(encrypted);
                var packet = new MaplePacket(decrypted);
                Session?.HandlePacket(packet);
                processed += (length + 4);
            }

            mReceiveBuffer.Limit -= processed;

            if (reset)
                mReceiveBuffer.Position = 0;
            else
                mReceiveBuffer.Position = mReceiveBuffer.Limit;
        }
    }

    private readonly CancellationTokenSource mCancelSource = new();
    private readonly ByteBuffer mReceiveBuffer = new() { Limit = 0 };
    private Task mTask;
}
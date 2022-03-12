using System;
using FreeMS.IO;

namespace FreeMS.Security
{
    public class MapleCryptograph : Cryptograph
    {
        private AesCryptograph Encryptograph { get; set; }
        private AesCryptograph Decryptograph { get; set; }

        public byte[] Initialize()
        {
            byte[] receiveIV = BitConverter.GetBytes(Application.Random.Next());
            byte[] sendIV = BitConverter.GetBytes(Application.Random.Next());

            Encryptograph = new AesCryptograph(sendIV, unchecked((short)(0xFFFF - Application.MapleVersion)));
            Decryptograph = new AesCryptograph(receiveIV, Application.MapleVersion);

            using ByteBuffer buffer = new ByteBuffer(16);
            buffer.WriteShort(0x0D); // 13 = MSEA, 14 = GlobalMS, 15 = EMS
            buffer.WriteShort(Application.MapleVersion);
            buffer.WriteString(Application.PatchVersion);
            buffer.WriteBytes(receiveIV);
            buffer.WriteBytes(sendIV);
            buffer.WriteByte(4); // 7 = MSEA, 8 = GlobalMS, 5 = Test Server

            return buffer.Array;
        }

        public override byte[] Encrypt(byte[] data)
        {
            lock (this)
            {
                byte[] result = new byte[data.Length];
                Buffer.BlockCopy(data, 0, result, 0, data.Length);

                byte[] header = this.Encryptograph.GenerateHeader(result.Length);

                BlurCryptograph.Encrypt(result);
                this.Encryptograph.Crypt(result);

                using ByteBuffer buffer = new ByteBuffer(data.Length + 4);
                buffer.WriteBytes(header);
                buffer.WriteBytes(result);

                return buffer.Array;
            }
        }

        public override byte[] Decrypt(byte[] data)
        {
            lock (this)
            {
                using ByteBuffer buffer = new ByteBuffer(data);
                byte[] header = buffer.ReadBytes(4);

                if (!this.Decryptograph.IsValidPacket(header))
                {
                    throw new CryptographyException("Invalid header.");
                }

                byte[] content = buffer.GetContent();

                int length = AesCryptograph.RetrieveLength(header);

                if (content.Length == length)
                {
                    this.Decryptograph.Crypt(content);
                    BlurCryptograph.Decrypt(content);

                    return content;
                }
                else
                {
                    throw new CryptographyException($"Packet length not matching ({content.Length} != {length}).");
                }
            }
        }

        protected override void CustomDispose()
        {
            this.Encryptograph.Dispose();
            this.Decryptograph.Dispose();
        }
    }
}
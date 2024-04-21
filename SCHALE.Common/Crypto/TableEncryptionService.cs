using MersenneTwister;
using SCHALE.Common.Crypto.XXHash;
using System.Buffers.Binary;
using System.Text;

namespace SCHALE.Common.Crypto
{
    public static class TableEncryptionService
    {
        /// <summary>
        /// Used for decrypting .bytes flatbuffers bin. Doesn't work yet
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bytes"></param>
        public static void XOR(string name, byte[] bytes)
        {
            using var xxhash = XXHash32.Create();
            xxhash.ComputeHash(Encoding.UTF8.GetBytes(name));

            var mt = MTRandom.Create((int)xxhash.HashUInt32);
            var key = GC.AllocateUninitializedArray<byte>(sizeof(int));
            BinaryPrimitives.WriteInt32LittleEndian(key, mt.Next() + 1);

            int i = 0;
            int j = 0;
            while (i < bytes.Length)
            {
                if (j == 4)
                {
                    key = key = GC.AllocateUninitializedArray<byte>(sizeof(int));
                    BinaryPrimitives.WriteInt32LittleEndian(key, mt.Next() + 1);
                    j = 0;
                }

                bytes[i++] ^= key[j++];
            }
        }
    }
}

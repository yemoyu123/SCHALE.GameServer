using MersenneTwister;
using SCHALE.Common.Crypto.XXHash;
using System.Text;

namespace SCHALE.Common.Crypto
{
    public static class TableService
    {
        /// <summary>
        /// General password gen by file name, encode to base64 for zips password
        /// </summary>
        /// <param name="key"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] CreatePassword(string key, int length = 20)
        {
            byte[] password = GC.AllocateUninitializedArray<byte>((int)Math.Round((decimal)(length / 4 * 3)));

            using var xxhash = XXHash32.Create();
            xxhash.ComputeHash(Encoding.UTF8.GetBytes(key));

            var mt = MTRandom.Create((int)xxhash.HashUInt32);

            int i = 0;
            while (i < password.Length)
            {
                // IDK why the random result is off by one in this implementation compared to the game's
                Array.Copy(BitConverter.GetBytes(mt.Next() + 1), 0, password, i, Math.Min(4, password.Length - i));
                i += 4;
            }

            return password;
        }
    }
}

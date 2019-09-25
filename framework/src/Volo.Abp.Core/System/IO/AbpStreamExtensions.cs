using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace System.IO
{
    public static class AbpStreamExtensions
    {
        /// <summary>复制到内存流</summary>
        public static async Task<MemoryStream> CopyToMemoryAsync(this Stream stream, long length)
        {
            var memoryStream = new MemoryStream((int)length);
            await stream.CopyToAsync(memoryStream);
            return memoryStream;
        }

        public static byte[] GetAllBytes(this Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static async Task<byte[]> GetAllBytesAsync(this Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
        /// <summary>计算流的MD5</summary>
        public static string ToMD5(this Stream stream, bool useLower = true)
        {
            using (var md5 = MD5.Create())
            {
                var hashBytes = md5.ComputeHash(stream);
                return HashToMd5(hashBytes, useLower);
            }
        }
        public static string ToMd5(this byte[] inputBytes, bool useLower = true)
        {
            using (var md5 = MD5.Create())
            {
                var hashBytes = md5.ComputeHash(inputBytes);
                return HashToMd5(hashBytes, useLower);
            }
        }
        private static string HashToMd5(byte[] hashBytes, bool useLower = true)
        {
            var sb = new StringBuilder(hashBytes.Length * 2);
            var format = useLower ? "x2" : "X2";
            foreach (var hashByte in hashBytes)
            {
                sb.Append(hashByte.ToString(format));
            }
            return sb.ToString();
        }
    }
}

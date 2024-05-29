using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Google.FlatBuffers;
using Ionic.Zip;
using SCHALE.Common.Crypto;

namespace SCHALE.GameServer.Services
{
    // TODO: High priority, cache UnPack-ed table!
    public class ExcelTableService(ILogger<ExcelTableService> _logger)
    {
        private readonly ILogger<ExcelTableService> logger = _logger;
        private readonly Dictionary<Type, object> caches = [];

        private static string? ResourcesFolder;
        private static string? ExcelFolder;

        private static string GetUrl()
        {
            string urlPath;
            if (ResourcesFolder == null)
            {
                var folder = Path.GetDirectoryName(AppContext.BaseDirectory);
                while (true)
                {
                    urlPath = Path.Join(folder, "Resources/url.txt");
                    if (File.Exists(urlPath))
                        break;
                    folder = Path.GetDirectoryName(folder);
                    if (folder == null)
                        throw new FileNotFoundException($"Resources folder is not found.");
                }
                ResourcesFolder = Path.GetDirectoryName(urlPath);
            }
            else
            {
                urlPath = Path.Join(ResourcesFolder, "url.txt");
            }
            string url = File.ReadAllText(urlPath);
            return url + "/";
        }

        private static async Task GetZip()
        {
            string url = GetUrl();
            string filePath = "TableBundles/Excel.zip";
            string zipPath = Path.Combine(ResourcesFolder!, "download", filePath);
            if (File.Exists(zipPath))
                return;
            Directory.CreateDirectory(Path.GetDirectoryName(zipPath)!);
            ExcelFolder = zipPath[..^4];

            using HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync(url + filePath);
            response.EnsureSuccessStatusCode();
            byte[] content = await response.Content.ReadAsByteArrayAsync();
            File.WriteAllBytes(zipPath, content);
            using ZipFile zip = ZipFile.Read(zipPath);
            zip.Password = "/wy5f3hIGGXLOIUDS9DZ";
            foreach (ZipEntry e in zip)
            {
                e.Extract(ExcelFolder, ExtractExistingFileAction.OverwriteSilently);
            }
        }

        public static async Task Init()
        {
            await GetZip();
        }

        /// <summary>
        /// Please <b>only</b> use this to get table that <b>have a respective file</b> (i.e. <c>CharacterExcelTable</c> have <c>characterexceltable.bytes</c>)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public T GetTable<T>()
            where T : IFlatbufferObject
        {
            var type = typeof(T);

            if (caches.TryGetValue(type, out var cache))
                return (T)cache;

            ArgumentNullException.ThrowIfNull(ExcelFolder);
            var bytesFilePath = Path.Join(ExcelFolder, $"{type.Name.ToLower()}.bytes");
            if (!File.Exists(bytesFilePath))
            {
                throw new FileNotFoundException($"bytes files for {type.Name} not found");
            }

            var bytes = File.ReadAllBytes(bytesFilePath);
            TableEncryptionService.XOR(type.Name, bytes);
            var inst = type.GetMethod(
                        $"GetRootAs{type.Name}",
                        BindingFlags.Static | BindingFlags.Public,
                        [typeof(ByteBuffer)]
                    )!
                .Invoke(null, [new ByteBuffer(bytes)]);

            caches.Add(type, inst!);
            logger.LogDebug("{Excel} loaded and cached", type.Name);

            return (T)inst!;
        }
    }

    internal static class ExcelTableServiceExtensions
    {
        public static void AddExcelTableService(this IServiceCollection services)
        {
            services.AddSingleton<ExcelTableService>();
        }
    }
}

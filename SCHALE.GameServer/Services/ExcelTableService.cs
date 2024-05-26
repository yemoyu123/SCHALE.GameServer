using Google.FlatBuffers;
using SCHALE.Common.Crypto;
using System.Reflection;

namespace SCHALE.GameServer.Services
{
    // TODO: High priority, cache UnPack-ed table!
    public class ExcelTableService
    {
        private readonly ILogger<ExcelTableService> logger;
        private readonly Dictionary<Type, object> caches = [];

        public ExcelTableService(ILogger<ExcelTableService> _logger)
        {
            logger = _logger;
        }

        /// <summary>
        /// Please <b>only</b> use this to get table that <b>have a respective file</b> (i.e. <c>CharacterExcelTable</c> have <c>characterexceltable.bytes</c>)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public T GetTable<T>() where T : IFlatbufferObject
        {
            var type = typeof(T);

            if (caches.TryGetValue(type, out var cache))
                return (T)cache;

            var folder = Path.GetDirectoryName(AppContext.BaseDirectory);
            string excelFolder;
            while (true){
                excelFolder = Path.Join(folder, "Resources/excel");
                if (Directory.Exists(excelFolder)) break;
                folder = Path.GetDirectoryName(folder);
                if (folder == null)
                    throw new FileNotFoundException($"Excel folder is not found.");
            }

            var bytesFilePath = Path.Join(excelFolder, $"{type.Name.ToLower()}.bytes");
            if (!File.Exists(bytesFilePath))
            {
                throw new FileNotFoundException($"bytes files for {type.Name} not found");
            }

            var bytes = File.ReadAllBytes(bytesFilePath);
            TableEncryptionService.XOR(type.Name, bytes);
            var inst = type.GetMethod($"GetRootAs{type.Name}", BindingFlags.Static | BindingFlags.Public, [typeof(ByteBuffer)])!.Invoke(null, [new ByteBuffer(bytes)]);

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

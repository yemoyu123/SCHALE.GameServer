using Microsoft.AspNetCore.Mvc;
using SCHALE.Common.Crypto;
using SCHALE.Common.NetworkProtocol;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SCHALE.GameServer.Controllers.Data
{
    [ApiController]
    [Route("/data")]
    public class DataController : ControllerBase
    {

        private readonly string absFolder;
        private readonly ILogger<DataController> logger;
        public DataController(ILogger<DataController> _logger)
        {
            logger = _logger;

            var folder = Path.GetDirectoryName(AppContext.BaseDirectory);
            string dataFolder;
            while (true)
            {
                dataFolder = Path.Join(folder, "Resources/data");
                if (Directory.Exists(dataFolder)) break;
                folder = Path.GetDirectoryName(folder);
                if (folder == null)
                    throw new FileNotFoundException($"Excel folder is not found.");
            }
            absFolder = dataFolder;
        }


        string? AbsPath(string relPath)
        {
            string filePath = Path.Combine(absFolder, relPath);
            if (!System.IO.File.Exists(filePath)) return null;
            logger.LogDebug($"Use our own {relPath}.");
            return filePath;
        }

        [HttpGet("TableBundles/{fileName}")]
        public IActionResult GetTableBundles(string fileName)
        {
            string relPath = $"TableBundles/{fileName}";
            string? filePath = AbsPath(relPath);
            if (filePath == null) return CatchAll(relPath);

            if (fileName.EndsWith(".json"))
            {
                var jsonContent = System.IO.File.ReadAllText(filePath);
                return Content(jsonContent, "application/json");
            }
            if (fileName.EndsWith(".zip"))
            {
                var fileStream = System.IO.File.OpenRead(filePath);
                return File(fileStream, "application/zip", fileName);
            }
            if (fileName.EndsWith(".bytes"))
            {
                var fileStream = System.IO.File.OpenRead(filePath);
                return File(fileStream, "application/octet-stream", fileName);
            }
            return CatchAll(relPath);
        }

        [HttpGet("{*catchAll}")]
        public IActionResult CatchAll(string catchAll)
        {
            logger.LogDebug("Data path: {path}", catchAll);
            string re = $"https://prod-clientpatch.bluearchiveyostar.com/r68_10iazxytt13razwn7x9n_3/{catchAll}";
            return Redirect(re);
            // return RedirectPermanent(re);
        }
    }
}

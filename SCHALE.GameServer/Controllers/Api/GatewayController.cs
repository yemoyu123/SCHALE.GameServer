using Microsoft.AspNetCore.Mvc;
using SCHALE.Common.Crypto;
using System.IO.Compression;
using System.Text;

namespace SCHALE.GameServer.Controllers.Api
{
    [Route("/api/[controller]")]
    public class GatewayController : ControllerBase
    {
        [HttpPost]
        public IResult GatewayRequest()
        {
            var formFile = Request.Form.Files.GetFile("mx");
            if (formFile is null)
                return Results.BadRequest("Expecting an mx file");

            using var reader = new BinaryReader(formFile.OpenReadStream());

            // CRC + Protocol type conversion (?)
            reader.BaseStream.Position = 12;

            byte[] compressedPayload = reader.ReadBytes((int)reader.BaseStream.Length - 12);
            XOR.Crypt(compressedPayload, 0xD9);
            using var gzStream = new GZipStream(new MemoryStream(compressedPayload), CompressionMode.Decompress);
            using var payloadMs = new MemoryStream();
            gzStream.CopyTo(payloadMs);

            return Results.Text(Encoding.UTF8.GetString(payloadMs.ToArray()), "application/json");
        }
    }
}

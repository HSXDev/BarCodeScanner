using BarCodeScanner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using IronBarCode;

namespace BarCodeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarCodeController : ControllerBase
    {
        [Route("GetPDF")]
        [HttpPost]
        public IActionResult GetPDF()
        {
            try
            {
                var file = Request.Form.Files[0];
                string barcodes = "";
                
                if (file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        barcodes = GetBarCodes(stream);
                    }

                    return Ok(barcodes);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        private string GetBarCodes(Stream file)
        {
           return BarCodeAccess.ScanEANPdf(file);
        }
    }
}
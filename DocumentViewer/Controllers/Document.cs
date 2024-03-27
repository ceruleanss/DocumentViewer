using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DocumentViewer.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DocumentViewer.Controllers
{

    [Route("document")]
    public class Document : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        public Document(IWebHostEnvironment env)
        {
            _env = env;
        }
        [HttpGet("invoice/view")]
        public IActionResult InvoiceView([FromQuery] string f)
        {
            try
            {
                Console.WriteLine(f);
                var findfile = DocumentDropper.FindFileRecursively(DocumentDropper.SetDirectory(ModuleDirectory.Closing, ClosingModuleAction.Document, isProduction: _env.IsProduction()), f) ?? throw new Exception("File not found, " + f);

                var content = "application/pdf";
                var file = System.IO.File.ReadAllBytes(findfile);
                return File(file, content);
            }
            catch (Exception)
            {
                return BadRequest(new ResponseModel
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Message = "Bad Request",
                    Data = null
                });
            }

        }
    }

    public class ResponseModel
    {
        public int Status { get; set; }
        public object Message { get; set; } = null!;
        public object Data { get; set; } = null!;
    }
}


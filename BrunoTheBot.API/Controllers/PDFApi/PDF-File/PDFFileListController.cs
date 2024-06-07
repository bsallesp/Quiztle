using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PDFFileListController : ControllerBase
    {
        private readonly string pdfDirectory;

        public PDFFileListController(IWebHostEnvironment env, IConfiguration configuration)
        {
            if (env.IsDevelopment()) pdfDirectory = configuration["PDFDirectory"] ?? throw new InvalidOperationException();
            else pdfDirectory = Environment.GetEnvironmentVariable("PDF_DIRECTORY") ?? "/app/bucket";
        }

        [HttpGet("list-files")]
        public ActionResult<APIResponse<PDFFileListResponse>> ListPDFFiles()
        {
            try
            {
                Console.WriteLine($"Execution path: {pdfDirectory}");
                if (!Directory.Exists(pdfDirectory))
                {
                    Console.WriteLine("Getting this code: ");
                    return BadRequest(new APIResponse<PDFFileListResponse>
                    {
                        Status = CustomStatusCodes.NotFound,
                        Data = new PDFFileListResponse(),
                        Message = "PDF directory does not exist."
                    });
                }

                var pdfFiles = new List<string>();
                var pdfFilesWithPaths = new List<string>();

                foreach (var file in Directory.GetFiles(pdfDirectory, "*.pdf"))
                {
                    pdfFiles.Add(Path.GetFileName(file));
                    pdfFilesWithPaths.Add(Path.GetFullPath(file));
                }

                Console.WriteLine("Total pdfFiles: " + pdfFiles.Count);
                Console.WriteLine("Total pdfFilesWithPaths: " + pdfFilesWithPaths.Count);

                var response = new PDFFileListResponse
                {
                    FileNames = pdfFiles,
                    FilePaths = pdfFilesWithPaths
                };

                return Ok(new APIResponse<PDFFileListResponse>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = response,
                    Message = "Files retrieved successfully."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Getting catch: ");
                return StatusCode(500, new APIResponse<PDFFileListResponse>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new PDFFileListResponse(),
                    Message = $"An error occurred while retrieving files: {ex.Message}"
                });
            }
        }
    }

    public class PDFFileListResponse
    {
        [JsonPropertyName(nameof(FileNames))]
        public List<string> FileNames { get; set; } = [];

        [JsonPropertyName(nameof(FilePaths))]
        public List<string> FilePaths { get; set; } = [];
    }
}
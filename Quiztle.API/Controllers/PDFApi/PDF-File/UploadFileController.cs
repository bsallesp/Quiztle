﻿using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Quiztle.API.Controllers.PDFApi.PDF_File
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadFileController : ControllerBase
    {
        private readonly string pdfDirectory;

        public UploadFileController(IWebHostEnvironment env, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                pdfDirectory = configuration["UploadPDFDirectory"] ?? "C:/bucket/";
                Console.WriteLine($"Development environment detected. PDF directory set to: {pdfDirectory}");
            }
            else if (env.IsProduction())
            {
                pdfDirectory = Environment.GetEnvironmentVariable("UploadPDFDirectory") ?? "/bucket";
                Console.WriteLine($"Production environment detected. PDF directory set to: {pdfDirectory}");
            }
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse<string>>> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                Console.WriteLine("No file uploaded or file is empty.");
                return BadRequest(new APIResponse<string>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = string.Empty,
                    Message = "No file uploaded or file is empty."
                });
            }

            Console.WriteLine($"UploadFile endpoint hit with file: {file.FileName}");

            try
            {
                if (!Directory.Exists(pdfDirectory))
                {
                    Console.WriteLine($"PDF directory does not exist. Creating directory: {pdfDirectory}");
                    Directory.CreateDirectory(pdfDirectory);
                }
                else
                {
                    Console.WriteLine($"Using existing PDF directory: {pdfDirectory}");
                }

                var filePath = Path.Combine(pdfDirectory, file.FileName);
                Console.WriteLine($"Saving file to: {filePath}");

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                Console.WriteLine("File uploaded successfully.");
                return Ok(new APIResponse<string>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = filePath,
                    Message = "File uploaded successfully."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while uploading the file: {ex.Message}");
                return StatusCode(500, new APIResponse<string>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = string.Empty,
                    Message = $"An error occurred while uploading the file: {ex.Message}"
                });
            }
        }
    }
}
